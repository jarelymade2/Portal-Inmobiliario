using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models;
public class Inmueble
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string Codigo { get; set; } = "";

    [Required, StringLength(120)]
    public string Titulo { get; set; } = "";

    [Url] public string? Imagen { get; set; }

    [Required] public TipoInmueble Tipo { get; set; }

    [Required, StringLength(60)]
    public string Ciudad { get; set; } = "";

    [Required, StringLength(160)]
    public string Direccion { get; set; } = "";

    [Range(0, 50)] public int Dormitorios { get; set; }
    [Range(0, 50)] public int Banos { get; set; }
    [Range(1, 1000000)] public decimal MetrosCuadrados { get; set; }
    [Range(1, 100000000)] public decimal Precio { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Visita> Visitas { get; set; } = new List<Visita>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
