namespace Abdt.Loyal.NoteSaver.Domain
{
    public record Result<T>(T? Value, string Message);
}
