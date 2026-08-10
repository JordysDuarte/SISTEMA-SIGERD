using SIGERD.Interfaces.IRespositories.Inventario;
using SIGERD.Interfaces.IServices.Inventario;
using SIGERD.Models.Inventario;

namespace SIGERD.Services.Inventario
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodosAsync()
        {
            return await _categoriaRepository.ObtenerTodosAsync();
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                return null;
            }

            return await _categoriaRepository.ObtenerPorIdAsync(idCategoria);
        }

        public async Task CrearAsync(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new InvalidOperationException("La información de la categoría no es válida.");
            }

            categoria.nombreCategoria = categoria.nombreCategoria?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(categoria.nombreCategoria))
            {
                throw new InvalidOperationException("Debe ingresar el nombre de la categoría.");
            }

            bool existeNombre = await _categoriaRepository.ExisteNombreAsync(categoria.nombreCategoria);

            if (existeNombre)
            {
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");
            }

            categoria.estado = true;

            await _categoriaRepository.AgregarAsync(categoria);
            await _categoriaRepository.GuardarAsync();
        }

        public async Task ActualizarAsync(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new InvalidOperationException("La información de la categoría no es válida.");
            }

            if (categoria.idCategoria <= 0)
            {
                throw new InvalidOperationException("El identificador de la categoría no es válido.");
            }

            var categoriaActual = await _categoriaRepository.ObtenerPorIdAsync(categoria.idCategoria);

            if (categoriaActual is null)
            {
                throw new InvalidOperationException("La categoría solicitada no existe.");
            }

            categoria.nombreCategoria = categoria.nombreCategoria?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(categoria.nombreCategoria))
            {
                throw new InvalidOperationException("Debe ingresar el nombre de la categoría.");
            }

            bool existeNombre = await _categoriaRepository.ExisteNombreAsync(
                categoria.nombreCategoria,
                categoria.idCategoria
            );

            if (existeNombre)
            {
                throw new InvalidOperationException("Ya existe otra categoría con ese nombre.");
            }

            categoriaActual.nombreCategoria = categoria.nombreCategoria;
            categoriaActual.estado = categoria.estado;

            _categoriaRepository.Actualizar(categoriaActual);
            await _categoriaRepository.GuardarAsync();
        }

        public async Task CambiarEstadoAsync(int idCategoria, bool nuevoEstado)
        {
            if (idCategoria <= 0)
            {
                throw new InvalidOperationException("El identificador de la categoría no es válido.");
            }

            var categoria = await _categoriaRepository.ObtenerPorIdAsync(idCategoria);

            if (categoria is null)
            {
                throw new InvalidOperationException("La categoría solicitada no existe.");
            }

            categoria.estado = nuevoEstado;

            _categoriaRepository.Actualizar(categoria);
            await _categoriaRepository.GuardarAsync();
        }
    }
}