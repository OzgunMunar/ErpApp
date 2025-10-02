public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;

}
