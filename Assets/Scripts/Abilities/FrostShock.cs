using System.Collections;
using UnityEngine;

public class FrostShock : Skill
{
    public GameObject iceBlock;
    public GameObject marker;
    public float abilityDelay;

    public override void CastSkill(GameObject user, GameObject target)
    {
        if (onCooldown)
            return;

        onCooldown = true;
        StartCoroutine(CoolDown());

        var markerObject = Instantiate(
           marker,
           user.GetComponent<Movement>().WeaponPos.transform.GetChild(0).gameObject.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
           user.GetComponent<Movement>().WeaponPos.transform.rotation);

        Destroy(marker, abilityDelay);

        var iceBlockClone = Instantiate(
           iceBlock,
           user.GetComponent<Movement>().WeaponPos.transform.GetChild(0).gameObject.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
           user.GetComponent<Movement>().WeaponPos.transform.rotation
           );
    }

    public override void CastSkill(GameObject user, Vector3 targetPoint)
    {

        if (onCooldown)
            return;

        onCooldown = true;
        StartCoroutine(CoolDown()); 
        
        var markerObject = Instantiate(
             marker,
             user.GetComponent<Movement>().WeaponPos.transform.GetChild(0).gameObject.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
             user.GetComponent<Movement>().WeaponPos.transform.rotation);

        Destroy(marker, abilityDelay);
        StartCoroutine(SummonIceBlock(user));

    }

    IEnumerator SummonIceBlock(GameObject user)
    {
        yield return new WaitForSecondsRealtime(abilityDelay);

        var iceBlockClone = Instantiate(
           iceBlock,
           user.GetComponent<Movement>().WeaponPos.transform.GetChild(0).gameObject.transform.position + user.GetComponent<Movement>().WeaponPos.transform.forward * 0.5f,
           user.GetComponent<Movement>().WeaponPos.transform.rotation
          );
    }
}
