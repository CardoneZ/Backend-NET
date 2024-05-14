namespace Backend.Model;

public class CategoriaDTO
{
    public int? CategoriaId {get; set;}

    public required string Nombre {get; set;}

    public bool Protegida {get; set;} = false;
}