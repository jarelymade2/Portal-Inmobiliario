using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Data;
using PortalInmobiliario.Models;

namespace PortalInmobiliario.Controllers;

public class InmueblesController : Controller
{
    private readonly ApplicationDbContext _db;
    public InmueblesController(ApplicationDbContext db) => _db = db;

    // GET: /Inmuebles/Catalogo
    public async Task<IActionResult> Catalogo(string? ciudad, TipoInmueble? tipo,
        decimal? precioMin, decimal? precioMax, int? dormitorios, int page=1, int pageSize=10)
    {
        // Validaciones server-side
        if (precioMin is < 0 || precioMax is < 0)
            ModelState.AddModelError("", "Precios no pueden ser negativos.");
        if (precioMin.HasValue && precioMax.HasValue && precioMin > precioMax)
            ModelState.AddModelError("", "Precio mínimo no puede ser mayor al máximo.");
        if (!ModelState.IsValid)
            return View(Enumerable.Empty<Inmueble>());

        var q = _db.Inmuebles.AsNoTracking().Where(i => i.Activo);
        if (!string.IsNullOrWhiteSpace(ciudad)) q = q.Where(i => i.Ciudad == ciudad);
        if (tipo.HasValue) q = q.Where(i => i.Tipo == tipo);
        if (precioMin.HasValue) q = q.Where(i => i.Precio >= precioMin);
        if (precioMax.HasValue) q = q.Where(i => i.Precio <= precioMax);
        if (dormitorios.HasValue) q = q.Where(i => i.Dormitorios >= dormitorios);

        var items = await q.OrderByDescending(i => i.Id)
            .Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

        ViewData["FiltroCiudad"] = ciudad;
        ViewData["FiltroTipo"] = tipo;
        ViewData["FiltroDorms"] = dormitorios;
        ViewData["FiltroMin"] = precioMin;
        ViewData["FiltroMax"] = precioMax;

        return View(items);
    }

    // GET: /Inmuebles/Detalle/5
    public async Task<IActionResult> Detalle(int id)
    {
        var item = await _db.Inmuebles.FindAsync(id);
        if (item is null || !item.Activo) return NotFound();
        return View(item);
    }
}
