using ENTIDAD.DTOs.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Datos.Repositorios
{
    public class UsuarioRepositorio
    {
        private readonly MasterDbContext _context;

        public UsuarioRepositorio(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> ObtenerUsuarioAsync(string usuarioId)
        {
            // Validate analyst ID if provided
            if (!string.IsNullOrEmpty(usuarioId))
            {
                var usuarioExists = await _context.Users.AnyAsync(u => u.Id == usuarioId);
                if (!usuarioExists)
                {
                    throw new Exception($"The specified analyst ID {usuarioId} does not exist.");
                }
            }

            //var muestrasIds = _context.BitacoraMuestras
            //    .Where(b => b.IdAnalista == usuarioId)
            //    .Select(b => b.IdMuestra)
            //    .Distinct()
            //    .ToList();

            return await _context.Usuarios
                .Where(m => m.Id == usuarioId)
                .Select(m => new UserDto
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    Email = m.Email,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido
                })
                .ToListAsync();
        }

    }
}
