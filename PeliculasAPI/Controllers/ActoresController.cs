using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string _contenedor = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
        }

        // GET: api/Actores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return _mapper.Map<List<ActorDTO>>(entidades);
        }

        // GET: api/Actores/5
        [HttpGet("{id}", Name ="obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await _context.Actores.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return _mapper.Map<ActorDTO>(actor);
        }

        // PUT: api/Actores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] ActorCreacionDTO actor)
        {
            var actorDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if(actorDB == null)
            {
                return NotFound();
            }

            actorDB = _mapper.Map(actor, actorDB);

            if (actor.Foto != null)
            {
                using (var memorystream = new MemoryStream())
                {
                    await actor.Foto.CopyToAsync(memorystream);
                    var contenido = memorystream.ToArray();
                    var extension = Path.GetExtension(actor.Foto.FileName);
                    actorDB.Foto = await _almacenadorArchivos.EditarArchivo(contenido, extension, _contenedor, actorDB.Foto, actor.Foto.ContentType);
                }
            }

            //var entidad = _mapper.Map<Actor>(actor);
            //if (id != entidad.Id)
            //{
            //    return BadRequest();
            //}
            //entidad.Id = id;
            //_context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Actores
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Actor>> Post([FromForm] ActorCreacionDTO actor)
        {
            var entidad = _mapper.Map<Actor>(actor);

            if (actor.Foto != null)
            {
                using(var memorystream = new MemoryStream())
                {
                    await actor.Foto.CopyToAsync(memorystream);
                    var contenido = memorystream.ToArray();
                    var extension = Path.GetExtension(actor.Foto.FileName);
                    entidad.Foto = await _almacenadorArchivos.GuardarArchivo(contenido,extension,_contenedor,actor.Foto.ContentType);
                }
            }

            _context.Actores.Add(entidad);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<ActorDTO>(entidad);

            return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);
        }

        // DELETE: api/Actores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Actor>> Delete(int id)
        {
            var actor = await _context.Actores.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            _context.Actores.Remove(actor);
            await _context.SaveChangesAsync();

            return actor;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if(entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = _mapper.Map<ActorPatchDTO>(entidadDB);

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

        private bool ActorExists(int id)
        {
            return _context.Actores.Any(e => e.Id == id);
        }
    }
}
