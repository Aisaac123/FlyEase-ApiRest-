using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class TiposDeAplicacion
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    [JsonIgnore]
    public virtual ICollection<ApiClient> ApiClients { get; set; } = new List<ApiClient>();
}
