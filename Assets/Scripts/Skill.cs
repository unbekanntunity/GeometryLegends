using System.Collections;
using UnityEngine;

public enum SkillType
{
    AoE, SingleTarget
}

public abstract class Skill : MonoBehaviour
{
    public DamageType damageType;
    public SkillType skillType;

    public string skillName;
    public float manaCost;
    public float cooldown;
    public float baseDamage;
    public float additionalPercent;
    public float range;
    public bool needTarget;
    public Sprite skillIcon;
    public bool onCooldown = false;

    public abstract void CastSkill(GameObject user, GameObject target);
    public abstract void CastSkill(GameObject user, Vector3 targetPoint);

    public IEnumerator CoolDown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }
}
