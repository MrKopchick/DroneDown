using UnityEngine;

public class SecureEconomy
{
    private int playerMoney = 100000;

    public int PlayerMoney => playerMoney;

    public SecureEconomy(int initialMoney)
    {
        playerMoney = initialMoney;
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0 || playerMoney < amount)
        {
            Debug.LogWarning("Insufficient funds or invalid amount.");
            return false;
        }
        playerMoney -= amount;
        Debug.Log($"Spent: {amount}, Remaining: {playerMoney}");
        return true;
    }

    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            playerMoney += amount;
            Debug.Log($"Added: {amount}, New Balance: {playerMoney}");
        }
    }
}