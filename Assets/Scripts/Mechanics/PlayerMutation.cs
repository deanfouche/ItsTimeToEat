using Assets.Scripts.Controllers;
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

        private PlayerController _playerController;
        private PlayerInteraction _playerInteraction;

        // Use this for initialization
        void Start()
        {
            _playerController = player.GetComponent<PlayerController>();
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
                mutator.ApplyMutation(player);
                mutator.TimeToExpire = Time.deltaTime + mutator.Duration;
                mutator.IsActive = true;
                mutators.Add(mutator);

                // add a UI indicator of the lifespan of the mutation
                Mutations mutationsUI = mutationDisplay.GetComponent<Mutations>();
                if (mutationsUI != null)
                {
                    mutationsUI.AddMutationIndicator(mutator);
                }
            } else
            {
                IMutator originalMutator = mutators.Find(m => m.Mutation == mutator.Mutation);
                originalMutator.TimeToExpire = Time.deltaTime + originalMutator.Duration;
                // add a UI indicator of the lifespan of the mutation
                Mutations mutationsUI = mutationDisplay.GetComponent<Mutations>();
                if (mutationsUI != null)
                {
                    mutationsUI.ExtendMutationIndicator(originalMutator);
                }
            }
        }

        void DeactivateMutation(IMutator mutator)
        {
            if (mutators.Exists(m => m.Mutation == mutator.Mutation))
            {
                mutator.DeactivateMutation(player);
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