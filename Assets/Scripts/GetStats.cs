using UnityEngine;

public class GetStats : MonoBehaviour
{
    public Hero hero;

    public ProgressBarCircle expBarCircle;
    public ProgressBar healthBar;
    public ProgressBar manaBar;

    private void Update()
    {
        expBarCircle.UpdateValue(hero.currentexp, hero.maxExp);
        healthBar.UpdateValue(hero.currentHealth, hero.maxHealth);
        manaBar.UpdateValue(hero.currentMana, hero.maxMana);
    }
}
