using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSkillIcons : MonoBehaviour
{
    public Hero hero;
    public List<IconController> icons = new List<IconController>();

    private void UpdateIcons()
    {
        for (int i = 0; i < hero.abilities.Count; i++)
        {
            icons[i].setIcon(hero.abilities[i].GetComponent<Skill>().skillIcon);
        }
    }

    public void SetHero(Hero _hero)
    {
        hero = _hero;
        UpdateIcons();
    }
}
