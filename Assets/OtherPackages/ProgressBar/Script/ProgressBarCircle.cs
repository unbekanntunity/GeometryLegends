using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class ProgressBarCircle : MonoBehaviour
{
    [Header("Title Setting")]
    public string Title;
    public Color TitleColor;
    public Font TitleFont;

    [Header("Bar Setting")]
    public Color BarBackGroundColor;
    public Color BarColor;
    public Color MaskColor;
    public Sprite BarBackGroundSprite;

    [SerializeField]
    private Image bar, barBackground, Mask;
    [SerializeField]
    private Text txtTitle;
    private float barValue;

    public float BarValue
    {
        get { return barValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValue = value;
            UpdateValue(barValue, 100);

        }
    }

    private void Start()
    {
        txtTitle.text = Title;
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;


        bar.color = BarColor;
        Mask.color = MaskColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(barValue, 100);
    }

    public void UpdateValue(float val, float maxval)
    {
        bar.fillAmount = val / maxval;
        txtTitle.text = Title + " " + (val / maxval) * 100+ "%";
    }
}
