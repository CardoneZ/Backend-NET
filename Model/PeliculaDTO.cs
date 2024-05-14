namespace Backend.Model;

  public class PeliculaDTO
{
    public int? PeliculaId {get; set;}

    public string Titulo {get; set;} = "Sin Título";

    public string Sinopsis {get; set;} = "Sin Sinopsis";

    public int Anio {get; set;}

    public string Poster {get; set;} = "N/A";

       public int[]? Categorias {get; set;}
}