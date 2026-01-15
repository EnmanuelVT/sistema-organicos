
using ENTIDAD.DTOs.ResultadosPruebas;
using Microsoft.EntityFrameworkCore;

namespace DB.Datos.Repositorios;

public class ResultadoRepositorio
{
    private readonly MasterDbContext _context;
    
    public ResultadoRepositorio(MasterDbContext context)
    {
        _context = context;
    }
/*
CREATE PROCEDURE sp_registrar_resultado 
    @p_MST_CODIGO VARCHAR(30),
    @p_id_prueba INT,
    @p_id_parametro INT,
    @p_valor DECIMAL(18,6),
    @p_unidad VARCHAR(30),
    @p_validado_por VARCHAR(30)
AS
*/

    public async Task<IEnumerable<ResultadoPruebaDto?>> ObtenerResultadosPorMuestraAsync(string idMuestra)
    {
        return await (
                from r in _context.ResultadoPruebas
                join p in _context.ParametroNormas on r.IdParametro equals p.IdParametro into paramJoin
                from p in paramJoin.DefaultIfEmpty()
                where r.IdMuestra == idMuestra
                select new ResultadoPruebaDto
                {
                    IdMuestra = r.IdMuestra,
                    IdPrueba = r.IdPrueba,
                    IdParametro = r.IdParametro,
                    NombreParametro = p != null ? p.NombreParametro : null,
                    IdResultado = r.IdResultado,
                    ValorObtenido = r.ValorObtenido,
                    Unidad = r.Unidad,
                    CumpleNorma = r.CumpleNorma,
                    FechaRegistro = r.FechaRegistro,
                    ValidadoPor = r.ValidadoPor,
                    EstadoValidacion = r.EstadoValidacion
                })
            .ToListAsync();
    }
    
    public async Task<ResultadoPruebaDto?> ObtenerResultadoPorIdAsync(long idResultado)
    {
        return await (
                from r in _context.ResultadoPruebas
                join p in _context.ParametroNormas on r.IdParametro equals p.IdParametro into paramJoin
                from p in paramJoin.DefaultIfEmpty()
                where r.IdResultado == idResultado
                select new ResultadoPruebaDto
                {
                    IdMuestra = r.IdMuestra,
                    IdPrueba = r.IdPrueba,
                    IdParametro = r.IdParametro,
                    NombreParametro = p != null ? p.NombreParametro : null,
                    IdResultado = r.IdResultado,
                    ValorObtenido = r.ValorObtenido,
                    Unidad = r.Unidad,
                    CumpleNorma = r.CumpleNorma,
                    FechaRegistro = r.FechaRegistro,
                    ValidadoPor = r.ValidadoPor,
                    EstadoValidacion = r.EstadoValidacion
                })
            .FirstOrDefaultAsync();
    }

    public async Task<ResultadoPruebaDto?> RegistrarResultadoAsync(CreateResultadoPruebaDto createResultadoPruebaDto, string idUsuario)
    {
        // recibir id usuario
        
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_registrar_resultado @p_MST_CODIGO = {0}, @p_id_prueba = {1}, @p_id_parametro = {2}, @p_valor = {3}, @p_unidad = {4}, @p_id_usuario = {5}",
            createResultadoPruebaDto.IdMuestra,
            createResultadoPruebaDto.IdPrueba,
            createResultadoPruebaDto.IdParametro,
            createResultadoPruebaDto.ValorObtenido,
            createResultadoPruebaDto.Unidad,
            idUsuario // no se ha validado
        );

        if (result <= 0)
        {
            return null;
        }
        
        var resultadoDto = await (
                from r in _context.ResultadoPruebas
                join p in _context.ParametroNormas on r.IdParametro equals p.IdParametro into paramJoin
                from p in paramJoin.DefaultIfEmpty()
                where r.IdMuestra == createResultadoPruebaDto.IdMuestra && r.IdPrueba == createResultadoPruebaDto.IdPrueba
                orderby r.FechaRegistro descending
                select new ResultadoPruebaDto
                {
                    IdMuestra = r.IdMuestra,
                    IdPrueba = r.IdPrueba,
                    IdParametro = r.IdParametro,
                    NombreParametro = p != null ? p.NombreParametro : null,
                    IdResultado = r.IdResultado,
                    ValorObtenido = r.ValorObtenido,
                    Unidad = r.Unidad,
                    CumpleNorma = r.CumpleNorma,
                    FechaRegistro = r.FechaRegistro,
                    ValidadoPor = r.ValidadoPor,
                    EstadoValidacion = r.EstadoValidacion
                })
            .FirstOrDefaultAsync();

        return resultadoDto;
    }
}