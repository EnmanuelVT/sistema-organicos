namespace ENTIDAD.DTOs.ResultadosPruebas;

public class ResultadoParaDocumentoDto
{
    public int IdParametro { get; set; }             // enlaza directo con ParametroNorma.IdParametro
    public decimal ValorObtenido { get; set; }       // el valor que quieres plasmar en el certificado
    public string? Unidad { get; set; }       // si lo dejas null, tomamos la unidad desde ParametroNorma
    public string? Alias { get; set; } // opcional: nombre corto para mapear a la celda del template
}