using System;
using UnityEngine;

namespace LP.TurnBasedCombat
{
    [System.Serializable]
    public class Unit
    {
        [SerializeField] private string unitName;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private int attackPower;
        [SerializeField] private int healPower;

        private System.Random random;

        public string UnitName => unitName;
        public int MaxHp => maxHp;
        public int CurrentHp => currentHp;

        public Unit(string name, int maxHp, int attackPower, int healPower)
        {
            this.unitName = name;
            this.maxHp = maxHp;
            this.currentHp = maxHp;
            this.attackPower = attackPower;
            this.healPower = healPower;
            this.random = new System.Random();
        }

        public int Attack(Unit unitToAttack)
        {
            double rng = random.NextDouble();
            rng = rng / 2 + 0.75; // between 0.75–1.25 multiplier
            int randomDamage = (int)(attackPower * rng);

            unitToAttack.TakeDamage(randomDamage);
            Debug.Log($"{unitName} attacks {unitToAttack.unitName} for {randomDamage} damage!");
            return randomDamage;
        }

        public void TakeDamage(int damage)
        {
            currentHp -= damage;
            if (currentHp < 0) currentHp = 0;
        }

        public int Heal()
        {
            int healAmount = healPower;
            currentHp += healAmount;
            if (currentHp > maxHp) currentHp = maxHp;
            Debug.Log($"{unitName} heals for {healAmount} HP!");
            return healAmount;
        }
    }
}
