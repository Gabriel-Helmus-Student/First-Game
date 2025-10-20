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
        public void AttackBtn()
        {
            Attack(Enemy, 10);
        }
        //heal button function//
        public void HealBtn()
        {
            Heal(Player, 10);
        }

        //turn change function//
        private void ChangeTurn()
        {
           isPlayerTurn = !isPlayerTurn;

            if (!isPlayerTurn)
            {
                AttackButton.interactable = false;
                HealButton.interactable = false;
            }
            else
            {
                AttackButton.interactable = true;
                HealButton.interactable = true;
            }
        }


        private IEnumerator EnemyTurn()
        {
            yield return new WaitForSeconds(3);

            int random = 0;
            random = Random.Range(1, 3);

            if (random == 1)
            {
                Attack(Player, 12);
            }
            else
            {
                Heal(Enemy, 3);
            }
        }
    }

}


