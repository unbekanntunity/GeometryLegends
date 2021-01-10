using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
    public float maxhealth;
    public float currenthealh;
    public float maxmana;
    public float currentmana;
}
