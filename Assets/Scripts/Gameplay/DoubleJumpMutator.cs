using Assets.Scripts.Controllers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class DoubleJumpMutator : MonoBehaviour, IMutator
    {
        public MutationType Mutation { get; set; } = MutationType.DoubleJump;
        public float Intensity { get; set; } = 3f;
        public float TimeToExpire { get; set; }
        public float Duration { get; set; } = 10f;
        public bool IsActive { get; set; }

        public void ApplyMutation(GameObject player)
        {
            PlayerController playerController;
            if (player.TryGetComponent(out playerController))
            {
                playerController.enableDoubleJump = true;
            }
        }

        public void DeactivateMutation(GameObject player)
        {
            PlayerController playerController;
            if (player.TryGetComponent(out playerController))
            {
                playerController.enableDoubleJump = false;
            }
        }
    }
}