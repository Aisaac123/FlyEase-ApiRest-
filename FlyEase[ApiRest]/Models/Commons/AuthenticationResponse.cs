namespace FlyEase_ApiRest_.Models.Commons
{
    public class AuthenticationResponse
    {
        public TokenClass Token { get; set; } = new TokenClass();
        public bool Succes { get; set; }
        public string Msg { get; set; }
    }
}
