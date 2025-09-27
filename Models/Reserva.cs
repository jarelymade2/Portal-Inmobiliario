using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models;
public class Reserva
{
    public int Id { get; set; }

    [Required] public int InmuebleId { get; set; }
    public Inmueble Inmueble { get; set; } = null!;

    [Required] public string UsuarioId { get; set; } = "";

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaExpiracion { get; set; } // se setea al crear (ahora +48h)
}
