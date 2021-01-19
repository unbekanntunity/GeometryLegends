using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    AoE, SingleTarget
}

public abstract class Skill : ScriptableObject
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

    public abstract void CastSkill(GameObject user, GameObject target);
    public abstract void CastSkill(GameObject user, Vector3 targetPoint);
    public abstract void CastSkill(GameObject user);

}
