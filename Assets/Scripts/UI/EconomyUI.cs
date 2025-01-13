// EconomyUI.cs
using UnityEngine;
using TMPro;
namespace Economy{
    public class EconomyUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text totalMoneyText;
        [SerializeField] private TMP_Text totalTaxIncomeText;
        [SerializeField] private TMP_Text totalPopulationText;
        [SerializeField] private TMP_Text averageQualityOfLifeText;
        [SerializeField] private TMP_Text totalBunkersText;
        [SerializeField] private TMP_Text totalHospitalsText;

        [Header("QoL Colors")]
        [SerializeField] private Color goodQoLColor = Color.green;
        [SerializeField] private Color averageQoLColor = Color.yellow;
        [SerializeField] private Color lowQoLColor = Color.red;

        private EconomyManager economyManager;

        private void Start()
        {
            economyManager = EconomyManager.Instance;
            if (economyManager == null)
            {
                Debug.LogError("EconomyManager instance not found!");
                enabled = false;
            }
        }
        private void Update()
        {
            if (economyManager == null) return;

            UpdateUI();
        }
        private void UpdateUI()
        {
            if (totalMoneyText != null)
                totalMoneyText.text = $"{economyManager.GetPlayerMoney()}";

            if (totalTaxIncomeText != null)
                totalTaxIncomeText.text = $"Last Tax Income: {economyManager.LastTaxIncome}";

            if (totalPopulationText != null)
                totalPopulationText.text = $"Total Population: {economyManager.CalculateTotalPopulation()}";

            if (averageQualityOfLifeText != null)
            {
                float avgQoL = economyManager.CalculateAverageQualityOfLife();
                averageQualityOfLifeText.text = $"Average QoL: {avgQoL:F1}%";
                averageQualityOfLifeText.color = GetQoLColor(avgQoL);
            }

            if (totalBunkersText != null)
                totalBunkersText.text = $"Total Bunkers: {economyManager.CalculateTotalBunkers()}";

            if (totalHospitalsText != null)
                totalHospitalsText.text = $"Total Hospitals: {economyManager.CalculateTotalHospitals()}";
        }
        private Color GetQoLColor(float qualityOfLife)
        {
            if (qualityOfLife > 80) return goodQoLColor;
            if (qualityOfLife > 50) return averageQoLColor;
            return lowQoLColor;
        }
    }
}
