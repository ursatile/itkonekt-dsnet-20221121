namespace Messages;
public class Greeting {
    public DateTimeOffset Timestamp { get;set; } = DateTimeOffset.UtcNow;
    public string Host { get; set; } = Environment.MachineName;
    public string Message { get;set; } = String.Empty;

    public override string ToString() {
        return $"{Message} (from {Host} at {Timestamp:O})";
    }
}
