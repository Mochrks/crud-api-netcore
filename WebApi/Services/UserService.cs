using System;
using System.Threading.Tasks;
using WebApi.DTO.Request;
using WebApi.DTO.Response;
using WebApi.Models;
using WebApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;


namespace WebApi.Services
{
    public class UserService
    {
        private readonly WebApiDBContext _context;
        private readonly IConfiguration _configuration;

        private readonly ILogger<UserService> _logger;

        public UserService(WebApiDBContext context, IConfiguration configuration, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<object> SignUpUser(SignUpRequestDTO signUpRequest)
        {
            try
            {
                if (signUpRequest == null)
                {
                    throw new ArgumentNullException(nameof(signUpRequest), "kolom tidak boleh berisi null.");
                }

                if (string.IsNullOrWhiteSpace(signUpRequest.Username))
                {
                    throw new ArgumentException("Username tidak boleh kosong !", nameof(signUpRequest.Username));
                }

                if (string.IsNullOrWhiteSpace(signUpRequest.Fullname))
                {
                    throw new ArgumentException("Fullname tidak boleh kosong !", nameof(signUpRequest.Fullname));
                }

                if (string.IsNullOrWhiteSpace(signUpRequest.Password))
                {
                    throw new ArgumentException("Password tidak boleh kosong !");
                }

                if (!Regex.IsMatch(signUpRequest.Password, @"^(?=.*[a-zA-Z])(?=.*\d).{6,50}$"))
                {
                    throw new ArgumentException("Password minimal 6 karakter dan harus mengandung setidaknya satu huruf dan satu angka");
                }

                // Check username apakah sudah terpakai
                if (await _context.Users.AnyAsync(u => u.Username == signUpRequest.Username))
                {
                    throw new ArgumentException("Username telah digunakan oleh pengguna yang telah mendaftar sebelumnya.", nameof(signUpRequest.Username));
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpRequest.Password);

                var user = new User
                {
                    Username = signUpRequest.Username,
                    Fullname = signUpRequest.Fullname,
                    Password = hashedPassword,

                    CreatedTime = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Mendaftarkan akun baru...");

                string userRegisteredMessage = "Berhasil Mendaftarkan Akun";
                var response = new
                {
                    Message = userRegisteredMessage,
                    StatusCode = StatusCodes.Status201Created,
                    Status = "Success"

                };

                _logger.LogInformation($"Akun {signUpRequest.Username} berhasil didaftarkan.");
                return response;
            }
            catch (ArgumentException ex)
            {
                var response = new
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Bad Request"
                };
                _logger.LogError(ex, "Gagal mendaftarkan akun: {ErrorMessage}", ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Message = "Terjadi kesalahan server. Silakan coba kembali.",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Internal Server Error"
                };
                _logger.LogError(ex, "Terjadi kesalahan saat mendaftarkan akun.");
                return response;
            }
        }


        public async Task<object> SignInUser(SignInRequestDTO signInRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == signInRequest.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(signInRequest.Password, user.Password))
                {
                    throw new ArgumentException("Username atau Password salah.");
                }

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var subject = _configuration["Jwt:Subject"];

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("fullname", user.Fullname),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("userId", user.UserId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var responseData = new
                {
                    id = user.UserId,
                    token = tokenString,
                    type = "Bearer",
                    username = user.Username,
                    role = "User"
                };

                var response = new
                {
                    data = responseData,
                    message = $"User {user.Username} Berhasil login !!",
                    statusCode = StatusCodes.Status200OK,
                    status = "OK"
                };
                _logger.LogInformation($"User {user.Username} berhasil login.");
                return response;
            }
            catch (ArgumentException ex)
            {

                _logger.LogError(ex, "ArgumentException: {Message}", ex.Message);
                var response = new
                {
                    message = ex.Message,
                    statusCode = StatusCodes.Status400BadRequest,
                    status = "Bad Request"
                };

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat login: {Message}", ex.Message);
                var response = new
                {
                    message = "Terjadi kesalahan server. Silakan coba kembali.",
                    statusCode = StatusCodes.Status500InternalServerError,
                    status = "Internal Server Error"
                };

                return response;
            }
        }


    }
}
