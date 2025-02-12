using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones CRUD básicas
    /// </summary>
    /// <typeparam name="T">Entidad sobre la que opera el repositorio</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Método para obtener una entidad por su Id.
        /// </summary>
        /// <returns>Entidad encontrada o null si no existe</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene todas las entidades del tipo especificado
        /// </summary>
        /// <returns>Lista de entidades disponibles</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Agrega una nueva entidad al contexto y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad en el contexto y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad del contexto y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Verifica si una entidad con el Id especificado existe en la base de datos.
        /// </summary>
        /// <param name="id">Id de la entidad a buscar</param>
        /// <returns>True si la entidad existe, False si no</returns>
        Task<bool> ExistsAsync(Guid id);

    }
}