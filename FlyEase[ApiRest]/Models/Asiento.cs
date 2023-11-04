using System.Text.Json.Serialization;
using System.Collections;

namespace FlyEase_ApiRest_.Models;

public partial class Asiento
{
    public int Idasiento { get; set; }

    public int Posicion { get; set; }

    public bool Disponibilidad { get; set; }

    [JsonIgnore]
    public int? Idcategoria { get; set; }

    [JsonIgnore]
    public string Idavion { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore]
    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();

    public virtual Avion Avion { get; set; }

    public virtual Categoria Categoria { get; set; }
}