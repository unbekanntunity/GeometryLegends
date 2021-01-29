using UnityEngine;

[CreateAssetMenu(fileName = "new NonPassiveItem", menuName = "Item/NonPassiveItem")]
public class NonPassiveItems : Item
{
    public override void passive(GameObject user, GameObject target)
    {
    
    }
}
