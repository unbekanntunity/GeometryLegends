using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private Movement movement;

    private bool found = false;
    private bool inRangeOfShop = false;

    private void Awake()
    {
        movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        found = false;

        foreach (GameObject item in movement.ColliderInRange)
        {
            if (item.name.Contains("Shop") || item.name.Contains("shop"))
            {
                found = true;
                inRangeOfShop = true;
                break;
            }
        }
        if (!found)
            inRangeOfShop = false;
    }

    public bool ControlInterface()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return false;
        }
        else
        {
            gameObject.SetActive(true);
            return true;
        }
    }
}
