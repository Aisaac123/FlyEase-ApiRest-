using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models.Commons;

public partial class Refreshtoken
{
    public int Idtoken { get; set; }

    public int? IdUser { get; set; }

    public string Token { get; set; }

    public string RefreshtokenAtributte { get; set; }

    public DateTime? Fechacreacion { get; set; } = DateTime.Now;

    public DateTime? Fechaexpiracion { get; set; }

    public DateTime? Fecharegistro { get; set; } = DateTime.Now;

    public virtual Administrador Admin { get; set; }
}
