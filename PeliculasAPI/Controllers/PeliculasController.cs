using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string _contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
        }

        // GET: api/Peliculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeliculaDTO>>> Get()
        {
            var peliculas = await _context.Peliculas.ToListAsync();
            return _mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        // GET: api/Peliculas/5
        [HttpGet("{id}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return _mapper.Map<PeliculaDTO>(pelicula);
        }

        // PUT: api/Peliculas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = await _context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasGeneros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (peliculaDB == null)
            {
                return NotFound();
            }

            peliculaDB = _mapper.Map(peliculaCreacionDTO, peliculaDB);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memorystream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memorystream);
                    var contenido = memorystream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await _almacenadorArchivos.EditarArchivo(contenido, extension, _contenedor, peliculaDB.Poster, peliculaCreacionDTO.Poster.ContentType);
                }
            }

            AsignarOrdenActores(peliculaDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Peliculas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PeliculaDTO>> PostPelicula([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = _mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memorystream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memorystream);
                    var contenido = memorystream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await _almacenadorArchivos.GuardarArchivo(contenido, extension, _contenedor, peliculaCreacionDTO.Poster.ContentType);
                }
            }

            AsignarOrdenActores(pelicula);
            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            var peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);

            return new CreatedAtRouteResult("obtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }

        // DELETE: api/Peliculas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pelicula>> DeletePelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();

            return pelicula;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await _context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = _mapper.Map<PeliculaPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var valid = TryValidateModel(entidadDTO);

            if (!valid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(entidadDTO, entidadDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if(pelicula.PeliculasActores != null)
            {
                for(int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }
    }
}
