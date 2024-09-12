using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public interface IMutator
    {
        public MutationType Mutation { get; set; }

        public float Intensity { get; set; }

        public float TimeToExpire { get; set; }

        public float Duration { get; set; }

        public bool IsActive { get; set; }

        public string GetDescription()
        {
            var field = this.Mutation.GetType().GetField(this.Mutation.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? this.Mutation.ToString();
        }

        public void ApplyMutation(GameObject player);
        public void DeactivateMutation(GameObject player);
    }

    public enum MutationType
    {
        [Description("Speed boost")]
        SpeedBoost,

        [Description("Yeet boost")]
        ThrowBoost,

        [Description("Strength boost")]
        StrengthBoost,

        [Description("Armor boost")]
        Armor,

        [Description("Low Gravity")]
        LowGravity,

        [Description("Double Jump")]
        DoubleJump
    }
}