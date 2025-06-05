using Duende.IdentityServer;
using FutFut.Identity.Service.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Identity.Service.Controllers;

[ApiController]
[Route("users")]
[Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName, Roles = "Admin")]
public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpGet("get/all")]
    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await userManager.Users.ToListAsync();
    }
    
    [HttpGet("get/id/{id:guid}")]
    public async Task<ApplicationUser?> GetUser(Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }
}