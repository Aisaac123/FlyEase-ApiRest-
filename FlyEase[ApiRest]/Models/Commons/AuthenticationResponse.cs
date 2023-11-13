namespace FlyEase_ApiRest_.Models.Commons
{
    public class AuthenticationResponse
    {
        public TokenClass Tokens { get; set; } = new TokenClass();
        public bool Succes { get; set; }
        public string Msg { get; set; }
    }
}
