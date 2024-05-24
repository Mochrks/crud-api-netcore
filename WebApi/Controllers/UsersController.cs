using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.Request;
using WebApi.Models;
using WebApi.Data;
using System;
using System.Threading.Tasks;
using WebApi.DTO;
using Microsoft.AspNetCore.Http;
using WebApi.Services;



namespace food_order_be_netcore.Controllers;

[Route("api/user-management/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // POST: user-management/users/sign-up
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequest)
    {
        try
        {
            var response = await _userService.SignUpUser(signUpRequest);
            var responseData = response as dynamic;
            return StatusCode((int)responseData.StatusCode, responseData);

            // return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    // POST: user-management/users/sign-in
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequest)
    {
        try
        {
            var response = await _userService.SignInUser(signInRequest);
            return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

}