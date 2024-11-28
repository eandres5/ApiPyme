using ApiPyme.Context;
using ApiPyme.Models;
using ApiPyme.Repositories;

namespace ApiPyme.RepositoriesImpl
{
    public class UsuarioRolRepositoryImpl: IUsuarioRolRepository
    {
        private readonly AppDbContext _context;
        public UsuarioRolRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UsuarioRol> GetRol(int id)
        {
            return await _context.UsuarioRol.FindAsync(id);
        }

        public async Task<bool> Save(UsuarioRol usuarioRol)
        {
            try
            {
                await _context.UsuarioRol.AddAsync(usuarioRol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> Update(UsuarioRol usuarioRol)
        {
            throw new NotImplementedException();
        }
    }
}
