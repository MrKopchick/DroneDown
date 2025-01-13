using UnityEngine;
using TMPro;

public class ObjectCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text pvoCountText;
    [SerializeField] private TMP_Text villageCountText;
    [SerializeField] private TMP_Text factoryCountText;
    [SerializeField] private string pvoType = "AAStation";
    [SerializeField] private string villageType = "Village";
    [SerializeField] private string factoryType = "Factory";

    private void Update()
    {
        pvoCountText.text = $"AAR: {ObjectRegistry.GetObjectCount(pvoType)}";
        villageCountText.text = $"Villages: {ObjectRegistry.GetObjectCount(villageType)}";
        factoryCountText.text = $"Factories: {ObjectRegistry.GetObjectCount(factoryType)}";
    }
}