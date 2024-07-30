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

        // Use this for initialization
        void Start()
        {
            _playerController = player.GetComponent<FirstPersonController>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var mutator in mutators)
            {
                if (Time.time > mutator.TimeToExpire)
                {
                    deactivateMutation(mutator);
                }
            }

            mutators.RemoveAll(m => !m.IsActive);
        }

        public void applyMutation(IMutator mutator)
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
                        break;
                    case Mutation.StrengthBoost:
                        break;
                    case Mutation.Armor:
                        break;
                    default:
                        break;
                }
                mutator.TimeToExpire = Time.time + mutator.Duration;
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

        void deactivateMutation(IMutator mutator)
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