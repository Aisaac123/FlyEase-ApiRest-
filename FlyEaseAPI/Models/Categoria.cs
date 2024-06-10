using System.Text.Json.Serialization;

namespace FlyEase_ApiRest_.Models;

public class Categoria
{
    public int Idcategoria { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public bool Estadocategoria { get; set; }

    public double Tarifa { get; set; }

    public DateTime? Fecharegistro { get; set; }

    [JsonIgnore] public virtual ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();

    public bool Comercial { get; set; }
}