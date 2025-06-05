using System.ComponentModel.DataAnnotations;

namespace FutFut.Profile.Service;

public record ProfileDto(Guid Id, string DisplayName, string Avatar);

public record CreateProfileDto([Required] string DisplayName, string Avatar);
public record UpdateProfileDto([Required] string DisplayName, string Avatar);
