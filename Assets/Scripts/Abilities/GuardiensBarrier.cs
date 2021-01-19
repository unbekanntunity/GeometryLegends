using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuardiensBarrier", menuName = "Ability/GuardiensBarrier")]
public class GuardiensBarrier : Skill
{
    public override void CastSkill(GameObject user, GameObject target)
    {
        Debug.Log("barrier, Single, nned");
        user.GetComponent<GetStats>().selectedSkill = this;
    }

    public override void CastSkill(GameObject user)
    {
        Debug.Log("barrier, Single, noneed");
        user.GetComponent<GetStats>().selectedSkill = this;
    }

    public override void CastSkill(GameObject user, Vector3 targetPoint)
    {
        Debug.Log("barrier, Single, noneed");
        user.GetComponent<GetStats>().selectedSkill = this;
    }
}
