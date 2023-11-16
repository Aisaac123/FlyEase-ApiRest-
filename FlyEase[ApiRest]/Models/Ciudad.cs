using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Ciudad
{
    public int Idciudad { get; set; }

    public string Nombre { get; set; }

    [JsonIgnore]
    public int? Idregion { get; set; }

    public DateTime? Fecharegistro { get; set; }
    [JsonIgnore]

    public byte[] Imagen { get; set; }

    [JsonIgnore]
    public virtual ICollection<Aeropuerto> ListaAereopuertos { get; set; } = new List<Aeropuerto>();

    public virtual Region Region { get; set; }
}
