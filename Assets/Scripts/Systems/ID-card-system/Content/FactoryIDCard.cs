using UnityEngine;
using Game;
public class FactoryIDCard : IDCardBase
{
    private Factory factory;

    protected override void Awake()
    {
        base.Awake();
        factory = GetComponent<Factory>();

        if (factory == null)
        {
            Debug.LogError($"Factory null on {gameObject.name}");
        }
    }

    public override string GetSpecificContent()
    {
        if (factory == null) return "No Factory data available.";

        return $"Build Boost: {factory.buildBoost}";
    }
}