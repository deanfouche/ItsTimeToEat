﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        MetaGameController metaGameController;
        public bool isGameOver;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayerStarved()
        {
            TriggerGameOver("You starved!");
        }

        void TriggerGameOver(string gameOverMessage)
        {
            isGameOver = true;
            metaGameController.ToggleGameOver(true);
        }
    }
}