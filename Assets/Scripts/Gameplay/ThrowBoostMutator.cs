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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}