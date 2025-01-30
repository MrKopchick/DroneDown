using UnityEngine;
using Game.AA.Defenses;

public class AAStationIDCard : IDCardBase
{
    private MissileAAStation aaStation;

    protected override void Awake()
    {
        base.Awake();
        aaStation = GetComponent<MissileAAStation>();
    }

    public override string GetSpecificContent()
    {
        if (aaStation == null) return "No AAStation data available";

        return $"Detection Radius: {aaStation.DetectionRadius}\n" +
               $"Missiles: {aaStation.CurrentMissiles}/{aaStation.MaxMissiles}\n" +
               $"Reload Time: {aaStation.MissileReloadTime}s";
    }
}