using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LP.TurnBasedCombat
{
    public class GameController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private Slider PlayerHealthBar;
        [SerializeField] private Slider EnemyHealthBar;
        [SerializeField] private Button AttackButton;
        [SerializeField] private Button HealButton;

        private Unit playerUnit;
        private Unit enemyUnit;
        private bool isPlayerTurn = true;

        private void Start()
        {
            // Initialize both combatants
            playerUnit = new Unit("Player", 100, 15, 10);
            enemyUnit = new Unit("Enemy", 80, 12, 5);

            PlayerHealthBar.maxValue = playerUnit.MaxHp;
            EnemyHealthBar.maxValue = enemyUnit.MaxHp;
            PlayerHealthBar.value = playerUnit.CurrentHp;
            EnemyHealthBar.value = enemyUnit.CurrentHp;

            AttackButton.onClick.AddListener(AttackBtn);
            HealButton.onClick.AddListener(HealBtn);
        }

        public void AttackBtn()
        {
            if (!isPlayerTurn) return;

            int damage = playerUnit.Attack(enemyUnit);
            EnemyHealthBar.value = enemyUnit.CurrentHp;

            if (enemyUnit.CurrentHp <= 0)
            {
                Debug.Log("Enemy defeated!");
                return;
            }

            ChangeTurn();
            StartCoroutine(EnemyTurn());
        }

        public void HealBtn()
        {
            if (!isPlayerTurn) return;

            int healed = playerUnit.Heal();
            PlayerHealthBar.value = playerUnit.CurrentHp;

            ChangeTurn();
            StartCoroutine(EnemyTurn());
        }

        private void ChangeTurn()
        {
            isPlayerTurn = !isPlayerTurn;
            AttackButton.interactable = isPlayerTurn;
            HealButton.interactable = isPlayerTurn;
        }

        private IEnumerator EnemyTurn()
        {
            yield return new WaitForSeconds(2f);

            int randomChoice = Random.Range(1, 3);

            if (randomChoice == 1)
            {
                int damage = enemyUnit.Attack(playerUnit);
                PlayerHealthBar.value = playerUnit.CurrentHp;

                if (playerUnit.CurrentHp <= 0)
                {
                    Debug.Log("Player defeated!");
                    yield break;
                }
            }
            else
            {
                int healed = enemyUnit.Heal();
                EnemyHealthBar.value = enemyUnit.CurrentHp;
            }

            ChangeTurn();
        }
        private void CheckForDefeat()
        {
            if (playerUnit.CurrentHp <= 0)
                Debug.Log("Game Over!");

            if (enemyUnit.CurrentHp <= 0)
                Debug.Log("You Win!");
        }
    }
}
