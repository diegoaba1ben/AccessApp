using Microsoft.EntityFrameworkCore;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Exceptions;

namespace AccessAppUser.Infrastructure.Repositories.Base
{
    /// <summary>
    /// Repositorio genérico que implementa las operaciones CRUD básicas
    /// </summary>
    /// <typeparam name="T">Entidad que extiende la clase</typeparam>
    public class BaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor del repositorio genérico que recibe el contexto de base de datos
        /// </summary>
        /// <param name="context">Instancia de <see cref="AppDbContext"></param>
        public BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }
        /// <summary>
        /// Método para obtener una entidad por su Id.
        /// </summary>
        /// <returns>Entidad encontrada o null si no existe</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Método para obtener todas las entidades
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Agrega una nueva entidad al contexto y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Actualiza una entidad en el contexto y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Elimina una entidad del contexto y guarda los cambios.
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"Invalid id for entity type {typeof(T).Name}");
            }

            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Verifica si una entidad con el Id especificado existe en el contexto.
        /// </summary>
        /// <param name="id">Id de la entidad a buscar</param>
        /// <returns>True si la entidad existe, False si no</returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }
    }
}