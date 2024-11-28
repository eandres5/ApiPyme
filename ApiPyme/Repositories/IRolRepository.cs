using ApiPyme.Models;

namespace ApiPyme.Repositories
{
    public interface IRolRepository
    {
        Task<Rol> GetRol(int id);
        Task<bool> Save(Rol rol);
        Task<bool> Update(Rol rol);
        Task<bool> Delete(int id);
    }
}
