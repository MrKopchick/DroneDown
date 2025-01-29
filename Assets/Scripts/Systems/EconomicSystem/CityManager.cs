using System;
using System.Collections.Generic;

namespace Game.Economy
{
    public class CityManager
    {
        private readonly List<CityStats> cities = new();

        public void RegisterCity(CityStats city)
        {
            if (!cities.Contains(city)) cities.Add(city);
        }

        public void UnregisterCity(CityStats city)
        {
            cities.Remove(city);
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

        public List<CityStats> GetCities()
        {
            return cities;
        }
    }
}