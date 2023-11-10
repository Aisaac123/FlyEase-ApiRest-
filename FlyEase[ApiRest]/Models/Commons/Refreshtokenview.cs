using System;
using System.Collections.Generic;

namespace FlyEase_ApiRest_.Models.Commons;

public partial class Refreshtokenview
{
    public int? Idtoken { get; set; }

    public int? Idadmin { get; set; }

    public string Token { get; set; }

    public string Refreshtoken { get; set; }

    public DateTime? Fechacreacion { get; set; }

    public DateTime? Fechaexpiracion { get; set; }

    public bool? Esactivo { get; set; }

    public DateTime? Fecharegistro { get; set; }
}
