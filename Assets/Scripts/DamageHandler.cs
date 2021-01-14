using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public static void DealDamage(GetStats user, GetStats target)
    {
        Debug.Log("A");
        if(target.hero.currentHealth > 0)
            target.hero.currentHealth -= user.selectedSkill.baseDamage;
    }
}
