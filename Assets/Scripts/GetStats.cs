using UnityEngine;

public class GetStats : MonoBehaviour
{
    public Hero hero;

    public ProgressBarCircle expBarCircle;
    public ProgressBar healthBar;
    public ProgressBar manaBar;

    public Skill selectedSkill;
    public GetSkillIcons getSkillIcons { get; private set; }

    [SerializeField]
    private bool HaveSkillIcons = false;

    private void Awake()
    {
        if (HaveSkillIcons)
        {
            getSkillIcons = GetComponentInChildren<GetSkillIcons>();
            getSkillIcons.SetHero(hero);
        }
    }

    private void Update()
    {
        expBarCircle.UpdateValue(hero.currentexp, hero.maxExp);
        healthBar.UpdateValue(hero.currentHealth, hero.maxHealth);
        manaBar.UpdateValue(hero.currentMana, hero.maxMana);
    }
}
