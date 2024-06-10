using System.Text.Json.Serialization;
using FlyEase_ApiRest_.Models.Commons;

namespace FlyEase_ApiRest_.Models;

public class Administrador
{
    public int Idadministrador { get; set; }

    public string Numerodocumento { get; set; }

    public string Tipodocumento { get; set; }

    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public string Celular { get; set; }

    public string Correo { get; set; }

    public bool Estado { get; set; }

    public string Usuario { get; set; }

    public string Clave { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore] public virtual ICollection<Refreshtoken> Refreshtokens { get; set; } = new List<Refreshtoken>();
}