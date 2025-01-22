using ApiPyme.Dto;
using ApiPyme.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiPyme.Repositories
{
    public interface IUsuarioRepository
    {
        Task<PagedResult<UsuarioDto>> GetAllUsuarios(int page, int size, string search);
        Task<PagedResult<UsuarioDto>> GetAllUsuariosProveedor(int page, int size, string search);
        Task<UsuarioDto> GetUsuarioByIdentificaion(string identificacion);
        Task<Usuario> GetUsuario(int id);
        Task<bool> SaveUsuario(UsuarioDto usuarioDto);
        Task<bool> UpdateUsuario(UsuarioDto usuario);
        Task<bool> DeleteUsuario(UsuarioDto usuarioDto);
        Task<Usuario> GetLogin(LoginRequestDto login);
        Task<PagedResult<UsuarioDto>> GetAllUsuariosClientes(int page, int size, string search);
        Task<bool> RecoveryPassword(EmailDto correo);
        Task<bool> UpdatePassword(RecuperarPasswordDTO recuperarPasswordDTO);
    }
}
