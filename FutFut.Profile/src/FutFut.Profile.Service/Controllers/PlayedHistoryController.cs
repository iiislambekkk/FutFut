using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FutFut.Common;
using FutFut.Profile.Service.Dtos;
using FutFut.Profile.Service.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FutFut.Profile.Service.Controllers;

[ApiController]
[Route("playhistory")]
public class PlayedHistoryController(
    IRepository<PlayedHistoryEntity> playedHistoryRepository,
    IMapper mapper
    ) : ControllerBase
{
    [HttpGet("all/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<PlayedHistoryDto>>> GetAllAsync(Guid profileId)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!(currentUserId == profileId.ToString() || User.IsInRole("Admin")))
        {
            return Forbid();
        }

        var playedHistoryEntities = (await playedHistoryRepository.GetAllAsync(p => p.ProfileId == profileId)).ToList();
        var playedHistoryDtos = mapper.Map<List<PlayedHistoryDto>>(playedHistoryEntities);

        return playedHistoryDtos;
    }
    

    [HttpPost]
    public async Task<ActionResult<PlayedHistoryDto>> AddRecordToPlayedHistory(
        [FromBody] CreatePlayedHistoryDto createPlayedHistoryDto)
    {
        var playedHistoryEntity = mapper.Map<PlayedHistoryEntity>(createPlayedHistoryDto);
        await playedHistoryRepository.CreateAsync(playedHistoryEntity);

        return Ok();
    }
}