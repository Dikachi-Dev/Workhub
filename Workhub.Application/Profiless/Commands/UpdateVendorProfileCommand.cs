using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Profiless.Commands;

public record UpdateVendorProfileCommand(
    string UserId,
    string Description,
    IFormFile? Image1,
    IFormFile? Image2,
    string Instagram) : IRequest<ErrorOr<Success>>;

public class UpdateVendorProfileCommandHandler : IRequestHandler<UpdateVendorProfileCommand, ErrorOr<Success>>
{
    private readonly IProfileRepository repository;
    private readonly IFileStorageService fileStorage;

    public UpdateVendorProfileCommandHandler(IProfileRepository repository, IFileStorageService fileStorage)
    {
        this.repository = repository;
        this.fileStorage = fileStorage;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateVendorProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = repository.GetVendor(request.UserId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        string? image1Path = profile.VendorProfile?.Image1;
        if (request.Image1 != null && request.Image1.Length > 0)
        {
            image1Path = await fileStorage.SaveImageAsync(request.Image1, $"{request.UserId}_image1.png", resizeWidth: 200);
        }

        string? image2Path = profile.VendorProfile?.Image2;
        if (request.Image2 != null && request.Image2.Length > 0)
        {
            image2Path = await fileStorage.SaveImageAsync(request.Image2, $"{request.UserId}_image2.png", resizeWidth: 200);
        }

        profile.VendorProfile.UpdateDetails(request.Description, image1Path ?? string.Empty, image2Path ?? string.Empty, request.Instagram);

        repository.Update(profile);
        await repository.SaveChanges();

        return Result.Success;
    }
}
