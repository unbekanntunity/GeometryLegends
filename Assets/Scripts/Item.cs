using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public int goldprice;
    public Sprite image;

    public int magicPower;
    public int mana;
    public int manaRegeneration;
    public int magicalPenetration;
    public int magicalLifesteal;

    public int physicalAttack;
    public int attackSpeed;
    public int physicalPenetration;
    public int lifesteal;
    public int spellVamp;
    public int criticalChance;
    public int cooldownReduction;

    public int hp;
    public int hpRegeneration;
    public int armor;
    public int magicResistance;

    public int movementSpeed;

    public void euqip(GetStats user)
    {
        user.hero.magicPower += magicPower;
        user.hero.maxMana += mana;
        user.hero.manaRegen += manaRegeneration;
        user.hero.magicPEN += magicalPenetration;
        user.hero.magicalLifesteal += magicalLifesteal;
        user.hero.physicalATK += physicalAttack;
        user.hero.attackSpeed += attackSpeed;
        user.hero.physicalPEN += physicalPenetration;
        user.hero.physicalLifesteal += lifesteal;
        user.hero.spellVamp += spellVamp;
        user.hero.critChance += criticalChance;
        user.hero.cdReduction += cooldownReduction;
        user.hero.maxHealth += hp;
        user.hero.healthRegen += hpRegeneration;
        user.hero.armor += armor;
        user.hero.magicRES += magicResistance;
    }

    public void uneuqip(GetStats user)
    {
        user.hero.magicPower -= magicPower;
        user.hero.maxMana -= mana;
        user.hero.manaRegen -= manaRegeneration;
        user.hero.magicPEN -= magicalPenetration;
        user.hero.magicalLifesteal -= magicalLifesteal;
        user.hero.physicalATK -= physicalAttack;
        user.hero.attackSpeed -= attackSpeed;
        user.hero.physicalPEN -= physicalPenetration;
        user.hero.physicalLifesteal -= lifesteal;
        user.hero.spellVamp -= spellVamp;
        user.hero.critChance -= criticalChance;
        user.hero.cdReduction -= cooldownReduction;
        user.hero.maxHealth -= hp;
        user.hero.healthRegen -= hpRegeneration;
        user.hero.armor -= armor;
        user.hero.magicRES -= magicResistance;
    }

    public abstract void passive(GameObject user, GameObject target);
}
