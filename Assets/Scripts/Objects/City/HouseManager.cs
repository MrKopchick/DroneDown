using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> houses;

    private void Awake()
    {
        InitializeHouses();
    }

    private void InitializeHouses()
    {
        houses.RemoveAll(house => house == null);
    }

    public Vector3 GetRandomHousePosition()
    {
        if (houses.Count == 0)
        {
            Debug.LogWarning($"No houses available in {gameObject.name}.");
            return Vector3.zero;
        }

        GameObject randomHouse = houses[Random.Range(0, houses.Count)];
        return randomHouse.transform.position;
    }

    public bool DamageHouse(Vector3 position)
    {
        foreach (var house in houses)
        {
            if (house != null && house.transform.position == position)
            {
                MarkHouseAsDamaged(house);
                return true;
            }
        }
        return false;
    }

    private void MarkHouseAsDamaged(GameObject house)
    {
        Debug.Log($"House {house.name} is destroyed.");
        houses.Remove(house);

        // Update city stats if available
        CityStats cityStats = GetComponent<CityStats>();
        if (cityStats != null)
        {
            cityStats.TakeDamage(10);
        }

        Destroy(house);
    }

    public bool IsEmpty()
    {
        return houses.Count == 0;
    }
}