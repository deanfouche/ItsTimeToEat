using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public class Victory : MonoBehaviour
    {
        public UnityEvent victory;
        public bool isVictory;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AllFoodConsumed()
        {
            _TriggerVictory("You ate all the food!");
        }

        void _TriggerVictory(string victoryMessage)
        {
            isVictory = true;
            victory.Invoke();
        }
    }
}