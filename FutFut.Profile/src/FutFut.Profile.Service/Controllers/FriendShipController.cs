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
    IRepository<FriendShipRequestEntity> friendShipRequestRepository,
    IRepository<FriendShipEntity> friendShipRepository,
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    ILogger<ProfileController> logger) : ControllerBase
{
    
    [HttpPost("request")]
    [Authorize]
    public async Task<ActionResult> RequestFriendship([FromBody] FriendShipRequestDto friendShipRequestDto)
    {
        if (friendShipRequestDto.FromUserId == friendShipRequestDto.ToUserId)
        {
            return BadRequest("Requested userId and targetUserId cannot be same.");
        }

        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";

        if (friendShipRequestDto.FromUserId.ToString() != currentUserId)
        {
            return Forbid();
        }

        var currentUserProfile = await profileRepository.GetAsync(p => p.Id == Guid.Parse(currentUserId));
        if (currentUserProfile is null)
        {
            return NotFound($"The profile with userid {currentUserId} not found.");
        }
        
        /*var targetUserProfile = await profileRepository.GetAsync(p => p.Id == friendShipRequestDto.ToUserId);
        if (targetUserProfile is null)
        {
            return NotFound($"The profile with userid {friendShipRequestDto.ToUserId} not found.");
        }*/

        var existingFriendshipRequests = await friendShipRequestRepository.GetAllAsync(f =>
            f.FromUserId.ToString() == currentUserId || f.ToUserId.ToString() == currentUserId
            );

        var fromId = friendShipRequestDto.FromUserId;
        var toId = friendShipRequestDto.ToUserId;

        var isAlreadyFriends = await friendShipRepository.GetAsync(f =>
            (f.FriendAId == fromId && f.FriendBId == toId) ||
            (f.FriendAId == toId && f.FriendBId == fromId));
        
        if (isAlreadyFriends is not null)
        {
            return BadRequest(
                $"The friendship between user with id {friendShipRequestDto.FromUserId} and user with id {friendShipRequestDto.ToUserId} already exists.");
        }
        
        if (existingFriendshipRequests.Any(f => f.Status == FriendshipStatusEnum.Pending))
        {
            return BadRequest(
                $"The friendship request between user with id {friendShipRequestDto.FromUserId} and user with id {friendShipRequestDto.ToUserId} already exists.");
        }
        
        var newFriendship = new FriendShipRequestEntity()
        {
            FromUserId = Guid.Parse(currentUserId),
            ToUserId = friendShipRequestDto.ToUserId
        };

        await friendShipRequestRepository.CreateAsync(newFriendship);
        
        await publishEndpoint.Publish<SendNotification>(new(
            friendShipRequestDto.ToUserId,
            $"{currentUserProfile.DisplayName} wants to be your friend.",
            "",
            "{}",
            NotificationType.FriendshipRequest)
        );

        return Ok();
    }

    [HttpGet("get/{profileId}")]
    [Authorize]
    public async Task<ActionResult<FriendDto>> GetAllFriends(Guid profileId)
    {
        var friends = await friendShipRepository.GetAllAsync(f => f.FriendAId == profileId || f.FriendBId == profileId);
        
        var friendsIds = friends.Select(f => f.FriendAId == profileId ? f.FriendBId : f.FriendAId);
        var friendsProfiles = await profileRepository.GetAllAsync(p => friendsIds.Contains(p.Id));

        List<FriendDto> friendsDto = new();
        foreach (var friend in friendsProfiles)
        {
            var friendship = friends.FirstOrDefault(f => f.FriendAId == friend.Id || f.FriendBId == friend.Id);
            friendsDto.Add(new FriendDto(friendship is not null ? friendship.CreatedAt : DateTimeOffset.MinValue, mapper.Map<ProfileDto>(friend)));
        }
        
        return Ok(friendsDto);
    }

    [HttpPost("response")]
    [Authorize]
    public async Task<ActionResult> AcceptFriendship([FromBody] FriendshipResponseDto friendshipResponseDto)
    {
        if (!Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var currentUserId))
        {
            return Forbid();
        }
        
        var currentUserProfile = await profileRepository.GetAsync(p => p.Id == currentUserId);
        if (currentUserProfile is null)
        {
            return NotFound($"The profile with userid {currentUserId} not found.");
        }
        
        var requestEntity = await friendShipRequestRepository.GetAsync(f => f.Id == friendshipResponseDto.FriendshipRequestId);
        
        if (requestEntity is null)
        {
            return NotFound($"The request with id {friendshipResponseDto.FriendshipRequestId} not found.");
        }

        if (requestEntity.ToUserId != currentUserId)
        {
            return Forbid();
        }

        if (requestEntity.Status != FriendshipStatusEnum.Pending)
        {
            return BadRequest("Request is no longer available to respond.");
        }
        
        requestEntity.Status = friendshipResponseDto.Accepted ? FriendshipStatusEnum.Accepted : FriendshipStatusEnum.Rejected;
        await friendShipRequestRepository.UpdateAsync(requestEntity);

        var newFriendship = new FriendShipEntity()
        {
            FriendAId = currentUserId,
            FriendBId = requestEntity.FromUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await friendShipRepository.CreateAsync(newFriendship);
        
        await publishEndpoint.Publish<SendNotification>(new(
            requestEntity.FromUserId,
            $"{currentUserProfile.DisplayName} {(friendshipResponseDto.Accepted ? "accepted" : "rejected")} your friendship request.",
            "",
            "{}",
            NotificationType.FriendshipRequest)
        );

        return Ok(newFriendship.Id);
    }
    
    [HttpPost("{requestId:guid}/cancel")]
    [Authorize]
    public async Task<ActionResult> CancelFriendshipRequest(Guid requestId)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";
        
        var requestEntity = await friendShipRequestRepository.GetAsync(f => f.Id == requestId);
        
        if (requestEntity is null)
        {
            return NotFound($"The request with id {requestId} not found.");
        }

        if (requestEntity.FromUserId.ToString() != currentUserId)
        {
            return Forbid();
        }

        if (requestEntity.Status != FriendshipStatusEnum.Pending)
        {
            return BadRequest("Request is no longer available to cancel.");
        }
        
        requestEntity.Status = FriendshipStatusEnum.Cancelled;
        await friendShipRequestRepository.UpdateAsync(requestEntity);

        return Ok();
    }

    [HttpPost("friend/{friendId:guid}/delete")]
    [Authorize]
    public async Task<ActionResult> DeleteFriendship(Guid friendId)
    {
        if (!Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var currentUserId))
        {
            return BadRequest();
        }
        
        var friendShipEntity = await friendShipRepository.GetAsync(f =>
            (f.FriendAId == currentUserId && f.FriendBId == friendId) ||
            (f.FriendAId == friendId && f.FriendBId == currentUserId));

        if (friendShipEntity is null)
        {
            return NoContent();
        }

        await friendShipRepository.DeleteAsync(friendShipEntity.Id);

        return Ok();
    }
}