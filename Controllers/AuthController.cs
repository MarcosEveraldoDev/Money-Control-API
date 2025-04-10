using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    [DisableCors]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, TokenService tokenService, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _context = context;

        }

        public static async Task<bool> CpfExisteAsync(string cpf, AppDbContext context)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray()); // Remove caracteres não numéricos

            return await context.Users.AnyAsync(u => u.CPF == cpf);
        }


        /// <summary>
        /// Registra um novo usuário previo com apenas email e senha.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {

            var existingUsers = await _userManager.Users
                .Where(u => u.Email == dto.Email)
                .ToListAsync();

            if (existingUsers.Count > 1)
            {
                return BadRequest("Erro: Existe mais de um usuário cadastrado com este e-mail.");
            }
            if (existingUsers.Count == 1)
            {
                return BadRequest("Este e-mail já está cadastrado.");
            }
            if (CpfExisteAsync(dto.CPF, _context).Result)
            {
                return BadRequest("Este CPF já está cadastrado.");
            }

            var user = new User
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                CPF = dto.CPF,
                DateBirth = dto.DateBirth

            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var ErrorsObject = new
                {
                    Sucess = false,
                    Errors = result.Errors,
                };

                return Json(ErrorsObject);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


            var callbackUrl = Url.Action(
                "Confirmação de Email",
                "Auth",
                new { userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme
            );

            await _emailService.SendEmailAsync(dto.Email, "Confirme seu email", $"Por Favor confirme seu email clicando aqui: {callbackUrl}");

            var responseObject = new
            {
                Sucess = true,
                Message = "Usuario registrado com sucesso. Confira seu e-mail para confirmar a conta.",
                id = user.Id,
                TokenConfirmEmail = token,
            };

            return Json(responseObject);

        }


        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Usuario não encontrado");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (result.IsLockedOut)
                return Unauthorized("Conta Bloqueada");

            if (result.IsNotAllowed)
                return Unauthorized("Usuario precisa confirmar o email");

            if (result.RequiresTwoFactor)
                return Unauthorized("Precisa realizar a Autenticação de dois fatores");

            if (!result.Succeeded)
                return Unauthorized("Senha Invalida");

            var roles = await _userManager.GetRolesAsync(user);


            var tokenJWT = TokenService.Generate(user, roles);

            return Json(tokenJWT);

        }

        /// <summary>
        /// Gera um Refresh Token a partir do ID do Usuario
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return BadRequest("Usuario não Encontrado.");
            }

            var refreshToken = TokenService.Refresh(user, new List<string> { "User" });

            var response = new
            {
                RefreshToken = refreshToken
            };

            return Ok(response);
        }


        /// <summary>
        /// Desloga o usuário.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize(Roles = "User , Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Usuário deslogado com sucesso");
        }

        /// <summary>
        /// Confirma o e-mail do usuário.
        /// </summary>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Token de confirmação inválido.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok("E-mail confirmado com sucesso.");
            }

            return BadRequest("Erro ao confirmar o e-mail.");
        }


    }
}
