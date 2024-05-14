using Backend.Data;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PeliculasController(DataContext context) : Controller
{
    //GET: api/peliculas´s=titulo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pelicula>>> GetPeliculas(string? s)
    {
        if(string.IsNullOrEmpty(s))
            return await context.Pelicula.Include(i=>i.categorias).AsNoTracking().ToListAsync();
        return await context.Pelicula.Include(i=>i.categorias).Where(c=>c.Titulo.Contains(s)).AsNoTracking().ToListAsync();
    }

    //GET: api/peliculas/5
   [HttpGet("{id}")]
    public async Task<ActionResult<Pelicula>> GetPelicula(int id)
    {
        var pelicula = await context.Pelicula.Include(i=>i.categorias).AsNoTracking().FirstOrDefaultAsync(s => s.PeliculaId == id);
        if(pelicula==null) return NotFound();

        return pelicula;
    }

    //POST: api/peliculas
    [HttpPost]
    public async Task<IActionResult> PostPelicula (PeliculaDTO peliculaDTO)
    {
        Pelicula pelicula = new()
        {
            Titulo = peliculaDTO.Titulo,
            Sinopsis = peliculaDTO.Sinopsis,
            Anio = peliculaDTO.Anio,
            Poster = peliculaDTO.Poster,
            categorias = new List<Categoria>() 
        };

        context.Pelicula.Add(pelicula);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPelicula), new{id=pelicula.PeliculaId}, pelicula);
    }

    //PUT: api/peliculas/5
    [HttpPut("{id}")]
    public async Task<IActionResult>PutPelicula(int id, PeliculaDTO peliculaDTO)
    {
        if (id != peliculaDTO.PeliculaId) return BadRequest();

        var pelicula = await context.Pelicula.FirstOrDefaultAsync(s=>s.PeliculaId==id);
        if(pelicula==null) return NotFound();

        pelicula.Titulo = peliculaDTO.Titulo;
        pelicula.Sinopsis = peliculaDTO.Sinopsis;
        pelicula.Anio = peliculaDTO.Anio;
        pelicula.Poster = peliculaDTO.Poster;
        await context.SaveChangesAsync();

        return NoContent();
    }

    //DELETE: api/peliculas/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Pelicula>> DeletePelicula (int id)
    {
        var pelicula = await context.Pelicula.FindAsync(id);
        if(pelicula==null) return NotFound();

        context.Pelicula.Remove(pelicula);
        await context.SaveChangesAsync();

        return NoContent();
    }

    //POST: api/peliculas/5/categoria
    [HttpPost("{id}/categoria")]
    public async Task<IActionResult> PostCategoriaPelicula (int id, AsignaCategoriaDTO itemToAdd)
    {
        Categoria? categoria = await context.Categoria.FindAsync(itemToAdd.CategoriaId);
        if(categoria==null) return NotFound();

        var pelicula = await context.Pelicula.Include(i => i.categorias).FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula==null) return NotFound();

        if (pelicula?.categorias?.FirstOrDefault(categoria)!=null)
        {
            pelicula.categorias.Add(categoria);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }

    //DELETE: api/peliculas/5/categoria/1
    [HttpDelete("{id}/categoria/{categoriaid}")]
    public async Task<IActionResult> DeleteCategoriaPelicula (int id, int categoriaid)
    {
        Categoria? categoria = await context.Categoria.FindAsync(categoriaid);
        if(categoria==null) return NotFound();

        var pelicula = await context.Pelicula.Include(i => i.categorias).FirstOrDefaultAsync(s => s.PeliculaId == id);
        if(pelicula == null) return NotFound();

        if(pelicula?.categorias?.FirstOrDefault(categoria)!=null)
        {
            pelicula.categorias.Remove(categoria);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}