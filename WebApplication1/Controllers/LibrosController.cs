using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext contex;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext contex, IMapper mapper)
        {
            this.contex = contex;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libros = await contex.Libros
                .Include(libroBd => libroBd.AutoresLibros)
                .ThenInclude(autorLibroDb => autorLibroDb.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(libros == null)
                return NotFound();

            libros.AutoresLibros = libros.AutoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTO>(libros);
        }

        [HttpPost]
        public async Task<ActionResult<Libro>> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if(libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await contex.Autores
                 .Where(autorBD => libroCreacionDTO.AutoresIds.Contains(autorBD.Id))
                 .Select(x => x.Id).ToListAsync();

            if(libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            for (int i = 0; i < libro.AutoresLibros.Count; i++)
            {
                libro.AutoresLibros[i].Orden = i;
            }
            contex.Add(libro);
            await contex.SaveChangesAsync();
            return Ok();
        }



    }
}
