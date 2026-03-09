namespace BPUA.Application.Contracts
{
    public sealed class UseCaseActivationStamp
    {
        public string DefaultRoute { get; set; } = "/u/application";
        public System.DateTime TimestampUtc { get; set; } = System.DateTime.UtcNow;
    }
}
