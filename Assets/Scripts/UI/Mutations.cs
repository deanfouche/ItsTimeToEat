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
            MutationIndicator indicator = newMutationIndicator.GetComponent<MutationIndicator>();
            indicator.AddMutationUIElement(mutator.Mutation.ToString(), mutator.TimeToExpire, mutator.Duration);
            activeMutations.Add(newMutationIndicator);
        }

        public void RemoveMutationIndicator(Mutation mutation)
        {
            if (activeMutations.Count > 0)
            {
                var mutationType = mutation.GetType();

                GameObject indicatorToRemove = activeMutations.Find(indicator => indicator.name == mutation.ToString());
                Debug.Log($"Mutation UI element: {indicatorToRemove}");
                if (indicatorToRemove != null)
                {
                    activeMutations.Remove(indicatorToRemove);
                    indicatorToRemove.GetComponent<MutationIndicator>().RemoveMutationUIElement();
                }
            }
        }
    }
}