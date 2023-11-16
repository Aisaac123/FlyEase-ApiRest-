using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Avion
{
    public string Idavion { get; set; }

    public string Nombre { get; set; }

    public string Modelo { get; set; }

    public string Fabricante { get; set; }

    public double Velocidadpromedio { get; set; }

    public int Cantidadpasajeros { get; set; }

    public double? Cantidadcarga { get; set; }

    [JsonIgnore]
    public int? Idaereolinea { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore]
    public virtual ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();

    public virtual Aerolinea Aereolinea { get; set; }

    [JsonIgnore]
    public virtual ICollection<Vuelo> Vuelos { get; set; } = new List<Vuelo>();
}
