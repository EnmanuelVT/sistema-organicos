namespace ENTIDAD.DTOs.Pruebas;

public class CreatePruebaDto
{
    public string IdMuestra { get; set; }
    public int IdParametroNorma { get; set; }

    public string NombrePrueba { get; set; } = null!;

    public byte TipoMuestraAsociada { get; set; }
}