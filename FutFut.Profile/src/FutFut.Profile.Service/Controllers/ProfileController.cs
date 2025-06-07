using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FutFut.Common;
using FutFut.Common.AWS3;
using FutFut.Common.Settings;
using FutFut.Profile.Service.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FutFut.Profile.Service.Controllers;

[ApiController]
[Route("profile")]
public class ProfileController(IRepository<ProfileEntity> repository, IMapper mapper, IS3StorageService storageService, IConfiguration configuration) : ControllerBase
{
    private readonly AWS3Settings _aws3Settings = configuration.GetSection(nameof(AWS3Settings)).Get<AWS3Settings>()!;
    
    [HttpGet("get/{id}")]
    public async Task<ActionResult<IEnumerable<ProfileEntity>>> GetByIdAsync(Guid id)
    {
        var result = await repository.GetAsync(p => p.Id == id);
        return Ok(result);
    }
    
    [HttpGet("get/all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ProfileEntity>>> GetAllAsync()
    {
        var result = await repository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
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
    [Authorize]
    public async Task<ActionResult<string>> ChangeAvatarAsync(IFormFile file, Guid userId)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";

        if (!(userId.ToString() == currentUserId || User.IsInRole("Admin")))
        {
            return Forbid();
        }
        
        var profile = await repository.GetAsync(u => u.Id == userId);
        
        if (profile == null) return NotFound();
        
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл не предоставлен или пуст.");

        var contentType = file.ContentType;
        if (!contentType.StartsWith("image/"))
            throw new InvalidOperationException("Файл должен быть изображением.");
        
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"avatars/{Guid.NewGuid()}{extension}";

        using var stream = file.OpenReadStream();

        await storageService.UploadAsync(
            stream,
            fileName,
            contentType
        );
        
        var avatarUrl = $"{_aws3Settings.Endpoint}/{_aws3Settings.Endpoint}/{fileName}";
        
        profile.Avatar = avatarUrl;
        
        await repository.UpdateAsync(profile);

        return avatarUrl;
    } 
}