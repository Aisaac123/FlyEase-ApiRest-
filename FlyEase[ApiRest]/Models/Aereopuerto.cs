using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Aereopuerto
{
    public int Idaereopuerto { get; set; }

    public string Nombre { get; set; }

    public int? Idcoordenada { get; set; }

    public int? Idciudad { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual Ciudad IdciudadNavigation { get; set; }

    public virtual Coordenada IdcoordenadaNavigation { get; set; }

    public virtual ICollection<Vuelo> VueloIddespegueNavigations { get; set; } = new List<Vuelo>();

    public virtual ICollection<Vuelo> VueloIddestinoNavigations { get; set; } = new List<Vuelo>();
}
