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
        private Slider hungerSlider;
        private Image sliderImage;

        // Start is called before the first frame update
        void Start()
        {
            _playerHunger = GameObject.FindGameObjectWithTag("Player").GetComponent<Hunger>();
            hungerSlider = GetComponent<Slider>();
            hungerSlider.maxValue = _playerHunger.hungerStatus.MaxVal;
            hungerSlider.value = _playerHunger.hungerStatus.CurrentVal;
            sliderImage = GetComponentInChildren<Image>();
        }

        public void SetHunger(float hungerLevel)
        {
            hungerSlider.value = hungerLevel;

            if (_playerHunger.hungerStatus.StatusPercentage > 50f)
            {
                sliderImage.color = new Color(0, 255, 0);
            }
            else if (_playerHunger.hungerStatus.StatusPercentage <= 50f && _playerHunger.hungerStatus.StatusPercentage > 25f)
            {
                sliderImage.color = new Color(255, 255, 0);
            }
            else
            {
                sliderImage.color = new Color(255, 0, 0);
            }
        }
    }
}
