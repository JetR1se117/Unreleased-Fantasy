using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HotKeyAbilitySystem : MonoBehaviour
{
    private List<HotkeyAbility> hotkeyAbilityList;
    public enum AbilityType 
    { 
        Magic1,
        Magic2,
        Magic3,
        Magic4,
    }
    
    public HotKeyAbilitySystem()
    {
        hotkeyAbilityList = new List<HotkeyAbility>();
    //    hotkeyAbilityList.Add(new HotkeyAbility { abilityType = AbilityType.Magic1, activateAbilityAction = () =>  })
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
    }

    public class HotkeyAbility {
        public AbilityType abilityType;
        public Action activateAbilityAction;
    }
}
