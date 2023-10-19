using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models;

public partial class Cliente
{
    public string Numerodocumento { get; set; }

    public string Tipodocumento { get; set; }

    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public string Celular { get; set; }

    public string Correo { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
}
