using SIGERD.Models.Seguridad;
using SIGERD.ViewModels.Seguridad.Usuarios;

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
                NombreUsuario = usuario.nombreUsuario,
                Correo = usuario.correo,
                Rol = usuario.Rol?.nombreRol ?? "Sin rol",
                Delegacion = usuario.Delegacion?.nombreDelegacion ?? "Sin delegación",
                Estado = usuario.estado
            };
        }

        public static UsuarioDetailsViewModel ToDetailsViewModel(Usuario usuario)
        {
            return new UsuarioDetailsViewModel
            {
                IdUsuario = usuario.idUsuario,
                NombreCompleto = usuario.nombreCompleto,
                NombreUsuario = usuario.nombreUsuario,
                Correo = usuario.correo,
                Rol = usuario.Rol?.nombreRol ?? "Sin rol asignado",
                Delegacion = usuario.Delegacion?.nombreDelegacion ?? "Sin delegación asignada",
                Estado = usuario.estado,
                DebeCambiarClave = usuario.debeCambiarClave
            };
        }

        #endregion

        #region ViewModel -> Entity

        public static Usuario ToEntity(UsuarioCreateViewModel model)
        {
            return new Usuario
            {
                nombreCompleto = model.NombreCompleto.Trim(),
                nombreUsuario = model.NombreUsuario.Trim().ToLower(),
                correo = model.Correo.Trim().ToLower(),
                idRolUsuario = model.IdRol,
                idDelegacionUsuario = model.IdDelegacion,
                estado = true,
                debeCambiarClave = true,
                VersionSeguridad = Guid.NewGuid()
            };
        }

        #endregion
    }
}