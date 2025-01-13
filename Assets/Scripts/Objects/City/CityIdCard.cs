using UnityEngine;

public class CityIdCard : MonoBehaviour
{
    [SerializeField] private CityStats cityStats;

    private void Awake()
    {
        if (cityStats == null)
        {
            cityStats = GetComponent<CityStats>();
            if (cityStats == null)
            {
                Debug.LogError($"CityStats component not found on {gameObject.name}");
            }
        }
    }

    public string GetCityCardContent()
    {
        if (cityStats == null)
        {
            return "City stats unavailable.";
        }

        return $"City Name: {cityStats.cityName}\n" +
               $"Population: {cityStats.population}\n" +
               $"Quality of Life: {cityStats.qualityOfLife}%\n" +
               $"Bunkers: {cityStats.bunkers}/{cityStats.maxBunkers}\n" +
               $"Hospitals: {cityStats.hospitals}/{cityStats.maxHospitals}\n" +
               $"Infrastructure: {cityStats.infrastructureIntegrity}%";
    }

    public void DisplayCityCard()
    {
        if (IDCardManager.Instance != null)
        {
            IDCardManager.Instance.ShowCityCard(this);
        }
        else
        {
            Debug.LogError("IDCardManager instance not found");
        }
    }
}