using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public partial class Aeropuerto
{
    public int Idaereopuerto { get; set; }

    public string Nombre { get; set; }

    [JsonIgnore]
    public int? Idcoordenada { get; set; }

    [JsonIgnore]
    public int? Idciudad { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual Ciudad Ciudad { get; set; }

    public virtual Coordenada Coordenadas { get; set; }

    [JsonIgnore]
    public virtual ICollection<Vuelo> Despegues { get; set; } = new List<Vuelo>();

    [JsonIgnore]
    public virtual ICollection<Vuelo> Destinos { get; set; } = new List<Vuelo>();
}