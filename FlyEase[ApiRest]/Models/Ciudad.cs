using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Ciudad
{
    public int Idciudad { get; set; }

    public string Nombre { get; set; }

    public int? Idregion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual ICollection<Aereopuerto> Aereopuertos { get; set; } = new List<Aereopuerto>();

    public virtual Region IdregionNavigation { get; set; }
}
