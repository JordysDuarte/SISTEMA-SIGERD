using SIGERD.ViewModels.Seguridad.Usuarios;
using SIGERD.Models.Seguridad;

namespace SIGERD.Mappings
{
    public static class UsuarioMapper
    {
        #region Entity -> ViewModel
        public static UsuarioListViewModel ToListViewModel(Usuario usuario)
        {
            return new UsuarioListViewModel
            {
                IdUsuario = usuario.idUsuario,
                NombreCompleto = usuario.nombreCompleto,
                Correo = usuario.correo,
                Rol = usuario.Rol?.nombreRol ?? string.Empty,
                Estado = usuario.estado
            };
        }

        public static UsuarioDetailsViewModel ToDetailsViewModel(Usuario usuario)
        {
            return new UsuarioDetailsViewModel
            {
                IdUsuario = usuario.idUsuario,
                NombreCompleto = usuario.nombreCompleto,
                Correo = usuario.correo,
                Rol = usuario.Rol?.nombreRol ?? "Sin rol asignado",
                Estado = usuario.estado 
            };
        }

        public static UsuarioEditViewModel ToEditViewModel(Usuario usuario)
        {
            return new UsuarioEditViewModel
            {
                IdUsuario = usuario.idUsuario,
                NombreCompleto = usuario.nombreCompleto,
                Correo = usuario.correo,
                Estado = usuario.estado,
                IdRol = usuario.idRolUsuario
            };
        }

        #endregion

        #region ViewModel -> Entity

        public static Usuario ToEntity(UsuarioCreateViewModel model)
        {
            return new Usuario
            {
                nombreCompleto = model.NombreCompleto,
                correo = model.Correo,
                clave = model.Clave,
                idRolUsuario = model.IdRol
            };
        }

        public static Usuario ToEntity(UsuarioEditViewModel model)
        {
            return new Usuario
            {
                idUsuario = model.IdUsuario,
                nombreCompleto = model.NombreCompleto,
                correo = model.Correo,
                idRolUsuario = model.IdRol,
                estado = model.Estado
            };
        }

        #endregion
    }
}
