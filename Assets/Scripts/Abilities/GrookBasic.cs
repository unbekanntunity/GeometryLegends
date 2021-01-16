using UnityEngine;

[CreateAssetMenu(fileName = "NormalBasicProjectile", menuName = "Ability/NormalBasicProjectile")]
public class GrookBasic : Skill
{
    public GameObject projectile;

    public override void CastSkill(GameObject user, GameObject target)
    {
        var bullet = Instantiate(
                projectile,
                user.GetComponent<Movement>().WeaponPos.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
                user.GetComponent<Movement>().WeaponPos.transform.rotation
                );

        bullet.GetComponent<Projectile>().user = user;
    }

    public override void CastSkill(GameObject user)
    {
        var bullet = Instantiate(
            projectile,
            user.GetComponent<Movement>().WeaponPos.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
            user.GetComponent<Movement>().WeaponPos.transform.rotation
            );

        bullet.GetComponent<Projectile>().user = user;
    }
}
