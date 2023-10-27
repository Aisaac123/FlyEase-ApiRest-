using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Pais
{
    public int Idpais { get; set; }

    public string Nombre { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual ICollection<Region> Regiones { get; set; } = new List<Region>();
}
