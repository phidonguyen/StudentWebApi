namespace SystemTech.Core.Messages
{
    public interface IPermanentDeleteQuery
    {
        public bool WithTrashed { get; set; }
    }

    public interface IPermanentDeleteParam
    {
        public bool Permanent { get; set; }
    }
}