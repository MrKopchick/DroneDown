using UnityEngine;
using Game.AA.Defenses;

public class GunAAStationIDCard : IDCardBase
{
    private GunAAStation gunAAStation;

    protected override void Awake()
    {
        base.Awake();
        gunAAStation = GetComponent<GunAAStation>();
    }

    public override string GetSpecificContent()
    {
        if (gunAAStation == null) return "No GunAAStation data available";

        return 
               $"Detection Radius: {gunAAStation.DetectionRadius}m\n" +
               $"Fire Rate: {gunAAStation.FireRate} shots/sec\n" +
               $"Fire Radius: {gunAAStation.FireRadius}m\n" +
               $"Current Ammo: {gunAAStation.CurrentAmmo} / {gunAAStation.MaxAmmo}\n";
    }
}