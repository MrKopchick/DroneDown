using System.Collections.Generic;
using UnityEngine;

public abstract class EconomyBase : MonoBehaviour
{
    protected List<CityStats> cities = new List<CityStats>();

    [SerializeField] private float baseTaxRate = 10f;
    [SerializeField] private float highQualityOfLifeMultiplier = 2f;

    protected int lastTaxIncome;
    public int LastTaxIncome => lastTaxIncome;
    public float BaseTaxRate => baseTaxRate;
    public float HighQualityOfLifeMultiplier => highQualityOfLifeMultiplier;

    public int CalculateTotalPopulation()
    {
        int totalPopulation = 0;
        foreach (var city in cities)
        {
            if (city != null)
            {
                totalPopulation += city.population;
            }
        }
        return totalPopulation;
    }

    public float CalculateAverageQualityOfLife()
    {
        float totalQualityOfLife = 0f;
        int validCityCount = 0;

        foreach (var city in cities)
        {
            if (city != null && city.population > 0)
            {
                totalQualityOfLife += city.qualityOfLife;
                validCityCount++;
            }
        }
        return validCityCount > 0 ? totalQualityOfLife / validCityCount : 0f;
    }

    public int CalculateTotalBunkers()
    {
        int totalBunkers = 0;
        foreach (var city in cities)
        {
            if (city != null)
            {
                totalBunkers += city.bunkers;
            }
        }
        return totalBunkers;
    }

    public int CalculateTotalHospitals()
    {
        int totalHospitals = 0;
        foreach (var city in cities)
        {
            if (city != null)
            {
                totalHospitals += city.hospitals;
            }
        }
        return totalHospitals;
    }
}