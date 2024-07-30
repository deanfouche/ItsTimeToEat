using Assets.Scripts.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HungerMeter : MonoBehaviour
    {
        private Hunger _playerHunger;
        public Slider hungerMeter;

        // Start is called before the first frame update
        void Start()
        {
            _playerHunger = GameObject.FindGameObjectWithTag("Player").GetComponent<Hunger>();
            hungerMeter = GetComponent<Slider>();
            hungerMeter.maxValue = _playerHunger.maxHungerLevel;
            hungerMeter.value = _playerHunger.hungerLevel;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetHunger(float hungerLevel)
        {
            hungerMeter.value = hungerLevel;
        }
    }
}
