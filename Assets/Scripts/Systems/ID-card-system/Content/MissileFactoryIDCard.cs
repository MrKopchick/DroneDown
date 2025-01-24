using UnityEngine;

public class MissileFactoryIDCard : IDCardBase
{
    private MissileFactory factory;

    protected override void Awake()
    {
        base.Awake();
        factory = GetComponent<MissileFactory>();
    }

    public override string GetSpecificContent()
    {
        if (factory == null) return "Factory data unavailable.";
        return $"Production Time: {factory.productionTime}s\nMissiles Per Batch: {factory.missilesPerBatch}";
    }
}