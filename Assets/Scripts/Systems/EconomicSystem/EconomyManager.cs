using UnityEngine;

namespace Game.Economy
{
    public sealed class EconomyManager : MonoBehaviour, IEconomyManager
    {
        public static EconomyManager Instance { get; private set; }

        [SerializeField] private float taxInterval = 60f;
        [SerializeField] private float baseTaxRate = 10f;
        [SerializeField] private float qualityMultiplier = 2f;

        private readonly SecureEconomy economy = new(1000000);
        private CityManager cityManager;
        private TaxCalculator taxCalculator;
        private float taxTimer;

        // Add this property to store the last tax income
        public int LastTaxIncome { get; private set; }

        private void Awake() => InitializeSingleton();

        private void InitializeSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                cityManager = new CityManager();
                taxCalculator = new TaxCalculator(baseTaxRate, qualityMultiplier);
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

        public bool Spend(int amount) => economy.Spend(amount);
        
        public void AddFunds(int amount) => economy.Add(amount);

        public int GetPlayerMoney() => economy.PlayerMoney;

        private void CollectTaxes()
        {
            int total = taxCalculator.CalculateTaxIncome(cityManager.GetCities());
            economy.Add(total);
            LastTaxIncome = total;
            NotificationManager.Instance.ShowMessage(
                $"Collected taxes: {total} coins.", 
                "green"
            );
        }

        public void RegisterCity(CityStats city)
        {
            cityManager.RegisterCity(city);
        }

        public void UnregisterCity(CityStats city)
        {
            cityManager.UnregisterCity(city);
        }

        public int CalculateTotalPopulation() => cityManager.CalculateTotalPopulation();

        public float CalculateAverageQualityOfLife() => cityManager.CalculateAverageQualityOfLife();

        public int CalculateTotalBunkers() => cityManager.CalculateTotalBunkers();

        public int CalculateTotalHospitals() => cityManager.CalculateTotalHospitals();
    }
}