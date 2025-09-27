using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Data;
using PortalInmobiliario.Models;
using System.Security.Claims;

namespace PortalInmobiliario.Controllers;

[Authorize]
public class ReservasController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _um;

    public ReservasController(ApplicationDbContext db, UserManager<IdentityUser> um)
    {
        _db = db; _um = um;
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Reservar(int inmuebleId)
    {
        // Verifica inmueble activo
        var inmueble = await _db.Inmuebles.AsNoTracking().FirstOrDefaultAsync(i => i.Id == inmuebleId && i.Activo);
        if (inmueble is null)
        {
            TempData["err"] = "El inmueble no existe o no está activo.";
            return RedirectToAction("Catalogo", "Inmuebles");
        }

        // Rechaza si ya existe una reserva activa (no expirada)
        var ahora = DateTime.UtcNow;
        var hayReservaActiva = await _db.Reservas.AnyAsync(r => r.InmuebleId == inmuebleId && r.FechaExpiracion > ahora);
        if (hayReservaActiva)
        {
            TempData["err"] = "Ya existe una reserva activa para este inmueble.";
            return RedirectToAction("Detalle", "Inmuebles", new { id = inmuebleId });
        }

        // Usuario
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            TempData["err"] = "Debes iniciar sesión.";
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        _db.Reservas.Add(new Reserva
        {
            InmuebleId = inmuebleId,
            UsuarioId = userId,
            FechaCreacion = ahora,
            FechaExpiracion = ahora.AddHours(48)
        });
        await _db.SaveChangesAsync();

        TempData["ok"] = "Reserva creada por 48 horas.";
        return RedirectToAction("Detalle", "Inmuebles", new { id = inmuebleId });
    }
}
