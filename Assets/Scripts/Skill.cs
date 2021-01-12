using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : ScriptableObject
{
    public DamageType damageType;

    public string skillName;
    public float manaCost;
    public float cooldown;
    public float baseDamage;
    public float additionalPercent;

    public Sprite skillIcon; 

    public abstract void CastSkill(GameObject user, GameObject target);
}
