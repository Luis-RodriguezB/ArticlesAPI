using ArticlesAPI.DTOs.Auth;
using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Services.Interfaces;
using AutoMapper;
using BlogApi.DTOs.Auth;
using BlogApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArticlesAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository userRepository;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IPersonRepository personRepository;
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;

    public AuthService(
        IUserRepository userRepository,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IPersonRepository personRepository,
        IConfiguration configuration,
        IMapper mapper)
    {
        this.userRepository = userRepository;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.personRepository = personRepository;
        this.configuration = configuration;
        this.mapper = mapper;
    }

    public async Task<AuthResponseDTO> Register(RegisterDTO registerDTO)
    {
        var user = new User { UserName = registerDTO.Username, Email = registerDTO.Email };
        var result = await userManager.CreateAsync(user, registerDTO.Password);

        if (result.Succeeded)
        {
            var person = mapper.Map<Person>(registerDTO);
            person.UserId = user.Id;

            await personRepository.Save(person);
            return await GenerateToken(user);
        } else
        {
            throw new BadRequestException(result.Errors.ElementAt(0).Description);
        }
    }

    public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
    {
        var user = await GetUserByEmail(loginDTO.Email);

        var result = await signInManager.PasswordSignInAsync(
            user.UserName,
            loginDTO.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
        {
            return await GenerateToken(user);
        }

        throw new BadRequestException("Invalid login attempt");
    }

    public async Task<AuthResponseDTO> RenewToken(string email)
    {
        var user = await GetUserByEmail(email);

        return await GenerateToken(user);
    }

    public async Task<ResponseDTO> ToggleAdmin(string email)
    {
        User user = await GetUserByEmail(email);
        var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

        IdentityResult result = isAdmin
                ? await userManager.RemoveFromRoleAsync(user, "Admin")
                : await userManager.AddToRoleAsync(user, "Admin");

        if (result.Succeeded)
        {
            var action = isAdmin ? "revoked" : "granted";
            return new ResponseDTO { Message = $"Role 'Admin' has been {action} successfully" };
        }

        throw new BadRequestException(result.Errors.ElementAt(0).Description);
    }

    private async Task<User> GetUserByEmail(string email)
    {
        return await userManager.FindByEmailAsync(email)
            ?? throw new NotFoundException($"Not exist a user with this email {email}");
    }

    public async Task<List<UserDTO>> Get(UserFilter userFilter)
    {
        var users = await userRepository.GetAll(userFilter);
        return mapper.Map<List<UserDTO>>(users);
    }

    public async Task<UserDTO> GetById(string id)
    {
        var user = await userRepository.GetById(id);
        return mapper.Map<UserDTO>(user);
    }

    public async Task<ResponseDTO> Delete(string id)
    {
        var user = await userManager.FindByIdAsync(id) 
            ?? throw new NotFoundException($"Not exist a user with the id {id}");

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return new ResponseDTO { Message = $"The user with the id {id} has been eliminated successfully" };
        }

        throw new BadRequestException(result.Errors.ElementAt(0).Description);
    }

    private async Task<AuthResponseDTO> GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(1);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
        };
        var claimsDb = await userManager.GetClaimsAsync(user);
        claims.AddRange(claimsDb);

        var userRoles = userManager.GetRolesAsync(user).Result;
        if (userRoles.Count > 0)
        {
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        JwtSecurityToken token = new(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new AuthResponseDTO()
        {
            Email = user.Email,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }
}
