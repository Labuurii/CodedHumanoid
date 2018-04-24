using ArenaServices;
using Grpc.Core;
using ServerClientShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingClientHL : MonoBehaviour
{
    MatchMakingClientLL client;
    Arena selected_arena;
    MatchMode selected_match_mode;
    TeamSize selected_team_size;

    public BaseClientHL BaseClient;
    public DisplayBinaryQuestion BinaryQuestion;
    public GameObject[] QueueRelatedObjects;
    public Button[] MatchModeButtons;
    public Button[] TeamSizeButtons;
    public Button QueueBtn, CancelQueueBtn;
    public RectTransform ArenasParent;

    public Button ArenaButtonPrefab;

    private void Start()
    {
        QueueBtn.onClick.AddListener(on_queue_click);
        CancelQueueBtn.onClick.AddListener(on_cancel_queue_click);

        {
            var names = Enum.GetNames(typeof(Arena));
            var values = (Arena[])Enum.GetValues(typeof(Arena));
            for (var i = 0; i < names.Length; ++i)
            {
                var name = names[i];
                var value = values[i];
                var btn_go = Instantiate(ArenaButtonPrefab.gameObject, ArenasParent);
                var btn = btn_go.GetComponent<Button>();
                var text = btn_go.GetComponentInChildren<Text>();
                text.text = name; //TRANSLATE
                btn.onClick.AddListener(() => on_arena_button_click(value));
            }
        }

        {
            Debug.Log("MatchMode:");
            var values = (MatchMode[])Enum.GetValues(typeof(MatchMode));
            Debug.Assert(values.Length == MatchModeButtons.Length, "MatchMode buttons has to match arena buttons.");
            for(var i = 0; i < MatchModeButtons.Length; ++i)
            {
                var btn = MatchModeButtons[i];
                var mode = values[i];
                Debug.Log(mode + " => " + btn.GetComponentInChildren<Text>().text);
                btn.onClick.AddListener(() => on_match_mode_btn_clicked(mode));
            }
        }

        {
            Debug.Log("TeamSize:");
            var values = (TeamSize[])Enum.GetValues(typeof(TeamSize));
            Debug.Assert(values.Length == TeamSizeButtons.Length, "TeamSize buttons has to match arena buttons.");
            for (var i = 0; i < TeamSizeButtons.Length; ++i)
            {
                var btn = TeamSizeButtons[i];
                btn.gameObject.SetActive(false);
                var size = values[i];
                Debug.Log(size + " => " + btn.GetComponentInChildren<Text>().text);
                btn.onClick.AddListener(() => on_team_size_selected(size));
            }
        }
    }

    private void on_team_size_selected(TeamSize size)
    {
        selected_team_size = size;
    }

    private void on_match_mode_btn_clicked(MatchMode mode)
    {
        selected_match_mode = mode;
    }

    private void on_arena_button_click(Arena arena)
    {
        selected_arena = arena;

        //Activate teamsize buttons accordingly
        {
            var values = (TeamSize[])Enum.GetValues(typeof(TeamSize));
            Debug.Assert(values.Length == TeamSizeButtons.Length, "TeamSize buttons has to match arena buttons.");
            for (var i = 0; i < TeamSizeButtons.Length; ++i)
            {
                var btn = TeamSizeButtons[i];
                var size = values[i];
                btn.gameObject.SetActive(ArenaOps.IsMatchSizeSupported(arena, size));
            }
        }   
    }

    private void on_cancel_queue_click()
    {
        client.Cancel();
    }

    private void on_queue_click()
    {
        client.Queue(selected_arena, selected_match_mode, selected_team_size);
    }

    public void Connect(Channel channel, Guid session_token)
    {
        if(client == null)
        {
            client = new MatchMakingClientLL(channel, session_token);
            client.OnDisconnected += on_disconnected;
            client.OnNotAuthenticated += on_not_authenticated;
            client.OnStateChanged += on_queue_state_changed;
        }
    }

    private void on_queue_state_changed(QueueState new_state)
    {
        switch(new_state)
        {
            case QueueState.NotQueued:
                set_objects_active(QueueRelatedObjects, false);
                break;
            case QueueState.ArenaJoinable:
                Debug.Log("Time to join arena!!!");
                break;
            case QueueState.AwaitingOtherPlayers:
                set_objects_interactable(QueueRelatedObjects, false);
                break;
            case QueueState.InArena:
                //What should I do here?
                //TODO: What was the semantics between ArenaJoinable and InArena??
                break;
            case QueueState.Queued:
                set_objects_active(QueueRelatedObjects, true);
                set_objects_interactable(QueueRelatedObjects, true);
                break;
            case QueueState.QueuePopped:
                BinaryQuestion.Display("Queue popped.", "Accept?", () => answer_queue_pop(true), () => answer_queue_pop(false));
                break;
            default:
                throw new Exception("Unhandled enum value " + new_state);
        }
    }

    private void set_objects_interactable(GameObject[] objects, bool v)
    {
        foreach (var obj in objects)
        {
            var selectable = obj.GetComponent<UnityEngine.UI.Selectable>();
            if(selectable != null)
                selectable.interactable = v;
        }
    }

    private void answer_queue_pop(bool v)
    {
        client.AnswerQueuePop(v);
    }

    private static void set_objects_active(GameObject[] objects, bool v)
    {
        foreach (var obj in objects)
            obj.SetActive(v);
    }

    private void on_not_authenticated()
    {
        throw new NotImplementedException();
    }

    private void on_disconnected(bool internal_error, StatusCode? status)
    {
        throw new NotImplementedException();
    }
}
