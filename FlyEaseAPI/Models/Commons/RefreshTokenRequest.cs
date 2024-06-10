namespace FlyEase_ApiRest_.Models.Commons;

public class RefreshTokenRequest
{
    public string ExpiredToken { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}