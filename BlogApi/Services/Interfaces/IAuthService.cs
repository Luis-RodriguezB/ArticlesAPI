using ArticlesAPI.DTOs.Auth;
using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using BlogApi.DTOs.Auth;

namespace ArticlesAPI.Services.Interfaces;
public interface IAuthService
{
    Task<AuthResponseDTO> Register(RegisterDTO registerDTO);
    Task<AuthResponseDTO> Login(LoginDTO loginDTO);
    Task<AuthResponseDTO> RenewToken(string email);
    Task<ResponseDTO> ToggleAdmin(string email);
    Task<List<UserDTO>> Get(UserFilter userFilter);
    Task<UserDTO> GetById(string id);
    Task<ResponseDTO> Delete(string id);
}
