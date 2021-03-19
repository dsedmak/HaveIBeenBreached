namespace GenePlanet.HaveIBeenBreached.BreachedEmails.ProbabilisticEmailAddressCollectionAdapter
{
    public class ProbabilisticEmailAddressCollectionOptions
    {
        public int MemoryAvailableInBytes { get; set; } = 1 << 26;

        public double ProbabilisticThreshold { get; set; } = 0.001;

        public CollectionBehaviour CollectionBehaviour { get; set; } = CollectionBehaviour.Exact;
    }

    public enum CollectionBehaviour
    {
        Exact,
        Probabilistic
    }
}