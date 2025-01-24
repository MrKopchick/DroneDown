using UnityEngine;

public class AAStationIDCard : IDCardBase
{
    private AAStation aaStation;

    protected override void Awake()
    {
        base.Awake();
        aaStation = GetComponent<AAStation>();
        if (aaStation == null)
        {
            Debug.LogError($"AAStation null on {gameObject.name}");
        }
    }

    public override string GetSpecificContent()
    {
        if (aaStation == null) return "No AAStation data available";

        return $"Detection Radius: {aaStation.GetDetectionRadius()}\n" +
               $"Missiles: {aaStation.GetCurrentMissiles()}/{aaStation.GetMaxMissiles()}\n" +
               $"Reload Time: {aaStation.GetReloadTime()}s";
    }
}