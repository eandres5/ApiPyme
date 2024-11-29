using ApiPyme.Context;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiPyme.RepositoriesImpl
{
    public class RolRepositoryImpl : IRolRepository
    {
        private readonly AppDbContext _context;
        public RolRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Rol> GetRol(int id)
        {
            return await _context.Rols.FindAsync(id);
        }

        public async Task<Rol> GetRolByNombre(string nombre)
        {
            return await _context.Rols.FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task<bool> Save(Rol rol)
        {
            try
            {
                await _context.Rols.AddAsync(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> Update(Rol rol)
        {
            throw new NotImplementedException();
        }
    }
}
