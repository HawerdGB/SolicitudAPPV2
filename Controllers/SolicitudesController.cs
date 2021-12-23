using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolicitudesAppV2.Data;
using SolicitudesAppV2.Models;
using PagedList;

namespace SolicitudesAppV2
{
    public class SolicitudesController : Controller
    {
        private readonly SolicitudesAppV2Context _context;

        public SolicitudesController(SolicitudesAppV2Context context)
        {
            _context = context;
        }

        // GET: Solicitudes
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SolicitudSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["FechaCSortParm"] = sortOrder == "Fecha" ? "Fecha_desc" : "Fecha";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var solicitudes = from s in _context.Solicitud.Include(s => s.Estado).Include(s => s.Persona) select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                solicitudes = solicitudes.Where(s => s.Estado.NombEstado.Contains (searchString));
               

            }
            switch (sortOrder)
            {
                case "id_desc":
                    solicitudes = solicitudes.OrderByDescending(s => s.ID);
                    break;
                case "Fecha":
                    solicitudes = solicitudes.OrderBy(s => s.FechaCreacion);
                    break;
                case "Fecha_desc":
                    solicitudes = solicitudes.OrderByDescending(s => s.FechaCreacion);
                    break;
                default:
                    solicitudes = solicitudes.OrderBy(s => s.ID);
                    break;
            }



            int pageSize = 5;
            return View(await PaginatedList<Solicitud>.CreateAsync(solicitudes.AsNoTracking(), pageNumber ?? 1, pageSize));


        }

        // GET: Solicitudes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicitud
                .Include(s => s.Estado)
                .Include(s => s.Persona)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // GET: Solicitudes/Create
        public IActionResult Create()
        {
            ViewData["EstadoID"] = new SelectList(_context.Estado, "ID", "NombEstado");
            ViewData["PersonaID"] = new SelectList(_context.Persona, "ID", "NombreCompleto");
            return View();
        }

        // POST: Solicitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonaID,EstadoID,Descripcion,ID,FechaCreacion,FechaUltMode")] Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {

                solicitud.FechaCreacion = DateTime.Now;
                solicitud.FechaUltMode = DateTime.Now;
                _context.Add(solicitud);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoID"] = new SelectList(_context.Estado, "ID", "NombEstado", solicitud.EstadoID);
            ViewData["PersonaID"] = new SelectList(_context.Persona, "ID", "NombreCompleto", solicitud.PersonaID);
            return View(solicitud);
        }

        // GET: Solicitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicitud.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            ViewData["EstadoID"] = new SelectList(_context.Estado, "ID", "NombEstado", solicitud.EstadoID);
            ViewData["PersonaID"] = new SelectList(_context.Persona, "ID", "NombreCompleto", solicitud.PersonaID);
            return View(solicitud);
        }

        // POST: Solicitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonaID,EstadoID,Descripcion,ID,FechaCreacion,FechaUltMode")] Solicitud solicitud)
        {
            if (id != solicitud.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    solicitud.FechaUltMode = DateTime.Now;
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudExists(solicitud.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoID"] = new SelectList(_context.Estado, "ID", "NombEstado", solicitud.EstadoID);
            ViewData["PersonaID"] = new SelectList(_context.Persona, "ID", "NombreCompleto", solicitud.PersonaID);
            return View(solicitud);
        }

        // GET: Solicitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicitud
                .Include(s => s.Estado)
                .Include(s => s.Persona)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: Solicitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.Solicitud.FindAsync(id);
            _context.Solicitud.Remove(solicitud);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicitud.Any(e => e.ID == id);
        }
    }
}
