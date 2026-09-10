using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Profiless.Query;

public record VendorProfileResult(
    string Description,
    string Instagram,
    string Image1,
    string Image2,
    string Id,
    string FirstName,
    string LastName,
    string Address,
    string Occupation,
    string Country,
    string State,
    int Rating,
    string PhoneNumber);

public record GetVendorProfileQuery(string VendorId) : IRequest<ErrorOr<VendorProfileResult>>;

public class GetVendorProfileQueryHandler : IRequestHandler<GetVendorProfileQuery, ErrorOr<VendorProfileResult>>
{
    private readonly IProfileRepository repository;
    private readonly IFileStorageService fileStorage;

    public GetVendorProfileQueryHandler(IProfileRepository repository, IFileStorageService fileStorage)
    {
        this.repository = repository;
        this.fileStorage = fileStorage;
    }

    public async Task<ErrorOr<VendorProfileResult>> Handle(GetVendorProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = repository.GetVendor(request.VendorId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        string image1Url = fileStorage.GetImageUrl(profile.VendorProfile?.Image1);
        string image2Url = fileStorage.GetImageUrl(profile.VendorProfile?.Image2);

        return new VendorProfileResult(
            Description: profile.VendorProfile?.Description ?? string.Empty,
            Instagram: profile.VendorProfile?.Instagram ?? string.Empty,
            Image1: image1Url,
            Image2: image2Url,
            Id: profile.Id,
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            Address: profile.Address,
            Occupation: profile.Occupation,
            Country: profile.Country,
            State: profile.State,
            Rating: profile.Rating,
            PhoneNumber: profile.PhoneNumber);
    }
}
