using Assets.Scripts.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Mutations : MonoBehaviour
    {
        public GameObject uiMutationPrefab;
        public Transform parentContainer;

        private List<GameObject> activeMutations = new List<GameObject>();

        public void AddMutationIndicator(IMutator mutator)
        {
            GameObject newMutationIndicator = Instantiate(uiMutationPrefab, parentContainer);
            newMutationIndicator.name = mutator.Mutation.ToString();
            MutationIndicator indicator = newMutationIndicator.AddComponent<MutationIndicator>();
            indicator.timeToExpire = mutator.TimeToExpire;
            indicator.totalTime = mutator.Duration;
            activeMutations.Add(newMutationIndicator);
        }

        public void RemoveMutationIndicator(Mutation mutation)
        {
            if (activeMutations.Count > 0)
            {
                GameObject indicatorToRemove = activeMutations.Find(indicator => indicator.name == mutation.ToString());
                if (indicatorToRemove != null)
                {
                    activeMutations.Remove(indicatorToRemove);
                    Destroy(indicatorToRemove);
                }
            }
        }
    }
}