using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuardiensBarrier", menuName = "Ability/GuardiensBarrier")]
public class GuardiensBarrier : Skill
{
    public override void CastSkill(GameObject user, GameObject target)
    {
        user.GetComponent<GetStats>().selectedSkill = this;
    }
}
