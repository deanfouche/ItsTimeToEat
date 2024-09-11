using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class SpeedMutator : MonoBehaviour, IMutator
    {
        public Mutation Mutation { get; set; } = Mutation.SpeedBoost;
        public float Intensity { get; set; } = 3f;
        public float TimeToExpire { get; set; }
        public float Duration { get; set; } = 10f;
        public bool IsActive { get; set; }

        public void ApplyMutation(GameObject player)
        {
            FirstPersonController firstPersonController;
            if (player.TryGetComponent<FirstPersonController>(out firstPersonController))
            {
                firstPersonController.walkSpeed += Intensity;
                firstPersonController.sprintSpeed += Intensity;
            }
        }

        public void DeactivateMutation(GameObject player)
        {
            FirstPersonController firstPersonController;
            if (player.TryGetComponent<FirstPersonController>(out firstPersonController))
            {
                firstPersonController.walkSpeed -= Intensity;
                firstPersonController.sprintSpeed -= Intensity;
            }
        }
    }
}