﻿using System.Threading.Tasks;
using AutoMapper;
using ChatBox.Dtos;
using ChatBox.Entities.Identity;
using ChatBox.Errors;
using ChatBox.Extensions;
using ChatBox.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatBox.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }
        
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use"}});
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var roleAddResult = await _userManager.AddToRoleAsync(user, "Member");
            
            if (!roleAddResult.Succeeded) return BadRequest("Failed to add to role");

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}
