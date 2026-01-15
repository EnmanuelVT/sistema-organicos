namespace ENTIDAD.DTOs.Pruebas;

public class CreatePruebaDto
{
    public string IdMuestra { get; set; }

    public int TipoPruebaId { get; set; }

    public string NombrePrueba { get; set; } = null!;
}