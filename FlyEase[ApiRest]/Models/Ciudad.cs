using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Ciudad
{
    public int Idciudad { get; set; }

    public string Nombre { get; set; }

    public int? Idregion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore]
    public virtual ICollection<Aereopuerto> ListaAereopuertos { get; set; } = new List<Aereopuerto>();

    public virtual Region Region { get; set; }
}
