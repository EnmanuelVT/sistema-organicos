using ENTIDAD.DTOs;
using ENTIDAD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DB.Datos.Repositorios
{
    public class AdminUserRepositorio
    {
        private readonly UserManager<Usuario> _userManager;

        public AdminUserRepositorio(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Usuario> AddUserAsync(UserDto adminUserDto)
        {
            var user = new Usuario
            {
                UserName = adminUserDto.UserName,
                Email = adminUserDto.Email,
                UsCedula = adminUserDto.UsCedula,
                Nombre = adminUserDto.Nombre,
                Apellido = adminUserDto.Apellido
            };

            var result = await _userManager.CreateAsync(user, adminUserDto.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return user;
            }

            throw new Exception("Error creating user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<Usuario?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<Usuario>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task UpdateUserAsync(UserDto adminUserDto)
        {
            var user = await _userManager.FindByIdAsync(adminUserDto.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.UserName = adminUserDto.UserName;
            user.Email = adminUserDto.Email;
            user.UsCedula = adminUserDto.UsCedula;
            user.Nombre = adminUserDto.Nombre;
            user.Apellido = adminUserDto.Apellido;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Error updating user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Error deleting user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
        {
            // Returns all users as Usuario (derived class)
            var usuarios = await _userManager.Users.ToListAsync();
            return usuarios;
        }
    }
}