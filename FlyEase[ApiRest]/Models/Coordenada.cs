using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Coordenada
{
    public int Idcoordenada { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore]
    public virtual Aereopuerto Aereopuerto { get; set; }
}
