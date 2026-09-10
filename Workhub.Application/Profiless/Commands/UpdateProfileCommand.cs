using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Profiless.Commands;

public record UpdateProfileCommand(
    string UserId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    IFormFile? Image) : IRequest<ErrorOr<Success>>;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ErrorOr<Success>>
{
    private readonly IProfileRepository repository;
    private readonly IFileStorageService fileStorage;

    public UpdateProfileCommandHandler(IProfileRepository repository, IFileStorageService fileStorage)
    {
        this.repository = repository;
        this.fileStorage = fileStorage;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetById(request.UserId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        string? imagePath = null;
        if (request.Image != null && request.Image.Length > 0)
        {
            imagePath = await fileStorage.SaveImageAsync(request.Image, $"{request.UserId}_profileImage.png", resizeWidth: 160);
        }

        profile.UpdatePersonalInfo(request.FirstName, request.LastName, request.PhoneNumber, imagePath);

        repository.Update(profile);
        await repository.SaveChanges();

        return Result.Success;
    }
}
