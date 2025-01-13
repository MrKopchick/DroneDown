using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Taxation Settings")]
    [SerializeField] private float baseTaxRate = 10f; 
    [SerializeField] private float highQualityOfLifeMultiplier = 2f;

    [Header("City Statistics")]
    private List<CityStats> cities = new List<CityStats>();

    private int lastTaxIncome;
    private float taxInterval = 60f;
    private float taxTimer;

    private SecureEconomy secureEconomy;

    public int LastTaxIncome { get; private set; }
    public float BaseTaxRate { get => baseTaxRate; private set => baseTaxRate = value; }
    public float HighQualityOfLifeMultiplier { get => highQualityOfLifeMultiplier; private set => highQualityOfLifeMultiplier = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            secureEconomy = new SecureEconomy(1000000);
            Debug.Log("EconomyManager initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        taxTimer += Time.deltaTime;
        if (taxTimer >= taxInterval)
        {
            CollectTaxes();
            taxTimer = 0f;
        }
    }

    public void RegisterCity(CityStats city)
    {
        if (city != null && !cities.Contains(city))
        {
            cities.Add(city);
            Debug.Log($"City registered: {city.cityName}");
        }
    }

    public void UnregisterCity(CityStats city)
    {
        if (cities.Contains(city))
        {
            cities.Remove(city);
            Debug.Log($"City unregistered: {city.cityName}");
        }
    }

    public void CollectTaxes()
    {
        LastTaxIncome = 0;
        foreach (var city in cities)
        {
            if (city == null || city.population <= 0) continue;

            float qualityMultiplier = city.qualityOfLife > 80 ? HighQualityOfLifeMultiplier : 1f;
            int cityTax = Mathf.CeilToInt(city.population * BaseTaxRate * qualityMultiplier / 100f);
            LastTaxIncome += cityTax;
        }

        secureEconomy.AddMoney(LastTaxIncome);

        NotificationManager.Instance.ShowMessage($"Collected taxes: {LastTaxIncome} coins.", "green");
        Debug.Log($"Total taxes collected: {LastTaxIncome}");
    }

    public bool SpendMoney(int amount)
    {
        return secureEconomy.SpendMoney(amount);
    }

    public int GetPlayerMoney()
    {
        return secureEconomy.PlayerMoney;
    }

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

    public void DebugCities()
    {
        Debug.Log("Cities registered in EconomyManager:");
        foreach (var city in cities)
        {
            if (city != null)
            {
                Debug.Log($"City: {city.cityName}, Population: {city.population}, QoL: {city.qualityOfLife}, Bunkers: {city.bunkers}, Hospitals: {city.hospitals}, Infrastructure: {city.infrastructureIntegrity}%");
            }
            else
            {
                Debug.LogWarning("Null city found in EconomyManager.");
            }
        }
    }
}
