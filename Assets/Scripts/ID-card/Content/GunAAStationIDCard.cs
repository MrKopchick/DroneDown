using UnityEngine;

public class GunAAStationIDCard : IDCardBase
{
    private GunAAStation gunAAStation;

    protected override void Awake()
    {
        base.Awake();
        gunAAStation = GetComponent<GunAAStation>();
        if (gunAAStation == null)
        {
            Debug.LogError($"GunAAStation not found on {gameObject.name}");
        }
    }

    public override string GetSpecificContent()
    {
        if (gunAAStation == null) return "No GunAAStation data available";

        return $"Gun AA Station\n" +
               $"--------------------------\n" +
               $"Detection Radius: {gunAAStation.GetDetectionRadius()}m\n" +
               $"Fire Rate: {gunAAStation.GetFireRate()} shots/sec\n" +
               $"Bullet Speed: {gunAAStation.GetBulletSpeed()} m/s\n" +
               $"Fire Radius: {gunAAStation.GetFireRadius()}m\n" +
               $"Max Ammo: {gunAAStation.GetMaxAmmo()}\n" +
               $"Current Ammo: {gunAAStation.GetCurrentAmmo()}";
    }
}