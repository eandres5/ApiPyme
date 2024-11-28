using ApiPyme.Models;

namespace ApiPyme.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetAllCategorias();
        Task<Categoria> GetCategoria(int id);
        Task<bool> SaveCategoria(Categoria categoria);
        Task<bool> UpdateCategoria(Categoria categoria);
        Task<bool> DeleteCategoria(int id);
    }
}
