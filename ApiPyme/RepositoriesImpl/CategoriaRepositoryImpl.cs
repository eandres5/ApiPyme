using ApiPyme.Context;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;

namespace ApiPyme.RepositoriesImpl
{
    public class CategoriaRepositoryImpl: ICategoriaRepository
    {
        // conexion a base de datos
        private readonly AppDbContext _context;


        public CategoriaRepositoryImpl(AppDbContext context) {
            _context = context;
        }

        public Task<bool> DeleteCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Categoria>> GetAllCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria> GetCategoria(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<bool> SaveCategoria(Categoria categoria)
        {
            try
            {
                await _context.Categorias.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Manejo de excepciones
                return false;
            }
        }

        public async Task<bool> UpdateCategoria(Categoria categoria)
        {
            try
            {
                var existingCategoria = await _context.Categorias.FindAsync(categoria.IdCategoria);
                if (existingCategoria != null)
                {
                    existingCategoria.Nombre = categoria.Nombre;
                    existingCategoria.CreatedAt = categoria.CreatedAt;
                    existingCategoria.UpdateAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                // Manejo de excepciones
                return false;
            }
        }
    }
}
