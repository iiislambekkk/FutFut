using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FutFut.Common;
using FutFut.Common.AWS3;
using FutFut.Profile.Service.Dtos;
using FutFut.Profile.Service.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FutFut.Profile.Service.Controllers;

[ApiController]
[Route("profile")]
public class ProfileController(
    IRepository<ProfileEntity> profileRepository,
    IRepository<PlayedHistoryEntity> playedHistoryRepository,
    IRepository<AboutPhotoEntity> aboutPhotosRepository,
    IRepository<FriendShipEntity> friendShipRepository,
    IMapper mapper,
    IS3StorageService storageService,
    IConfiguration configuration,
    ILogger<ProfileController> logger) : ControllerBase
{
    [HttpGet("get/view/{id}")]
    public async Task<ActionResult<ProfileDto>> GetByIdNotPersonalDataAsync(Guid id)
    {
        var profileEntity = await profileRepository.GetAsync(p => p.Id == id);
        if (profileEntity is null) return NotFound($"Profile with id {id} not found.");
        
        var aboutPhotos = await aboutPhotosRepository.GetAllAsync(p => p.Id == id);
        
        profileEntity.AboutPhotos = aboutPhotos.ToList();

        if (!profileEntity.IsPrivate)
        {
            if (profileEntity.ShowFriends)
            {
                var friendShipsEntities =
                    await friendShipRepository.GetAllAsync(u => u.RequestedUserId == id || u.RespondedUserId == id);
                profileEntity.FriendShips = friendShipsEntities.ToList();
            }
        }
        
        var profileDto = mapper.Map<ProfileDto>(profileEntity);
        
        return Ok(profileDto);
    }
    
    [HttpGet("get/detailed/{id}")]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> GetByIdFullInfoAsync(Guid id)
    {
        var profileEntity = await profileRepository.GetAsync(p => p.Id == id);
       
        
        if (profileEntity is null) return NotFound($"Profile with id {id} not found.");
        
        var playedHistory = await playedHistoryRepository.GetAllAsync(p => p.ProfileId == id);
        var aboutPhotos = await aboutPhotosRepository.GetAllAsync(p => p.Id == id);
        
        profileEntity.PlayedHistory = playedHistory.ToList();
        profileEntity.AboutPhotos = aboutPhotos.ToList();
        
        var friendShipsEntities =
            await friendShipRepository.GetAllAsync(u => u.RequestedUserId == id || u.RespondedUserId == id);
        profileEntity.FriendShips = friendShipsEntities.ToList();
        
        var profileDto = mapper.Map<ProfileDto>(profileEntity);
        
        return Ok(profileDto);
    }
    
    [HttpGet("get/all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ProfileEntity>>> GetAllAsync()
    {
        var result = await profileRepository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> CreateAsync(CreateProfileDto entity)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (currentUserId is null)
        {
            return BadRequest("Cant find id of user.");
        }

        var existedUser = await profileRepository.GetAsync(p => p.Id == Guid.Parse(currentUserId));

        if (existedUser is not null)
        {
            return BadRequest("Profile for user that requested action already exists.");
        }
        
        var profile = mapper.Map<ProfileEntity>(entity);
        profile.Id = Guid.Parse(currentUserId);
        
        await profileRepository.CreateAsync(profile);
        
        return CreatedAtAction(nameof(GetByIdFullInfoAsync), new { id = profile.Id }, profile);
    }

    [HttpPut("avatar")]
    [Authorize]
    public async Task<ActionResult<string>> ChangeAvatarAsync(IFormFile file, Guid userId)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";

        if (!(userId.ToString() == currentUserId || User.IsInRole("Admin")))
        {
            logger.LogInformation("Access denied for user {id}", currentUserId);
            return Forbid("Access denied");
        }
        
        var profile = await profileRepository.GetAsync(u => u.Id == userId);
        
        if (profile == null) return NotFound("Profile not found");
        
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл не предоставлен или пуст.");

        var contentType = file.ContentType;
        if (!contentType.StartsWith("image/"))
            throw new InvalidOperationException("Файл должен быть изображением.");
        
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"avatars/{Guid.NewGuid()}{extension}";

        await using var stream = file.OpenReadStream();

        await storageService.UploadAsync(
            stream,
            fileName,
            contentType
        );
        
        var avatarUrl = $"{fileName}";
        
        profile.Avatar = avatarUrl;
        
        await profileRepository.UpdateAsync(profile);

        return avatarUrl;
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<ActionResult> UpdateProfileInfo([FromBody] UpdateProfileDto updateProfileDto)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";

        if (currentUserId != updateProfileDto.Id.ToString() && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var profile = await profileRepository.GetAsync(p => p.Id == updateProfileDto.Id);

        if (profile == null)
        {
            return NotFound($"User with id {updateProfileDto.Id} not found.");
        }

        var updatedProfileEntity = mapper.Map(updateProfileDto, profile);

        await profileRepository.UpdateAsync(updatedProfileEntity);

        return Ok();
    }
}