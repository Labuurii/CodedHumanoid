using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LocalNavMeshNetworkProxy))]
[RequireComponent(typeof(EquipmentController))]
[RequireComponent(typeof(Bag))]
[RequireComponent(typeof(CharacterRotator))]
public class LoopingPlayerScriptEnv : NetworkBehaviour {

    const string WindowsZeroBranePath = "zerobrane_windows/zbstudio.exe";
    const string MacOSZeroBranePath = "zerobrane_mac/zbstudio.exe";
    const string LinuxZeroBranePath = "zerobrane_linux/zbstudio.exe";

    OperatingSystemFamily os;
    string base_dir;

    Thread code_updater_thread;
    CancellationTokenSource cancel_source;
    Snapshot current_snapshot;

    //Data sources + command
    NavMeshAgent agent;
    LocalNavMeshNetworkProxy local_navmesh_network;
    EquipmentController equipment_controller;
    Bag bag;
    CharacterRotator rotator;

    public float ElevationDataDistance;

    [Multiline]
    public volatile string Code;

    public int NewCodeHasBeenSubmitted; //TODO: Serious implementation should use a threadsafe queue just to be safe.

    public UnityEvent OnEditorOpen;
    public UnityEvent OnEditorClose;
    public UnityEvent OnCodeChanged;

	void Start () {
		if(isLocalPlayer)
        {
            os = SystemInfo.operatingSystemFamily;
            if (Application.isEditor)
                base_dir = Directory.GetCurrentDirectory();
            else
                base_dir = AppDomain.CurrentDomain.BaseDirectory;
            agent = GetComponent<NavMeshAgent>();
            local_navmesh_network = GetComponent<LocalNavMeshNetworkProxy>();
            equipment_controller = GetComponent<EquipmentController>();
            bag = GetComponent<Bag>();
            rotator = GetComponent<CharacterRotator>();

            new Script(); //Only to make sure script env dll is loaded on main thread.

            update_script_data();
            cancel_source = new CancellationTokenSource();

            { //Try to execute the environment over and over again
                var thread = new Thread(run_env);
                thread.Name = "Player Scripting Env";
                thread.IsBackground = true;
                thread.Start();
            }

            { //Create a temporary file and open that with the zerobrane editor
                var thread = code_updater_thread = new Thread(code_editor_watcher);
                thread.Name = "Code Editor Watcher";
                thread.IsBackground = true;
                thread.Start();
            }
        }
	}

    private void code_editor_watcher()
    {
        string tmp_file = null;
        try
        {
            var cancel_token = cancel_source.Token;
            tmp_file = create_tmp_code_file();

            string brane_path;
            switch (os)
            {
                case OperatingSystemFamily.Linux:
                    brane_path = LinuxZeroBranePath;
                    break;
                case OperatingSystemFamily.MacOSX:
                    brane_path = MacOSZeroBranePath;
                    break;
                case OperatingSystemFamily.Windows:
                    brane_path = WindowsZeroBranePath;
                    break;
                default:
                    throw new Exception("Unhandled enum value " + SystemInfo.operatingSystemFamily);
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(base_dir, brane_path),
                    Arguments = '"' + tmp_file + '"'
                }
            };
            process.Start();
            var fs = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(tmp_file),
                Filter = Path.GetFileName(tmp_file)
            };
            fs.EnableRaisingEvents = true;

            for (; ; )
            {
                if (cancel_token.IsCancellationRequested)
                    break;
                var change = fs.WaitForChanged(WatcherChangeTypes.All, 100);
                if (!change.TimedOut)
                {
                    switch (change.ChangeType)
                    {
                        case WatcherChangeTypes.Created:
                        case WatcherChangeTypes.Deleted:
                        case WatcherChangeTypes.Renamed:
                            tmp_file = create_tmp_code_file();
                            goto case WatcherChangeTypes.Changed;
                        case WatcherChangeTypes.Changed:
                            Code = File.ReadAllText(tmp_file);
                            NewCodeHasBeenSubmitted = 1;
                            MainThreadEventQueue.Enqueue(() => OnCodeChanged.Invoke());
                            break;
                        default:
                            throw new Exception("Unhandled enum value " + change.ChangeType);
                    }
                }
            }

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        } finally
        {
            UnityEngine.Debug.Log("Stopped looking for code changes.");
            MainThreadEventQueue.Enqueue(() => OnEditorClose.Invoke());

            try
            {
                if(!string.IsNullOrEmpty(tmp_file))
                    File.Delete(tmp_file);
            } catch { }
        }


    }

    private string create_tmp_code_file()
    {
        string tmp_file = FileOps.CreateTmpFile(".lua");
        File.WriteAllText(tmp_file, Code);
        return tmp_file;
    }

    private void OnDestroy()
    {
        if(cancel_source != null)
        {
            cancel_source.Cancel();
        }
    }

    private void LateUpdate()
    {
        if(isLocalPlayer)
        {
            update_script_data();
        }
    }

    private void update_script_data()
    {
        var new_snapshot = new Snapshot();

        {
            var ps = new_snapshot.player_state;
            ps.position = transform.position;
            ps.destination = agent.destination;
            ps.rotation = transform.rotation;
            var weapon_controller = equipment_controller.EquippedWeaponController;
            if(weapon_controller != null)
            {
                ps.equipped_weapon = new Weapon(weapon_controller);
            }

            var weapons_in_inventory = bag.GetWeapons();
            ps.weapons_in_inventory = new Weapon[weapons_in_inventory.Count];
            for(var i = 0; i < weapons_in_inventory.Count; ++i)
            {
                var c = weapons_in_inventory[i];
                ps.weapons_in_inventory[i] = new Weapon(c);
            }
        }

        var env = new_snapshot.env_state;

        {
            var weapons_on_ground = DroppedWeaponsStore.Instance.WeaponsOnGround;
            env.weapons = new Weapon[weapons_on_ground.Count];
            for (var i = 0; i < weapons_on_ground.Count; ++i)
            {
                var c = weapons_on_ground[i];
                env.weapons[i] = new Weapon(c);
            }
        }

        {
            var bullets_in_air = BulletsStore.Instance.Bullets;
            env.bullets_in_air = new Bullet[bullets_in_air.Count];
            for(var i = 0; i < bullets_in_air.Count; ++i)
            {
                var go = bullets_in_air[i];
                env.bullets_in_air[i] = new Bullet
                {
                    position = go.transform.position,
                    direction = go.transform.rotation
                };
            }                
        }

        {
            var other_players = OtherPlayersStore.Instance.Players;
            env.other_players = new OtherPlayer[other_players.Count];
            for(var i = 0; i < other_players.Count; ++i)
            {
                var data = other_players[i];
                env.other_players[i] = new OtherPlayer
                {
                    name = data.name,
                    position = data.transform.position,
                    rotation = data.transform.rotation
                };
            }
        }

        {
            
            var e = env.elevation;
            e.forward = raycast_navmesh(transform.forward);
            e.right = raycast_navmesh(transform.right);
            e.left = raycast_navmesh(-transform.right);


            var water_t = WaterDecl.Instance?.transform;
            if(water_t != null)
            {
                var water_height = water_t.position.y;
                e.is_forward_in_water = e.forward < water_height;
                e.is_right_in_water = e.right < water_height;
                e.is_left_in_water = e.left < water_height;
            }
            
        }

        current_snapshot = new_snapshot;
    }

    private float raycast_navmesh(Vector3 direction)
    {
        NavMeshHit hit;
        var blocked = NavMesh.Raycast(transform.position, transform.position + direction * ElevationDataDistance, out hit, NavMesh.AllAreas);
        if (blocked)
            UnityEngine.Debug.LogError("Navmesh raycast got blocked");
        if (!hit.hit)
            return float.NaN;
        return hit.position.y;
    }

    class Snapshot
    {
        public PlayerState player_state = new PlayerState();
        public EnvState env_state = new EnvState();
    }

    class EnvState
    {
        public Weapon[] weapons;
        public Bullet[] bullets_in_air;
        public OtherPlayer[] other_players;
        public Elevation elevation = new Elevation();
    }

    class Weapon
    {
        [MoonSharpHidden]
        public readonly WeaponController controller;

        public string name;
        public Vector3 position;
        public WeaponType type;

        [MoonSharpHidden]
        public Weapon(WeaponController weapon)
        {
            controller = weapon;
            name = weapon.Name;
            position = weapon.transform.position;
            type = weapon.WeaponType;
        }
    }

    class Bullet
    {
        public Vector3 position;
        public Quaternion direction;
    }

    class OtherPlayer
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    class Elevation
    {
        public float forward, right, left;
        public bool is_forward_in_water, is_right_in_water, is_left_in_water;
    }

    class PlayerState
    {
        public Vector3 position, destination;
        public Quaternion rotation;
        public Weapon equipped_weapon;
        public Weapon[] weapons_in_inventory;

        [MoonSharpHidden]
        public Vector3? new_dest;
        public void SetDestination(Vector3 new_d)
        {
            new_dest = new_d;
        }
        public void SetDestination(float x, float y, float z)
        {
            SetDestination(new Vector3(x, y, z));
        }

        [MoonSharpHidden]
        public bool use_weapon;
        public void UseWeapon()
        {
            use_weapon = true;
        }

        [MoonSharpHidden]
        public Weapon weapon_to_pick_up;
        public void PickUpWeapon(Weapon weapon)
        {
            weapon_to_pick_up = weapon;
        }

        [MoonSharpHidden]
        public Weapon weapon_to_drop;
        public void DropWeapon(Weapon weapon)
        {
            weapon_to_drop = weapon;
        }

        [MoonSharpHidden]
        public Weapon weapon_to_equip;
        public void EquipWeapon(Weapon weapon)
        {
            weapon_to_equip = weapon;
        }

        [MoonSharpHidden]
        public Quaternion? new_rotation;
        public void Aim(Quaternion rotation)
        {
            new_rotation = rotation;
        }
        public void Aim(Vector3 euler)
        {
            Aim(Quaternion.Euler(euler));
        }
        public void Aim(float x, float y, float z)
        {
            Aim(x, y);
        }
        public void Aim(float x, float y)
        {
            Aim(new Vector3
            {
                x = x,
                y = y,
                z = 0
            });
        }
    }

    void run_env()
    {
        var engine = new Script();
        var cancel_token = cancel_source.Token;

        var ud_v3 = UserData.RegisterType(typeof(Vector3));
        var ud_q = UserData.RegisterType(typeof(Quaternion));
        var ud_debug = UserData.RegisterType(typeof(UnityEngine.Debug));
        UserData.RegisterType(typeof(PlayerState));
        UserData.RegisterType(typeof(EnvState));
        UserData.RegisterType(typeof(Weapon));
        var ud_weapon_type = UserData.RegisterType(typeof(WeaponType));
        UserData.RegisterType(typeof(Bullet));
        UserData.RegisterType(typeof(OtherPlayer));
        UserData.RegisterType(typeof(Elevation));
        for(; ; )
        {
            var timer = new CyclicTimer(1f / 10f);

            try
            {
                var local_snapshot = current_snapshot; //Really important local_* is used instead of snapshot here. Basicly the state is viewed as consumed from here on.
                var local_ps = local_snapshot.player_state;
                var local_env = local_snapshot.env_state;
                //Set up state
                engine.Globals["vector3"] = UserData.CreateStatic(ud_v3);
                engine.Globals["quaternion"] = UserData.CreateStatic(ud_q);
                engine.Globals["WEAPON_TYPE"] = UserData.CreateStatic(ud_weapon_type);
                engine.Globals["debug"] = UserData.CreateStatic(ud_debug); //TODO: Remove this in production
                engine.Globals["player"] = local_ps;
                engine.Globals["environment"] = local_env;

                //Run script
                engine.DoString(Code, codeFriendlyName: "Script");

                //Run commands from script
                if (local_ps.new_dest.HasValue) //SetDestination
                {
                    var v = local_ps.new_dest.Value;
                    local_ps.new_dest = null;
                    MainThreadEventQueue.Enqueue(() => local_navmesh_network.SetDestination(v));
                }
                if(local_ps.use_weapon) //UseWeapon
                {
                    if(local_ps.equipped_weapon != null)
                    {
                        local_ps.use_weapon = false;
                        MainThreadEventQueue.Enqueue(() => equipment_controller.CmdUseWeapon());
                    }
                }
                if(local_ps.weapon_to_pick_up != null) //PickUpWeapon
                {
                    var weapon = local_ps.weapon_to_pick_up;
                    local_ps.weapon_to_pick_up = null;
                    MainThreadEventQueue.Enqueue(() => bag.PickUp(weapon.controller)); //TODO: Error message: weapon already picked up or owned by other
                }
                if(local_ps.weapon_to_drop != null) //DropWeapon
                {
                    var weapon = local_ps.weapon_to_drop;
                    local_ps.weapon_to_drop = null;
                    MainThreadEventQueue.Enqueue(() => bag.Drop(weapon.controller)); //TODO: Error message: weapon not ours
                }
                if(local_ps.weapon_to_equip != null) //EquipWeapon
                {
                    var weapon = local_ps.weapon_to_equip;
                    local_ps.weapon_to_equip = null;
                    MainThreadEventQueue.Enqueue(() => equipment_controller.EquipWeapon(weapon.controller)); //TODO: Error message: weapon not picked up
                }
                if(local_ps.new_rotation.HasValue) //Aim
                {
                    var rot = local_ps.new_rotation.Value;
                    local_ps.new_rotation = null;
                    MainThreadEventQueue.Enqueue(() => rotator.CmdSetRotation(rot));
                }

                timer.WaitUntilNextFrame();
                if (cancel_token.IsCancellationRequested)
                    break;
            } catch(SyntaxErrorException e)
            {
                UnityEngine.Debug.Log(e.DecoratedMessage);
                wait_until_new_code_has_been_submitted(timer);
            } catch(ScriptRuntimeException e)
            {
                UnityEngine.Debug.Log(e.DecoratedMessage);
                wait_until_new_code_has_been_submitted(timer);
            } catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                wait_until_new_code_has_been_submitted(timer);
            }
        }

        UnityEngine.Debug.Log("Ended running scripts");
    }

    private void wait_until_new_code_has_been_submitted(CyclicTimer timer)
    {
        for (; ; )
        {
            timer.WaitUntilNextFrame();
            if (has_new_code_been_submitted())
            {
                break;
            }
        }
    }

    private bool has_new_code_been_submitted()
    {
        return Interlocked.CompareExchange(ref NewCodeHasBeenSubmitted, 0, 1) == 1;
    }
}
