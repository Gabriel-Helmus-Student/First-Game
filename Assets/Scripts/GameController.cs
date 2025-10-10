using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

//Combat system//
namespace LP.TurnBaesedCombat
{
//define variables//
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject Player = null;
        [SerializeField] private GameObject Enemy = null;
        [SerializeField] private Slider PlayerHealthBar = null;
        [SerializeField] private Slider EnemyHealthBar = null;
        [SerializeField] private Button AttackButton = null;
        [SerializeField] private Button HealButton = null;

        private bool isPlayerTurn = true;

        //attack function//
        private void Attack(GameObject target, float damage)
        {
            if (target == Enemy)
            {
                EnemyHealthBar.value -= damage;
            }
            else
            {
                PlayerHealthBar.value -= damage;
            }

            ChangeTurn();
        }
        //heal function//
        private void Heal(GameObject target, float amount)
        {
            if (target == Enemy)
            {
                EnemyHealthBar.value += amount;
            }
            else
            {
                PlayerHealthBar.value += amount;
            }

            ChangeTurn();
        }

        //attack button function//
        public void AttackButton()
        {
            Attack(Enemy, 10);
        }
        //heal button function//
        public void HealButton()
        {
            Heal(Player, 10);
        }

        //turn change function//
        private void ChangeTurn()
        {
           isPlayerTurn = !isPlayerTurn;
        }
    }

}


