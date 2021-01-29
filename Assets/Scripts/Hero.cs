using System.Collections.Generic;
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
    public string heroName;
    public Sprite heroPic;

    public float maxHealth;
    public float currentHealth;
    public float maxMana;
    public float currentMana;
    public float physicalATK;
    public float armor;
    public float physicalPEN;
    public float physicalLifesteal;
    public float spellVamp;

    public float healthRegen;
    public float manaRegen;
    public float magicPower;
    public float magicRES;
    public float magicPEN;
    public float magicalLifesteal;

    public float movementSPD;
    public float cdReduction;
    public float attackSpeed;
    public float critChance;
    
    public float currentLvl;
    public float maxExp;
    public float currentexp;

    public List<GameObject> abilities = new List<GameObject>();
    public GameObject basicAttack;

}
