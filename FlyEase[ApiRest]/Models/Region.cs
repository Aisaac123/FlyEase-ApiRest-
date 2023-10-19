using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Region
{
    public int Idregion { get; set; }

    public string Nombre { get; set; }

    public int Idpais { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual ICollection<Ciudad> Ciudades { get; set; } = new List<Ciudad>();

    public virtual Pais IdpaisNavigation { get; set; }
}
