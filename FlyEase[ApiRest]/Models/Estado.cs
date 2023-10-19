﻿using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Estado
{
    public int Idestado { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public bool Detencion { get; set; }

    public virtual ICollection<Vuelo> Vuelos { get; set; } = new List<Vuelo>();
}
