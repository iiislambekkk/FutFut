using System.Security.Claims;
using AutoMapper;
using FutFut.Common;
using FutFut.Notify.Contracts;
using FutFut.Profile.Service.Dtos;
using FutFut.Profile.Service.Entities;
using FutFut.Profile.Service.Enums;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FutFut.Profile.Service.Controllers;

[ApiController]
[Route("friendship")]
public class FriendShipController(
    IRepository<ProfileEntity> profileRepository,
    IRepository<FriendShipEntity> friendShipRepository,
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    ILogger<ProfileController> logger) : ControllerBase
{
    
    [HttpPost("friendship/request")]
    [Authorize]
    public async Task<ActionResult> RequestFriendship([FromBody] FriendShipRequestDto friendShipRequestDto)
    {
        if (friendShipRequestDto.RequestedUserId == friendShipRequestDto.TargetUserId)
        {
            return BadRequest("Requested userId and targetUserId cannot be same.");
        }

        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";

        if (friendShipRequestDto.RequestedUserId.ToString() != currentUserId)
        {
            return Forbid();
        }

        var currentUserProfile = await profileRepository.GetAsync(p => p.Id == Guid.Parse(currentUserId));
        if (currentUserProfile is null)
        {
            return NotFound($"The profile with userid {currentUserId} not found.");
        }

        var existingFriendship = await friendShipRepository.GetAsync(f =>
            f.RequestedUserId.ToString() == currentUserId || f.RespondedUserId.ToString() == currentUserId);

        if (existingFriendship is not null)
        {
            if (existingFriendship.Status == FriendshipStatusEnum.Accepted)
            {
                return BadRequest(
                    $"The friendship between user with id {friendShipRequestDto.RequestedUserId} and user with id {friendShipRequestDto.TargetUserId}");
            }

            existingFriendship.Status = FriendshipStatusEnum.Pending;
        }
        else
        {
            var newFriendship = new FriendShipEntity()
            {
                RequestedUserId = Guid.Parse(currentUserId),
                RespondedUserId = friendShipRequestDto.TargetUserId
            };

            await friendShipRepository.CreateAsync(newFriendship);
        }

        await publishEndpoint.Publish<SendNotification>(new(
            friendShipRequestDto.TargetUserId,
            $"{currentUserProfile.DisplayName} want to be friend with you.",
            "Cho tam",
            "{}",
            NotificationType.FriendshipRequest)
        );

        return Ok();
    }
}