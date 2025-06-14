using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FutFut.Common;
using FutFut.Common.AWS3;
using FutFut.Profile.Service.Dtos;
using FutFut.Profile.Service.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FutFut.Profile.Service.Controllers;

[ApiController]
[Route("aboutphoto")]
public class AboutPhotosController(
    IRepository<AboutPhotoEntity> aboutPhotosRepository,
    IRepository<ProfileEntity> profileRepository,
    IMapper mapper,
    IS3StorageService storageService
    ): ControllerBase
{
    [HttpGet("{profileId:guid}")]
    public async Task<ActionResult<List<AboutPhotoDto>>> GetAllByUserId(Guid profileId)
    {
        var aboutPhotos = await aboutPhotosRepository.GetAllAsync(p => p.ProfileId == profileId);
        return mapper.Map<List<AboutPhotoEntity>, List<AboutPhotoDto>>(aboutPhotos.ToList());
    }

    [HttpPost]
    public async Task<ActionResult> AddPhoto(IFormFile photo, Guid userId)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";
        if (!(currentUserId == userId.ToString() || User.IsInRole("Admin")))
        {
            return Forbid();
        }
        
        var profile = await profileRepository.GetAsync(u => u.Id == userId);
        if (profile == null) return NotFound("Profile not found");
        
        var contentType = photo.ContentType;
        if (!contentType.StartsWith("image/"))
            throw new InvalidOperationException("File must be image formatted.");
        
        var extension = Path.GetExtension(photo.FileName);
        var fileName = $"aboutPhotos/{Guid.NewGuid()}{extension}";

        await using var stream = photo.OpenReadStream();

        await storageService.UploadAsync(
            stream,
            fileName,
            contentType
        );
        
        var photoKeyInObjectStorage = $"{fileName}";

        var newAboutPhotoEntity = new AboutPhotoEntity()
        {
            ProfileId = userId,
            Path = photoKeyInObjectStorage
        };

        await aboutPhotosRepository.CreateAsync(newAboutPhotoEntity);

        return Ok();
    }


    [HttpDelete]
    public async Task<ActionResult> DeletePhoto([FromBody] DeleteAboutPhotoDto deleteAboutPhotoDto)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";
        
        if (!(currentUserId == deleteAboutPhotoDto.UserId.ToString() || User.IsInRole("Admin")))
        {
            return Forbid();
        }

        var existingPhoto = await aboutPhotosRepository.GetAsync(p => p.Id == deleteAboutPhotoDto.AboutPhotoId);

        if (existingPhoto == null) return NoContent();
        
        var isDeleted = await storageService.DeleteAsync(existingPhoto.Path);

        if (isDeleted)
        {
            await aboutPhotosRepository.DeleteAsync(deleteAboutPhotoDto.AboutPhotoId);

            return NoContent();
        }

        return BadRequest();
    }
}