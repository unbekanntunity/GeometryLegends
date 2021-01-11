using UnityEngine;

public class Skill : ScriptableObject
{
    public DamageType damageType;

    public string skillName;
    public Skill manaCost;
    public float baseDamage;
    public float additionalPercent;
}
