namespace FlyEase_ApiRest_.Models.Commons;

public class Refreshtokenview
{
    public int? Idtoken { get; set; }

    public int? Idadmin { get; set; }

    public string Token { get; set; }

    public string Refreshtoken { get; set; }

    public DateTimeOffset? Fechacreacion { get; set; }

    public DateTimeOffset? Fechaexpiracion { get; set; }

    public bool? Esactivo { get; set; }

    public DateTime? Fecharegistro { get; set; }
}