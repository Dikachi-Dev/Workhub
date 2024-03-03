using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Authentication.Seller.Commands;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<GetResult>>
{
    private readonly IMediator mediator;
    private readonly IProfileRepository repository;
    private readonly IJWTGenerator jWTGenerator;

    public UpdateCommandHandler(IMediator mediator, IProfileRepository repository, IJWTGenerator jWTGenerator)
    {
        this.mediator = mediator;
        this.repository = repository;
        this.jWTGenerator = jWTGenerator;
    }

    public async Task<ErrorOr<GetResult>> Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        var profile = repository.GetProfileByEmail(command.Email);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }
        profile.FirstName = command.FirstName;
        profile.LastName = command.LastName;
        profile.PhoneNumber = command.PhoneNumber;
        //profile.Email = command.Email;
        profile.Country = command.Country;
        profile.State = command.State;
        profile.Address = command.Address;
        profile.Experience = command.Experience;
        profile.Occupation = command.Occupation;

        repository.Update(profile);
        await repository.SaveChanges();

        //string token = jWTGenerator.GenerateJWTToken(profile);
        return new GetResult(profile);

    }
}
