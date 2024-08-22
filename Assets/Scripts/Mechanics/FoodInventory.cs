using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Mechanics
{
    public class FoodInventory : MonoBehaviour
    {
        private GameObject[] _foodItems;
        public int foodCount;

        public GameObject counterDisplay;
        private Text _counterText;

        [SerializeField]
        Victory victory;

        // Start is called before the first frame update
        void Start()
        {
            _counterText = counterDisplay.GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            _foodItems = GameObject.FindGameObjectsWithTag("Food");
            if (foodCount != _foodItems.Length)
            {
                foodCount = _foodItems.Length;
                UpdateFoodCounter(foodCount);

                if (foodCount <= 0)
                {
                    victory.AllFoodConsumed();
                }
            }
        }

        void UpdateFoodCounter(int foodCount)
        {
            _counterText.text = $"{foodCount}";
        }
    }
}
