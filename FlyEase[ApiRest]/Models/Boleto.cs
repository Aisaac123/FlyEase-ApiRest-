using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Boleto
{
    public int Idboleto { get; set; }

    public double? Precio { get; set; }

    public double Descuento { get; set; }

    public double? Preciototal { get; set; }

    public string Numerodocumento { get; set; }

    public int? Idasiento { get; set; }

    public int? Idvuelo { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual Asiento IdasientoNavigation { get; set; }

    public virtual Vuelo IdvueloNavigation { get; set; }

    public virtual Cliente NumerodocumentoNavigation { get; set; }
}
