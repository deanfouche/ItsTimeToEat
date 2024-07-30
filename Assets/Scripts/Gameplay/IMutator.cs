namespace Assets.Scripts.Gameplay
{
    public interface IMutator
    {
        public Mutation Mutation { get; set; }

        public float Intensity { get; set; }

        public float TimeToExpire { get; set; }

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