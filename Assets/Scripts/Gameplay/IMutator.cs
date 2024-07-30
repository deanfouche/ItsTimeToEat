using System.ComponentModel;
using System.Reflection;

namespace Assets.Scripts.Gameplay
{
    public interface IMutator
    {
        public Mutation Mutation { get; set; }

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
    }

    public enum Mutation
    {
        [Description("Speed boost")]
        SpeedBoost,

        [Description("Throw boost")]
        ThrowBoost,

        [Description("Strength boost")]
        StrengthBoost,

        [Description("Speed boost")]
        Armor
    }
}