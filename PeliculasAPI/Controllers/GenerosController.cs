﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : CustomBaseController
    {
        //private readonly ApplicationDbContext _context;
        //private readonly IMapper _mapper;

        public GenerosController(ApplicationDbContext context, IMapper mapper)
            : base (context, mapper)
        {
            //_context = context;
            //_mapper = mapper;
        }

        // GET: api/Generos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneroDTO>>> Get()
        {
            //var entidades = await _context.Generos.ToListAsync();
            //var dtos = _mapper.Map<List<GeneroDTO>>(entidades);
            //return dtos;
            return await Get<Genero, GeneroDTO>();
        }

        // GET: api/Generos/5
        [HttpGet("{id}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            //var entidad = await _context.Generos.FindAsync(id);

            //if (entidad == null)
            //{
            //    return NotFound();
            //}

            //var dto = _mapper.Map<GeneroDTO>(entidad);

            //return dto;
            return await Get<Genero, GeneroDTO>(id);
        }

        // PUT: api/Generos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id ,[FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            //var entidad = _mapper.Map<Genero>(generoCreacionDTO);
            ////if (id != entidad.Id)
            ////{
            ////    return BadRequest();
            ////}
            //entidad.Id = id;
            //_context.Entry(entidad).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            //return NoContent();
            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);
        }

        // POST: api/Generos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Genero>> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            //var entidad = _mapper.Map<Genero>(generoCreacionDTO);
            //_context.Add(entidad);
            //await _context.SaveChangesAsync();
            //var generoDTO = _mapper.Map<GeneroDTO>(entidad);
            //return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id }, generoDTO);
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "obtenerGenero");
        }

        // DELETE: api/Generos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Genero>> Delete(int id)
        {
            //var genero = await _context.Generos.FindAsync(id);

            //if (genero == null)
            //{
            //    return NotFound();
            //}

            //_context.Generos.Remove(genero);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return await Delete(id);
        }

        //private bool GeneroExists(int id)
        //{
        //    return _context.Generos.Any(e => e.Id == id);
        //}
    }
}
