using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Aereolinea
{
    public int Idaereolinea { get; set; }

    public string Nombre { get; set; }

    public string Codigoiata { get; set; }

    public string Codigoicao { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore]
    public virtual ICollection<Avion> Aviones { get; set; } = new List<Avion>();
}
