using Assets.Scripts.Controllers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class LowGravityMutator : MonoBehaviour, IMutator
    {
        public MutationType Mutation { get; set; } = MutationType.LowGravity;
        public float Intensity { get; set; } = 3f;
        public float TimeToExpire { get; set; }
        public float Duration { get; set; } = 10f;
        public bool IsActive { get; set; }

        public void ApplyMutation(GameObject player)
        {
            Physics.gravity = new Vector3(0, -Intensity, 0);
        }

        public void DeactivateMutation(GameObject player)
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }
}