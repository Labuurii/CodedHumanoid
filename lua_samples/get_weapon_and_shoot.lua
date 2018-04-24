if player.equipped_weapon then
  debug.Log("Uses weapon")
  player.UseWeapon()
  
elseif #player.weapons_in_inventory > 0 then
  debug.Log("Equips weapon")
  player.EquipWeapon(player.weapons_in_inventory[1])
  
else
  local weapons = environment.weapons
  local closest_weapon = nil
  local closest_distance = 10000
  for i, w in pairs(weapons) do 
    local d = vector3.Distance(player.position, w.position)
    if d < closest_distance then
      closest_weapon = w
      closest_distance = d
    end
  end

  if closest_weapon then
    debug.Log("Found weapon")
    player.SetDestination(closest_weapon.position)

    player.PickUpWeapon(closest_weapon)
  end
end
