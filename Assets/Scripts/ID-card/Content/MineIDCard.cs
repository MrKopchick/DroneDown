using UnityEngine;

public class MineIDCard : IDCardBase
{
    private Mine mine;

    protected override void Awake()
    {
        base.Awake();
        mine = GetComponent<Mine>();
        if (mine == null)
        {
            Debug.LogError($"Mine component not found");
        }
    }

    public override string GetSpecificContent()
    {
        if (mine == null) return "No Mine data available";

        return $"Mining Speed: {mine.GetMiningSpeed()} resources/second\n" +
               $"Status: {(mine.IsActive ? "Active" : "Inactive")}";
    }
}