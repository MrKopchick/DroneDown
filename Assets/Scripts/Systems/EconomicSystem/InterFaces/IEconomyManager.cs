namespace Game.Economy
{
    public interface IEconomyManager
    {
        bool Spend(int amount);
        void AddFunds(int amount);
        int GetPlayerMoney();
        void RegisterCity(CityStats city);
        void UnregisterCity(CityStats city);
        int CalculateTotalPopulation();
        float CalculateAverageQualityOfLife();
        int CalculateTotalBunkers();
        int CalculateTotalHospitals();
    }
}