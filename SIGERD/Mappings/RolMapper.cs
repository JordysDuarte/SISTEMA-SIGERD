using SIGERD.Constants.Seguridad;
using SIGERD.Models.Seguridad;
using SIGERD.ViewModels.Seguridad.Roles;

namespace SIGERD.Mappings
{
    public static class RolMapper
    {
        public static RolListViewModel ToListViewModel(Rol rol)
        {
            return new RolListViewModel
            {
                IdRol = rol.idRol,
                NombreRol = rol.nombreRol ?? string.Empty,
                Descripcion = rol.descripcion,
                EsRolBase = EsRolBase(rol.nombreRol)
            };
        }

        public static RolDetailsViewModel ToDetailsViewModel(Rol rol)
        {
            return new RolDetailsViewModel
            {
                IdRol = rol.idRol,
                NombreRol = rol.nombreRol ?? string.Empty,
                Descripcion = rol.descripcion,
                EsRolBase = EsRolBase(rol.nombreRol)
            };
        }

        public static RolEditViewModel ToEditViewModel(Rol rol)
        {
            return new RolEditViewModel
            {
                IdRol = rol.idRol,
                NombreRol = rol.nombreRol ?? string.Empty,
                Descripcion = rol.descripcion,
                EsRolBase = EsRolBase(rol.nombreRol)
            };
        }

        public static Rol ToEntity(RolCreateViewModel model)
        {
            return new Rol
            {
                nombreRol = model.NombreRol.Trim(),
                descripcion = model.Descripcion?.Trim()
            };
        }

        public static Rol ToEntity(RolEditViewModel model)
        {
            return new Rol
            {
                idRol = model.IdRol,
                nombreRol = model.NombreRol.Trim(),
                descripcion = model.Descripcion?.Trim()
            };
        }

        private static bool EsRolBase(string? nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                return false;
            }

            return nombreRol.Equals(RolesSistema.SuperAdministrador, StringComparison.OrdinalIgnoreCase)
                || nombreRol.Equals(RolesSistema.Administrador, StringComparison.OrdinalIgnoreCase)
                || nombreRol.Equals(RolesSistema.Usuario, StringComparison.OrdinalIgnoreCase);
        }
    }
}