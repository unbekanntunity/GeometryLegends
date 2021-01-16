using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMiniMapIcon : MonoBehaviour
{
    private GetStats getStats;
    private Image miniMapIcon;
    
    private void Awake()
    {
        miniMapIcon = GetComponent<Image>();
        getStats = GetComponentInParent<GetStats>();

        if(getStats.hero.heroPic != null)
            miniMapIcon.sprite = getStats.hero.heroPic;
    }
}
