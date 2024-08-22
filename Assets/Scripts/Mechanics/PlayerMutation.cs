using Assets.Scripts.Gameplay;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class PlayerMutation : MonoBehaviour
    {
        public GameObject player;
        public List<IMutator> mutators = new List<IMutator>();
        public GameObject mutationDisplay;

        private FirstPersonController _playerController;
        private PlayerInteraction _playerInteraction;

        // Use this for initialization
        void Start()
        {
            _playerController = player.GetComponent<FirstPersonController>();
            _playerInteraction = player.GetComponentInChildren<PlayerInteraction>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var mutator in mutators)
            {
                mutator.TimeToExpire -= Time.deltaTime;

                if (mutator.TimeToExpire <= 0)
                {
                    // End mutation
                    DeactivateMutation(mutator);
                }
            }

            mutators.RemoveAll(m => !m.IsActive);
        }

        public void ApplyMutation(IMutator mutator)
        {
            if (!mutators.Exists(m => m.Mutation == mutator.Mutation))
            {
                switch (mutator.Mutation)
                {
                    case Mutation.SpeedBoost:
                        _playerController.walkSpeed += mutator.Intensity;
                        _playerController.sprintSpeed += mutator.Intensity;
                        break;
                    case Mutation.ThrowBoost:
                        _playerInteraction.throwForce += mutator.Intensity;
                        break;
                    case Mutation.StrengthBoost:
                        break;
                    case Mutation.Armor:
                        break;
                    default:
                        break;
                }
                mutator.TimeToExpire = Time.deltaTime + mutator.Duration;
                mutator.IsActive = true;
                mutators.Add(mutator);

                // add a UI indicator of the lifespan of the mutation
                Mutations mutationsUI = mutationDisplay.GetComponent<Mutations>();
                if (mutationsUI != null)
                {
                    mutationsUI.AddMutationIndicator(mutator);
                }
            }
        }

        void DeactivateMutation(IMutator mutator)
        {
            if (mutators.Exists(m => m.Mutation == mutator.Mutation))
            {
                switch (mutator.Mutation)
                {
                    case Mutation.SpeedBoost:
                        var playerController = player.GetComponent<FirstPersonController>();
                        playerController.walkSpeed -= mutator.Intensity;
                        playerController.sprintSpeed -= mutator.Intensity;
                        break;
                    case Mutation.ThrowBoost:
                        _playerInteraction.throwForce -= mutator.Intensity;
                        break;
                    case Mutation.StrengthBoost:
                        break;
                    case Mutation.Armor:
                        break;
                    default:
                        break;
                }
                mutator.IsActive = false;// add a UI indicator of the lifespan of the mutation
                Mutations mutationsUI = mutationDisplay.GetComponent<Mutations>();
                if (mutationsUI != null)
                {
                    mutationsUI.RemoveMutationIndicator(mutator.Mutation);
                }
            }
        }
    }
}