using ENTIDAD.DTOs.Users;
using ENTIDAD.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Usuario> _userManager;

        public UsuarioRepositorio(MasterDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserDto?> ObtenerUsuarioAsync(string usuarioId)
        {
            // Validate analyst ID if provided
            if (string.IsNullOrEmpty(usuarioId))
            {
                return null;
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == usuarioId);

            if (usuario == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(usuario);
            var primaryRole = roles.FirstOrDefault() ?? "SOLICITANTE"; // Default role if none found
            
            return new UserDto
            {
                Id = usuario.Id,
                UserName = usuario.UserName,
                Email = usuario.Email,
                UsCedula = usuario.UsCedula,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Role = primaryRole
            };
        }
    }
}
