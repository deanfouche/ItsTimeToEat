using Assets.Scripts.Controllers;
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
            PlayerController firstPersonController;
            if (player.TryGetComponent<PlayerController>(out firstPersonController))
            {
                firstPersonController.throwForce += Intensity;
            }
        }

        public void DeactivateMutation(GameObject player)
        {
            PlayerController firstPersonController;
            if (player.TryGetComponent<PlayerController>(out firstPersonController))
            {
                firstPersonController.throwForce -= Intensity;
            }
        }
    }
}