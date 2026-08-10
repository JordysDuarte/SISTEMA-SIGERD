using SIGERD.Interfaces.IRespositories.Inventario;
using SIGERD.Interfaces.IServices.Inventario;
using SIGERD.Models.Inventario;

namespace SIGERD.Services.Inventario
{
    public class ArticuloService : IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository;

        public ArticuloService(IArticuloRepository articuloRepository)
        {
            _articuloRepository = articuloRepository;
        }

        public async Task<IEnumerable<Articulo>> ObtenerTodosAsync()
        {
            return await _articuloRepository.ObtenerTodosAsync();
        }

        public async Task<Articulo?> ObtenerPorIdAsync(int idArticulo)
        {
            if (idArticulo <= 0)
            {
                return null;
            }

            return await _articuloRepository.ObtenerPorIdAsync(idArticulo);
        }

        public async Task CrearAsync(Articulo articulo)
        {
            if (articulo is null)
            {
                throw new InvalidOperationException("La información del artículo no es válida.");
            }

            articulo.nombreArticulo = articulo.nombreArticulo?.Trim() ?? string.Empty;
            articulo.descripcion = articulo.descripcion?.Trim();

            if (string.IsNullOrWhiteSpace(articulo.nombreArticulo))
            {
                throw new InvalidOperationException("Debe ingresar el nombre del artículo.");
            }

            if (articulo.idCategoriaArticulo <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar una categoría.");
            }

            bool categoriaActiva = await _articuloRepository.ExisteCategoriaActivaAsync(
                articulo.idCategoriaArticulo
            );

            if (!categoriaActiva)
            {
                throw new InvalidOperationException("La categoría seleccionada no existe o está inactiva.");
            }

            bool existeNombre = await _articuloRepository.ExisteNombreAsync(
                articulo.nombreArticulo
            );

            if (existeNombre)
            {
                throw new InvalidOperationException("Ya existe un artículo con ese nombre.");
            }

            articulo.estado = true;

            await _articuloRepository.AgregarAsync(articulo);
            await _articuloRepository.GuardarAsync();
        }

        public async Task ActualizarAsync(Articulo articulo)
        {
            if (articulo is null)
            {
                throw new InvalidOperationException("La información del artículo no es válida.");
            }

            if (articulo.idArticulo <= 0)
            {
                throw new InvalidOperationException("El identificador del artículo no es válido.");
            }

            var articuloActual = await _articuloRepository.ObtenerPorIdAsync(articulo.idArticulo);

            if (articuloActual is null)
            {
                throw new InvalidOperationException("El artículo solicitado no existe.");
            }

            articulo.nombreArticulo = articulo.nombreArticulo?.Trim() ?? string.Empty;
            articulo.descripcion = articulo.descripcion?.Trim();

            if (string.IsNullOrWhiteSpace(articulo.nombreArticulo))
            {
                throw new InvalidOperationException("Debe ingresar el nombre del artículo.");
            }

            if (articulo.idCategoriaArticulo <= 0)
            {
                throw new InvalidOperationException("Debe seleccionar una categoría.");
            }

            bool categoriaActiva = await _articuloRepository.ExisteCategoriaActivaAsync(
                articulo.idCategoriaArticulo
            );

            if (!categoriaActiva)
            {
                throw new InvalidOperationException("La categoría seleccionada no existe o está inactiva.");
            }

            bool existeNombre = await _articuloRepository.ExisteNombreAsync(
                articulo.nombreArticulo,
                articulo.idArticulo
            );

            if (existeNombre)
            {
                throw new InvalidOperationException("Ya existe otro artículo con ese nombre.");
            }

            articuloActual.nombreArticulo = articulo.nombreArticulo;
            articuloActual.descripcion = articulo.descripcion;
            articuloActual.idCategoriaArticulo = articulo.idCategoriaArticulo;
            articuloActual.estado = articulo.estado;

            _articuloRepository.Actualizar(articuloActual);
            await _articuloRepository.GuardarAsync();
        }

        public async Task CambiarEstadoAsync(int idArticulo, bool nuevoEstado)
        {
            if (idArticulo <= 0)
            {
                throw new InvalidOperationException("El identificador del artículo no es válido.");
            }

            var articulo = await _articuloRepository.ObtenerPorIdAsync(idArticulo);

            if (articulo is null)
            {
                throw new InvalidOperationException("El artículo solicitado no existe.");
            }

            articulo.estado = nuevoEstado;

            _articuloRepository.Actualizar(articulo);
            await _articuloRepository.GuardarAsync();
        }
    }
}