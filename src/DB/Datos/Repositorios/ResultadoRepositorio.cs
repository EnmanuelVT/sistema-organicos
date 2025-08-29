
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
    @p_valor DECIMAL(18,6),
    @p_unidad VARCHAR(30),
    @p_validado_por VARCHAR(30)
AS
*/

    public async Task<IEnumerable<ResultadoPruebaDto?>> ObtenerResultadosPorMuestraAsync(string idMuestra)
    {
        return await _context.ResultadoPruebas
            .Where(r => r.IdMuestra == idMuestra)
            .Select(r => new ResultadoPruebaDto
            {
                IdMuestra = r.IdMuestra,
                IdPrueba = r.IdPrueba,
                IdResultado = r.IdResultado,
                ValorObtenido = r.ValorObtenido,
                Unidad = r.Unidad,
                CumpleNorma = r.CumpleNorma,
                FechaRegistro = r.FechaRegistro,
                ValidadoPor = r.ValidadoPor
            })
            .ToListAsync();
    }
    
    public async Task<ResultadoPruebaDto?> ObtenerResultadoPorIdAsync(long idResultado)
    {
        return await _context.ResultadoPruebas
            .Where(r => r.IdResultado == idResultado)
            .Select(r => new ResultadoPruebaDto
            {
                IdMuestra = r.IdMuestra,
                IdPrueba = r.IdPrueba,
                IdResultado = r.IdResultado,
                ValorObtenido = r.ValorObtenido,
                Unidad = r.Unidad,
                CumpleNorma = r.CumpleNorma,
                FechaRegistro = r.FechaRegistro,
                ValidadoPor = r.ValidadoPor
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ResultadoPruebaDto?> RegistrarResultadoAsync(CreateResultadoPruebaDto createResultadoPruebaDto)
    {
        // recibir id usuario
        
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_registrar_resultado @p_MST_CODIGO = {0}, @p_id_prueba = {1}, @p_valor = {2}, @p_unidad = {3}, @p_validado_por = {4}",
            createResultadoPruebaDto.IdMuestra,
            createResultadoPruebaDto.IdPrueba,
            createResultadoPruebaDto.ValorObtenido,
            createResultadoPruebaDto.Unidad,
            createResultadoPruebaDto.ValidadoPor
        );

        if (result <= 0)
        {
            return null;
        }
        
        var resultadoEntry = await _context.ResultadoPruebas
            .Where(r => r.IdMuestra == createResultadoPruebaDto.IdMuestra && r.IdPrueba == createResultadoPruebaDto.IdPrueba)
            .OrderByDescending(r => r.FechaRegistro)
            .FirstOrDefaultAsync();
        
        ResultadoPruebaDto resultadoPruebaDto = new ResultadoPruebaDto()
        {
            IdMuestra = resultadoEntry!.IdMuestra,
            IdPrueba = resultadoEntry.IdPrueba,
            IdResultado = resultadoEntry.IdResultado,
            ValorObtenido = resultadoEntry.ValorObtenido,
            Unidad = resultadoEntry.Unidad,
            CumpleNorma = resultadoEntry.CumpleNorma,
            FechaRegistro = resultadoEntry.FechaRegistro,
            ValidadoPor = resultadoEntry.ValidadoPor
        };

        return resultadoPruebaDto;
    }

    public async Task<ResultadoPruebaDto?> ValidarResultadoAsync(ValidarResultadoDto validarResultadoDto)
    {
        if (validarResultadoDto.Accion != "Aprobado" && validarResultadoDto.Accion != "Rechazado")
        {
            throw new ArgumentException("Accion debe ser Aprobado o Rechazado");
        }
        
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_validar_resultado @id_resultado = {0}, @id_usuario = {1}, @accion = {2}, @obs = {3}",
            validarResultadoDto.IdResultado,
            validarResultadoDto.IdUsuario,
            validarResultadoDto.Accion,
            validarResultadoDto.Observaciones
        );

        if (result <= 0)
        {
            return null;
        }
        
        var resultadoEntry = await _context.ResultadoPruebas
            .Where(r => r.IdResultado == validarResultadoDto.IdResultado)
            .FirstOrDefaultAsync();

        var resultadoPruebaDto = new ResultadoPruebaDto()
        {
            IdMuestra = resultadoEntry!.IdMuestra,
            IdPrueba = resultadoEntry.IdPrueba,
            IdResultado = resultadoEntry.IdResultado,
            ValorObtenido = resultadoEntry.ValorObtenido,
            Unidad = resultadoEntry.Unidad,
            CumpleNorma = resultadoEntry.CumpleNorma,
            FechaRegistro = resultadoEntry.FechaRegistro,
            ValidadoPor = resultadoEntry.ValidadoPor
        };
        
        return resultadoPruebaDto;
    }
}