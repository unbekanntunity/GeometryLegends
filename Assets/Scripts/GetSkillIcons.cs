using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSkillIcons : MonoBehaviour
{
    public Hero hero;
    public List<Image> icons = new List<Image>();

    private void UpdateIcons()
    {
        for (int i = 0; i < hero.abilities.Count; i++)
        {
            icons[i].GetComponentInChildren<Image>().sprite = hero.abilities[i].skillIcon;
        }
    }

    public void SetHero(Hero _hero)
    {
        hero = _hero;
        UpdateIcons();
    }
}
