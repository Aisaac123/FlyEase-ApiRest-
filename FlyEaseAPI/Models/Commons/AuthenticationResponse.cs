namespace FlyEase_ApiRest_.Models.Commons;

public class AuthenticationResponse
{
    public TokenClass Tokens { get; set; } = new();
    public bool Succes { get; set; }
    public string Msg { get; set; }
}