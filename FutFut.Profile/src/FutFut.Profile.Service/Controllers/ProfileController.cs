using AutoMapper;
using FutFut.Common;
using FutFut.Profile.Service.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FutFut.Profile.Service.Controllers;

[ApiController]
[Route("profile")]
public class ProfileController(IRepository<ProfileEntity> repository, IMapper mapper) : ControllerBase
{
    [HttpGet("get/{id}")]
    public async Task<ActionResult<IEnumerable<ProfileEntity>>> GetByIdAsync(Guid id)
    {
        var result = await repository.GetAsync(p => p.Id == id);
        return Ok(result);
    }
    
    [HttpGet("get/all")]
    public async Task<ActionResult<IEnumerable<ProfileEntity>>> GetAllAsync()
    {
        var result = await repository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileDto>> CreateAsync(CreateProfileDto entity)
    {
        var profile = mapper.Map<ProfileEntity>(entity);
        
        await repository.CreateAsync(profile);
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(profile.Id);
        Console.ResetColor();
        
        return CreatedAtAction(nameof(GetByIdAsync), new { id = profile.Id }, profile);
    }

    [HttpPost("avatar")]
    public async Task ChangeAvatarAsync()
    {
        throw new NotImplementedException();
    } 
}