using System;
using System.Collections;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Asiento
{
    public int Idasiento { get; set; }

    public int Posicion { get; set; }

    public BitArray Disponibilidad { get; set; }

    public int? Idcategoria { get; set; }

    public string Idavion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();

    public virtual Avion IdavionNavigation { get; set; }

    public virtual Categoria IdcategoriaNavigation { get; set; }
}
