using UnityEngine;

public class MissileStorageIDCard : IDCardBase
{
    private MissileStorage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<MissileStorage>();
    }

    public override string GetSpecificContent()
    {
        if (storage == null) return "Storage data unavailable.";
        return $"Current Missiles: {storage.GetCurrentMissiles()}\nMax Missiles: {storage.GetMaxMissiles()}";
    }
}