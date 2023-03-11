namespace RLDataTypes
{

    public enum ElementType { Neutral, Fire, Water, Grass }

    // Don't change the order of the statuses in the enum.
    // Prefabs use the enum ordinals, which will be messed up if the order changes.
    public enum StatusType
    {
        None,
        All,
        Blindness,
        Burn,
        Confusion,
        Muteness,
        Petrify,
        Poison,
        Regeneration,
        Sleep,
        Slow,
        PetrifyImmunity,
        SleepImmunity,
        SeismicShock,
        Stun,
    }
}
