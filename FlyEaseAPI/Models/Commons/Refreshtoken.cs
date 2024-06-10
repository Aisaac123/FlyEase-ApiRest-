namespace FlyEase_ApiRest_.Models.Commons;

public class Refreshtoken
{
    public int Idtoken { get; set; }

    public int? IdUser { get; set; }

    public string Token { get; set; }

    public string RefreshtokenAtributte { get; set; }

    public DateTimeOffset? Fechacreacion { get; set; } = DateTime.Now;

    public DateTimeOffset? Fechaexpiracion { get; set; }

    public DateTime? Fecharegistro { get; set; } = DateTime.Now;

    public virtual Administrador Admin { get; set; }
}