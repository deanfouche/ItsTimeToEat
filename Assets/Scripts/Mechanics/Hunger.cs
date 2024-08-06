using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class Hunger : MonoBehaviour
    {
        public GameOver gameOver;

        public float maxHungerLevel = 100f;
        public float hungerLevel = 0f;
        public float hungerIncrement = 5f;
        public float hungerRate = 2f;
        private float _nextHungerTick = 0f;
        [SerializeField]
        private HungerMeter _hungerMeter;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
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
                hungerLevel += hungerIncrement;
            }

            _hungerMeter.SetHunger(hungerLevel);

            if (hungerLevel >= maxHungerLevel)
            {
                gameOver.PlayerStarved();
            }
        }
    }
}