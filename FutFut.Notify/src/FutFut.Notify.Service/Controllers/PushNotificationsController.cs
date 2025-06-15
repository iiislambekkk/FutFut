using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FutFut.Common;
using FutFut.Notify.Contracts;
using FutFut.Notify.Service.Data.Entities;
using FutFut.Notify.Service.Data.Enums;
using FutFut.Notify.Service.Firebase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace FutFut.Notify.Service.Controllers;

[ApiController]
[Route("notify")]
public class PushNotificationsController(
    FirebaseService firebaseService,
    IRepository<DeviceEntity> deviceRepo,
    IRepository<NotificationEntity> notificationRepo,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult<List<NotificationDto>>> GetAllNotificationsAsync(Guid userId)
    {
        var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";
        
        if (!(User.IsInRole("Admin") || currentUserId == userId.ToString()))
        {
            return Forbid();
        }

        var notifications = await notificationRepo.GetAllAsync(n => n.UserId == userId);

        return mapper.Map<List<NotificationDto>>(notifications);
    }
    
    [HttpPost("register")]
    [Authorize]
    public async Task<ActionResult> RegisterDevice(RegisterDeviceDto req)
    {
        if (!Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var currentUserId))
        {
            return Forbid();
        }
        if (!(User.IsInRole("Admin") || currentUserId == req.UserId))
        {
            return Forbid();
        }
        
        var newDeviceEntity = mapper.Map<DeviceEntity>(req);

        var userAgentString = HttpContext.Request.Headers["User-Agent"].ToString();
        var parser = Parser.GetDefault();

        var parsedUA = parser.Parse(userAgentString);
        newDeviceEntity.Name = parsedUA.OS.ToString() + " " + parsedUA.Device.ToString();

        await deviceRepo.CreateAsync(newDeviceEntity);
        
        return Ok();
    }
    
    
    [HttpPost]
    public async Task<ActionResult> SendNotify([FromBody] NotificationDto sendNotifyDto)
    {
        var notificationEntity = mapper.Map<NotificationEntity>(sendNotifyDto);
        notificationEntity.CreatedAt = DateTimeOffset.UtcNow;
        notificationEntity.Type = NotificationType.FriendshipRequest;
        
        await notificationRepo.CreateAsync(notificationEntity);

        var devicesTokens = (await deviceRepo.GetAllAsync(d => d.UserId == sendNotifyDto.UserId)).Select(d => d.Token).ToList();
        
        await firebaseService.SendAsync(
            sendNotifyDto,
            devicesTokens
        );

        return Ok();
    }
}