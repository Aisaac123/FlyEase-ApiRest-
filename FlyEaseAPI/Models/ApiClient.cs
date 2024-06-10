using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public class ApiClient
{
    public string Clientid { get; set; }

    [JsonIgnore] public string Nombre { get; set; }

    [JsonIgnore] public bool Activo { get; set; }

    [JsonIgnore] public string Token { get; set; }

    [JsonIgnore] public string Url { get; set; }

    [JsonIgnore] public DateTime FechaRegistro { get; set; }

    [JsonIgnore] public int TipoAplicacionId { get; set; }

    [JsonIgnore] public virtual TiposDeAplicacion TipoAplicacion { get; set; }
}