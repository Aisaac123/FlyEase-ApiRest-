using System;
using System.Collections;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Vuelo
{
    public int Idvuelo { get; set; }

    public double Preciovuelo { get; set; }

    public double Tarifatemporada { get; set; }

    public double Descuento { get; set; }

    public double Distanciarecorrida { get; set; }

    public DateTime Fechayhorallegada { get; set; }

    public BitArray Cupo { get; set; }

    public int? Iddespegue { get; set; }

    public int? Iddestino { get; set; }

    public int? Idestado { get; set; }

    public string Idavion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public DateTime Fechayhoradesalida { get; set; }

    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();

    public virtual Avion IdavionNavigation { get; set; }

    public virtual Aereopuerto IddespegueNavigation { get; set; }

    public virtual Aereopuerto IddestinoNavigation { get; set; }

    public virtual Estado IdestadoNavigation { get; set; }
}
