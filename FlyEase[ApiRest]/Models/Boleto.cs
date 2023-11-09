using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Boleto
{
    public int Idboleto { get; set; }

    public double? Precio { get; set; }

    public double Descuento { get; set; }

    public double? Preciototal { get; set; }

    [JsonIgnore]
    public string Numerodocumento { get; set; }

    [JsonIgnore]

    public int? Idasiento { get; set; }

    [JsonIgnore]
    public int? Idvuelo { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual Asiento Asiento { get; set; }

    public virtual Vuelo Vuelo { get; set; }

    public virtual Cliente Cliente { get; set; }
}
