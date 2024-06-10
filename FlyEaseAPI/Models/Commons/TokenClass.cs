namespace FlyEase_ApiRest_.Models.Commons;

public class TokenClass
{
    public string PrimaryToken { get; set; }
    public string RefreshToken { get; set; }
    public bool AdminAuthentication { get; set; }
}