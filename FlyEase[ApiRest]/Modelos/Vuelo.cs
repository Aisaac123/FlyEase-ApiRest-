using Newtonsoft.Json;
using System.Collections;

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

    [JsonIgnore]
    public int? Iddespegue { get; set; }

    [JsonIgnore]
    public int? Iddestino { get; set; }

    [JsonIgnore]
    public int? Idestado { get; set; }

    [JsonIgnore]
    public string Idavion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public DateTime Fechayhoradesalida { get; set; }

    [JsonIgnore]
    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();

    public virtual Avion Avion { get; set; }

    public virtual Aereopuerto Aereopuerto_Despegue { get; set; }

    public virtual Aereopuerto Aereopuerto_Destino { get; set; }

    public virtual Estado Estado { get; set; }
}