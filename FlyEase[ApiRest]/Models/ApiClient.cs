using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class ApiClient
{
    public string Clientid { get; set; }

    public string Nombre { get; set; }

    public bool Activo { get; set; }

    public string Token { get; set; }

    public string Url { get; set; }

    public DateTime FechaRegistro { get; set; }

    [JsonIgnore]
    public int TipoAplicacionId { get; set; }

    public virtual TiposDeAplicacion TipoAplicacion { get; set; }
}
