using UnityEngine;

public class GetStats : MonoBehaviour
{
    public Hero hero;

    private ProgressBarCircle expBarCircle;

    private void Awake()
    {
        expBarCircle = GetComponentInChildren<ProgressBarCircle>();
    }

    private void Update()
    {
        expBarCircle.UpdateValue(hero.currentexp, hero.maxExp);
    }
}
