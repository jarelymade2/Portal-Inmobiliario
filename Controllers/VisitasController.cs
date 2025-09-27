using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Data;
using PortalInmobiliario.Models;

namespace PortalInmobiliario.Controllers;

[Authorize]
public class VisitasController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _um;
    public VisitasController(ApplicationDbContext db, UserManager<IdentityUser> um) { _db=db; _um=um; }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Agendar(int inmuebleId, DateTime fechaInicio, DateTime fechaFin, string? notas)
    {
        if (fechaInicio >= fechaFin) { TempData["err"]="Fin debe ser mayor a inicio"; return RedirectToAction("Detalle","Inmuebles", new { id = inmuebleId }); }
        if (fechaInicio.Hour < 8 || fechaFin.Hour > 19) { TempData["err"]="Visitas solo 08:00–19:00"; return RedirectToAction("Detalle","Inmuebles", new { id = inmuebleId }); }

        var solape = await _db.Visitas.AnyAsync(v =>
            v.InmuebleId == inmuebleId && v.Estado != EstadoVisita.Cancelada &&
            (fechaInicio < v.FechaFin && fechaFin > v.FechaInicio));
        if (solape) { TempData["err"]="Intervalo solapado con otra visita"; return RedirectToAction("Detalle","Inmuebles", new { id = inmuebleId }); }

        var user = await _um.GetUserAsync(User);
        _db.Visitas.Add(new Visita {
            InmuebleId = inmuebleId,
            UsuarioId = user!.Id,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            Notas = notas ?? "",
            Estado = EstadoVisita.Solicitada
        });
        await _db.SaveChangesAsync();
        TempData["ok"]="Visita solicitada";
        return RedirectToAction("Detalle","Inmuebles", new { id = inmuebleId });
    }
}
