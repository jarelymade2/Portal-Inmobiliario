using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models;
public class Visita
{
    public int Id { get; set; }

    [Required] public int InmuebleId { get; set; }
    public Inmueble Inmueble { get; set; } = null!;

    [Required] public string UsuarioId { get; set; } = "";

    [Required] public DateTime FechaInicio { get; set; }
    [Required] public DateTime FechaFin { get; set; }

    public EstadoVisita Estado { get; set; } = EstadoVisita.Solicitada;

    [StringLength(500)] public string? Notas { get; set; }
}
