namespace Assets.Scripts.Gameplay
{
    public interface IMutator
    {
        public Mutation Mutation { get; set; }

        public float Intensity { get; set; }

        public float ActivationTime { get; set; }

        public float Duration { get; set; }

        public bool IsActive { get; set; }
    }

    public enum Mutation
    {
        SpeedBoost,
        ThrowBoost,
        StrengthBoost,
        Armor
    }
}