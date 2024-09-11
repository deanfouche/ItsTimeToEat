using Assets.Scripts.Core;
using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Mechanics
{
    public class Hunger : MonoBehaviour
    {
        public GameOver gameOver;
        public StatusMeter hungerStatus = new StatusMeter();

        float hungerIncrement = 5f;
        float hungerRate = 2f;
        float _nextHungerTick = 2f;
        float lastHungerVal = 0;
        [SerializeField]
        HungerMeter _hungerMeter;

        private void Start()
        {
            lastHungerVal = hungerStatus.CurrentVal;
            _nextHungerTick = Time.time + hungerRate;
        }

        void Update()
        {
            if (!gameOver.isGameOver)
            {
                CalculateHunger();
            }
        }

        public void CalculateHunger()
        {
            // Increase hunger at regular intervals
            if (Time.time > _nextHungerTick)
            {
                _nextHungerTick = Time.time + hungerRate;
                hungerStatus.DecreaseStatusMeter(hungerIncrement);
            }

            if (hungerStatus.CurrentVal != lastHungerVal)
            {
                lastHungerVal = hungerStatus.CurrentVal;
                _hungerMeter.SetHunger(hungerStatus.CurrentVal);
            }

            if (hungerStatus.CurrentVal <= hungerStatus.MinVal)
            {
                if (SceneManager.GetActiveScene().name != "TheLaboratory")
                {
                    gameOver.PlayerStarved();
                }
            }
        }
    }
}