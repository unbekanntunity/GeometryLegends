using UnityEngine;

public enum HeroType
{
    Tank, Fighter, Assassin, ADC, Support
}

public enum DamageType
{
    Magical, Pysical, True
}

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public float maxMana;
    public float currentMana;
    public float physicalATK;
    public float armor;
    public float physicalPEN;
    public float Lifesteal;

    public float healthRegen;
    public float manaRegen;
    public float magicPower;
    public float magicRES;
    public float magicPEN;
    public float spellVamp;

    public float movementSPD;
    public float cdReduction;
    public float attackSpeed;
    public float critChance;
    
    public float currentLvl;
    public float maxExp;
    public float currentexp;
}
