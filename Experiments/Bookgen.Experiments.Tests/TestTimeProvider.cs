namespace Bookgen.Experiments.Tests;

public class TestTimeProvider : TimeProvider
{
    public override DateTimeOffset GetUtcNow()
    {
        return new DateTimeOffset(2026, 1, 1, 11, 12, 13, TimeSpan.Zero);
    }

    public override TimeZoneInfo LocalTimeZone
        => TimeZoneInfo.Utc;
}
