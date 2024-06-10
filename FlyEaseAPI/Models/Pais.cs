using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public class Pais
{
    public int Idpais { get; set; }

    public string Nombre { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore] public virtual ICollection<Region> Regiones { get; set; } = new List<Region>();
}