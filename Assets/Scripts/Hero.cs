using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public float maxMana;
    public float currentMana;
    public float currentLvl;
    public float maxExp;
    public float currentexp;
}
