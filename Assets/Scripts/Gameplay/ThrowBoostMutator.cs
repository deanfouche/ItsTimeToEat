using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class ThrowBoostMutator : MonoBehaviour, IMutator
    {
        public Mutation Mutation { get; set; } = Mutation.ThrowBoost;
        public float Intensity { get; set; } = 300f;
        public float TimeToExpire { get; set; }
        public float Duration { get; set; } = 20f;
        public bool IsActive { get; set; }

        public void ApplyMutation(GameObject player)
        {
            FirstPersonController firstPersonController;
            if (player.TryGetComponent<FirstPersonController>(out firstPersonController))
            {
                firstPersonController.throwForce += Intensity;
            }
        }

        public void DeactivateMutation(GameObject player)
        {
            FirstPersonController firstPersonController;
            if (player.TryGetComponent<FirstPersonController>(out firstPersonController))
            {
                firstPersonController.throwForce -= Intensity;
            }
        }
    }
}