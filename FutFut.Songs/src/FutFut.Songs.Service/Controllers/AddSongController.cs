using System.Security.Claims;
using Amazon.S3;
using Amazon.S3.Model;
using FutFut.Common;
using FutFut.Songs.Service.Data.Entities;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Grpc;

namespace FutFut.Songs.Service.Controllers;

[ApiController]
[Route("songs")]
public class AddSongController(
    IRepository<SongEntity> songsRepo,
    IRepository<ArtistEntity> artistRepo,
    IAmazonS3 amazonS3) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SongEntity>> GetAllAsync()
    {
        var songs = await songsRepo.GetAllAsync();
        return Ok(songs);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<string>> AddAsync(SongEntity songReq)
    {
        Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId);
        
        var artistEntity = await artistRepo.GetAsync(a => a.Id == userId);

        if (artistEntity == null)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new User.UserClient(channel);

            var user = await client.GetUserAsync(new GetUserRequest() { UserId = userId.ToString() });
            if (user == null) return NotFound("User not found");

            artistEntity = new ArtistEntity() { Id = Guid.Parse(user.UserId), Name = user.Name};
            await artistRepo.CreateAsync(artistEntity);
        }
        
        var song = new SongEntity() {Name = songReq.Name };
        song.ArtistEntities.Add(artistEntity);
        await songsRepo.CreateAsync(song);
        
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "your-bucket",
            Key = $"temp/{Guid.NewGuid()}/audio.mp3",
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(15),
            ContentType = "audio/mpeg"
        };
        
        string presignedUrl = await amazonS3.GetPreSignedURLAsync(request);
        return Ok(presignedUrl);
    }
}