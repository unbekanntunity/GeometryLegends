using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public static void DealDamage(GetStats user, GetStats target)
    {
        target.hero.currentHealth -= user.lastUsedSkill.baseDamage;
    }
}
