using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await _context.Autores
                 .ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }


        //[HttpGet("{id:int}/{param2?}")] el ? es para marcarlo como opcional
        //[HttpGet("{id:int}/{param2=persona}")] = para asignar un default
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var autor = await _context.Autores
                .FirstOrDefaultAsync(autorBd => autorBd.Id == id);

            if (autor == null)
                return NotFound();

            return mapper.Map<AutorDTO>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute]string nombre)
        {
            var autores = await _context.Autores
                .Where(x => x.Nombre.Contains(nombre))
                .ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromBody]AutorCreacionDTO autroCreacionDTO)
        {
            var existeAutorConElMismoNombre =await _context.Autores.AnyAsync(x => x.Nombre == autroCreacionDTO.Nombre);
            if(existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autroCreacionDTO.Nombre}");
            }

            var autor = mapper.Map<Autor>(autroCreacionDTO);
            _context.Add(autor);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }
            _context.Update(autor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            _context.Remove(new Autor() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
