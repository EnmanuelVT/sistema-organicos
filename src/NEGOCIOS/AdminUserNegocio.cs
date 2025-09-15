using DB.Datos.Repositorios;
using ENTIDAD.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using ENTIDAD.Models;

namespace NEGOCIOS
{
    public class AdminUserNegocio
    {
        private readonly AdminUserRepositorio _repositorio;
        private readonly UserManager<Usuario> _userManager;

        public AdminUserNegocio(AdminUserRepositorio repositorio, UserManager<Usuario> userManager)
        {
            _repositorio = repositorio;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> ListarUsuariosAsync()
        {
            var usuarios = await _repositorio.ObtenerUsuariosAsync();
            var adminUsersDto = new List<UserDto>();

            foreach (var usuario in usuarios)
            {
                adminUsersDto.Add(new UserDto
                {
                    Id = usuario.Id,
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Role = (await _userManager.GetRolesAsync(usuario)).FirstOrDefault(),
                    UsCedula = usuario.UsCedula,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Estado = usuario.Estado
                });
            }

            return adminUsersDto;
        }

        public async Task<UserDto> CrearUsuarioAsync(CreateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email))
                throw new ArgumentException("El email es obligatorio.", nameof(userDto.Email));

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(userDto.Password));

            if (string.IsNullOrWhiteSpace(userDto.Role))
                throw new ArgumentException("Debe especificar un rol para el usuario.", nameof(userDto.Role));

            // Sugerencia: setear UserName = Email para evitar confusiones en el login
            var usuario = new Usuario
            {
                UserName       = userDto.Email,
                Email          = userDto.Email,
                UsCedula       = userDto.UsCedula,
                Nombre         = userDto.Nombre,
                Apellido       = userDto.Apellido,
                EmailConfirmed = true // <-- Si NO usas confirmación por correo. Si la usas, pon false.
            };

            // IMPORTANTe: usar SIEMPRE el password del DTO; Identity generará el hash
            var createResult = await _userManager.CreateAsync(usuario, userDto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception("Error al crear el usuario: " + errors);
            }

            // Asignar rol
            var roleResult = await _userManager.AddToRoleAsync(usuario, userDto.Role!);
            if (!roleResult.Succeeded)
            {
                // Si falla la asignación de rol, opcionalmente podrías eliminar el usuario creado para no dejar basura
                // await _userManager.DeleteAsync(usuario);
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception("Error al asignar el rol: " + errors);
            }

            return new UserDto
            {
                Id        = usuario.Id,
                UserName  = usuario.UserName,
                Email     = usuario.Email,
                Role      = userDto.Role,
                UsCedula  = usuario.UsCedula,
                Nombre    = usuario.Nombre,
                Apellido  = usuario.Apellido
            };
        }

        public async Task ActualizarUsuarioAsync(UserDto userDto)
        {
            var usuario = await _userManager.FindByIdAsync(userDto.Id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            if (!string.IsNullOrEmpty(userDto.UserName)) 
            {
                usuario.UserName = userDto.UserName;
            }
            if (!string.IsNullOrEmpty(userDto.Email)) 
            {
                usuario.Email = userDto.Email;
            }
            if (userDto.Estado != null) 
            {
                usuario.Estado = userDto.Estado;
            }

            // change role if necessary
            var currentRole = (await _userManager.GetRolesAsync(usuario)).FirstOrDefault();
            if (currentRole != userDto.Role)
            {
                if (!string.IsNullOrEmpty(currentRole))
                {
                    await _userManager.RemoveFromRoleAsync(usuario, currentRole);
                }
                if (!string.IsNullOrEmpty(userDto.Role))
                {
                    await _userManager.AddToRoleAsync(usuario, userDto.Role);
                }
            }

            var result = await _userManager.UpdateAsync(usuario);
            if (!result.Succeeded)
            {
                throw new Exception("Error al actualizar el usuario: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task EliminarUsuarioAsync(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            var result = await _userManager.DeleteAsync(usuario);
            if (!result.Succeeded)
            {
                throw new Exception("Error al eliminar el usuario: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}