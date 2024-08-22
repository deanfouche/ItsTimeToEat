using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        public UnityEvent gameOver;

        public bool isGameOver;

        public void PlayerStarved()
        {
            TriggerGameOver("You starved!");
        }

        void TriggerGameOver(string gameOverMessage)
        {
            isGameOver = true;
            gameOver.Invoke();
        }
    }
}