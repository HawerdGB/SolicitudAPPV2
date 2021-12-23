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
    public class PersonasController : Controller
    {
        private readonly SolicitudesAppV2Context _context;

        public PersonasController(SolicitudesAppV2Context context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder)? "nombre_asc" : "" ;
            ViewData["ApellidoSortParm"] = sortOrder == "Apellido" ? "apellido_desc" : "Apellido";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            var personas = from p in _context.Persona
                           select p ;
           
            
            if (!String.IsNullOrEmpty(searchString))
            {
                personas = personas.Where(p => p.Nombre.Contains(searchString) || p.Apellido.Contains(searchString) );

            }
            switch (sortOrder)
            {
                case "nombre_asc":
                    personas = personas.OrderBy(p => p.Nombre);
                    break;
                case "Apellido":
                    personas = personas.OrderBy(p => p.Apellido);
                    break;
                case "apellido_desc":
                    personas = personas.OrderByDescending(p => p.Apellido);
                    break;
                default:
                    personas = personas.OrderBy(p => p.ID);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Persona>.CreateAsync(personas.AsNoTracking(), pageNumber ?? 1, pageSize));

            
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.ID == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Pasaporte,Direccion,Sexo,ID,FechaCreacion,FechaUltMode")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                persona.FechaUltMode = DateTime.Now;
                persona.FechaCreacion = DateTime.Now;

                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre,Apellido,Pasaporte,Direccion,Sexo,ID,FechaCreacion,FechaUltMode")] Persona persona)
        {
            if (id != persona.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    persona.FechaUltMode = DateTime.Now;
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.ID))
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.ID == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Persona.FindAsync(id);
            _context.Persona.Remove(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.ID == id);
        }
    }
}
