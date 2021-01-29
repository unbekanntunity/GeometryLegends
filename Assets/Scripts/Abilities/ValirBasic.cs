using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValirBasic : Skill
{
    public GameObject projectileObject;

    public override void CastSkill(GameObject user, GameObject target)
    {
        var bullet = Instantiate(
           projectileObject,
           user.GetComponent<Movement>().WeaponPos.transform.GetChild(0).gameObject.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
           user.GetComponent<Movement>().WeaponPos.transform.rotation
           );

        bullet.GetComponent<Projectile>().user = user;
    }

    public override void CastSkill(GameObject user, Vector3 targetPoint)
    {
        var bullet = Instantiate(
            projectileObject,
            user.GetComponent<Movement>().WeaponPos.transform.GetChild(0).gameObject.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
            user.GetComponent<Movement>().WeaponPos.transform.rotation
            );

        Projectile projectile = bullet.GetComponent<Projectile>();

        projectile.user = user;
        projectile.targetPoint = targetPoint;
    }
}
