using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    public Image selected;
    public Image skillIcon;

    public void setIcon(Sprite sprite)
    {
        skillIcon.sprite = sprite;
    }

    public void SelectIcon()
    {
        selected.enabled = selected.enabled ? false : true;
    }
}
