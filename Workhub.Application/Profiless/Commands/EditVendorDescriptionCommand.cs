using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Commands;

public record EditVendorDescriptionCommand(
    string UserId,
    string Description) : IRequest<ErrorOr<Success>>;

public class EditVendorDescriptionCommandHandler : IRequestHandler<EditVendorDescriptionCommand, ErrorOr<Success>>
{
    private readonly IProfileRepository repository;

    public EditVendorDescriptionCommandHandler(IProfileRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(EditVendorDescriptionCommand request, CancellationToken cancellationToken)
    {
        var profile = repository.GetVendor(request.UserId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        profile.VendorProfile.UpdateDescription(request.Description);

        repository.Update(profile);
        await repository.SaveChanges();

        return Result.Success;
    }
}
