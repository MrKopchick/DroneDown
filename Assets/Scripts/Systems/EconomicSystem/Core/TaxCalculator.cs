using System.Collections.Generic;
using UnityEngine;

namespace Game.Economy
{
    public class TaxCalculator
    {
        private readonly float baseTaxRate;
        private readonly float qualityMultiplier;

        public TaxCalculator(float baseTaxRate, float qualityMultiplier)
        {
            this.baseTaxRate = baseTaxRate;
            this.qualityMultiplier = qualityMultiplier;
        }

        public int CalculateTaxIncome(List<CityStats> cities)
        {
            int total = 0;
            foreach (var city in cities)
            {
                if (city == null) continue;

                float multiplier = city.qualityOfLife > 80 ? qualityMultiplier : 1f;
                total += Mathf.CeilToInt(city.population * baseTaxRate * multiplier / 100f);
            }
            return total;
        }
    }
}