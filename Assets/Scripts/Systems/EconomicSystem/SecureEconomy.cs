using UnityEngine;

namespace Game.Economy
{
    public sealed class SecureEconomy
    {
        private int balance;

        public int Balance => balance; 
        public int PlayerMoney => balance;

        public SecureEconomy(int initialBalance)
        {
            balance = Mathf.Max(initialBalance, 0);
        }

        public bool Spend(int amount)
        {
            if (amount <= 0 || balance < amount) return false;

            balance -= amount;
            return true;
        }

        public void Add(int amount)
        {
            if (amount > 0)
            {
                balance += amount;
            }
        }
    }
}