using Assets.Scripts.Mechanics;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class HandAnimator : MonoBehaviour
    {
        public GameObject player;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GrabObjectInHand()
        {
            PlayerInteraction playerInteraction = player.GetComponentInChildren<PlayerInteraction>();

            playerInteraction.GrabObject();
        }

        public void ConsumeObjectInHand()
        {
            PlayerInteraction playerInteraction = player.GetComponentInChildren<PlayerInteraction>();

            playerInteraction.ConsumeFood();
        }

        public void ReleaseTrownObjectInHand()
        {
            PlayerInteraction playerInteraction = player.GetComponentInChildren<PlayerInteraction>();

            playerInteraction.ReleaseThrownObject();
        }
    }
}