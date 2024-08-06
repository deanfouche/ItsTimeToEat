using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Victory : MonoBehaviour
    {
        [SerializeField]
        MetaGameController metaGameController;
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
            metaGameController.ToggleVictory(true);
        }
    }
}