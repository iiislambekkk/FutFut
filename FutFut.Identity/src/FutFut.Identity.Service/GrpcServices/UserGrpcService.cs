using FutFut.Common;
using FutFut.Identity.Service.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Shared.Grpc;

namespace FutFut.Identity.Service.GrpcServices;

public class UserGrpcService(UserManager<ApplicationUser> userManager) : User.UserBase
{
    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        
        if (user == null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        return new GetUserResponse() { UserId = user.Id, Email = user.Email, Name = user.UserName };
    }
}