using UnityEngine;

public class CityStats : MonoBehaviour
{
    public string cityName;
    public int population;
    public float qualityOfLife;
    public int bunkers;
    public int hospitals;
    public float infrastructureIntegrity;

    public int maxBunkers = 5;
    public int maxHospitals = 3;
    
    private HouseManager houseManager;

    private void Awake()
    {
        if (string.IsNullOrEmpty(cityName))
        {
            cityName = $"City_{Random.Range(1, 1000)}";
        }

        if (population <= 0)
        {
            population = Random.Range(100, 1000);
        }

        if (qualityOfLife <= 0)
        {
            qualityOfLife = Random.Range(50f, 100f);
        }

        if (infrastructureIntegrity <= 0)
        {
            infrastructureIntegrity = Random.Range(50f, 100f);
        }

        houseManager = GetComponent<HouseManager>();
        if (houseManager == null)
        {
            Debug.LogWarning($"HouseManager not found on {cityName}.");
        }

        EconomyManager.Instance?.RegisterCity(this);
    }
    
    private void Start()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.RegisterCity(this);
        }
        else
        {
            Debug.LogError($"EconomyManager is not ready to register {cityName}!");
        }
    }


    private void OnDestroy()
    {
        EconomyManager.Instance?.UnregisterCity(this);
    }

    public void BuildBunker(int cost)
    {
        if (bunkers < maxBunkers && EconomyManager.Instance.SpendMoney(cost))
        {
            bunkers++;
            NotificationManager.Instance.ShowMessage($"{cityName}: Bunker built. Total: {bunkers}.", "green");
        }
        else
        {
            NotificationManager.Instance.ShowMessage($"{cityName}: Not enough funds to build a bunker.", "red");
        }
    }


    public void BuildHospital(int cost)
    {
        if (hospitals < maxHospitals && EconomyManager.Instance.SpendMoney(cost))
        {
            hospitals++;
            Debug.Log($"{cityName}: Hospital built. Total hospitals: {hospitals}");
        }
        else
        {
            Debug.Log($"{cityName}: Cannot build more hospitals.");
        }
    }

    public void RepairInfrastructure(int cost)
    {
        if (infrastructureIntegrity < 100 && EconomyManager.Instance.SpendMoney(cost))
        {
            infrastructureIntegrity = Mathf.Min(100, infrastructureIntegrity + 20);
            Debug.Log($"{cityName}: Infrastructure repaired to {infrastructureIntegrity}%.");
        }
    }

    public void TakeDamage(float damage)
    {
        infrastructureIntegrity = Mathf.Max(0, infrastructureIntegrity - damage);
        if (bunkers > 0)
        {
            population = Mathf.Max(0, population - Mathf.CeilToInt(population * 0.05f));
        }
        else
        {
            population = Mathf.Max(0, population - Mathf.CeilToInt(population * 0.1f));
        }

        qualityOfLife = Mathf.Max(0, qualityOfLife - damage * 0.1f);
        NotificationManager.Instance.ShowMessage($"{cityName} took damage!", "red");
    }

}
