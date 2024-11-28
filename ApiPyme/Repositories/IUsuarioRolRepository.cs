using ApiPyme.Models;

namespace ApiPyme.Repositories
{
    public interface IUsuarioRolRepository
    {
        Task<UsuarioRol> GetRol(int id);
        Task<bool> Save(UsuarioRol usuarioRol);
        Task<bool> Update(UsuarioRol usuarioRol);
        Task<bool> Delete(int id);
    }
}
