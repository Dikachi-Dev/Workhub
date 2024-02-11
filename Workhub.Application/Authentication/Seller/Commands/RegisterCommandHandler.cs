using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthResult>>
{
    private readonly IMediator mediator;
    private readonly IJWTGenerator jWTGenerator;
    private readonly IProfileRepository repository;

    public RegisterCommandHandler(IProfileRepository repository, IMediator mediator, IJWTGenerator jWTGenerator)
    {
        this.repository = repository;
        this.mediator = mediator;
        this.jWTGenerator = jWTGenerator;
    }


    public async Task<ErrorOr<AuthResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (repository.GetProfileByEmail(command.Email) != null)
        {
            return Domain.Errors.Errors.Profile.DuplicateEmail;
        }
        var profile = new Profile
        {

            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Country = command.Country,
            State = command.State,
            Address = command.Address,
            Occupation = command.Occupation,
            Gender = command.Gender,
            Experience = command.Experience,
            Password = command.Password,
            ProfileImage = command.ProfileImage,
            NIN = command.NIN,
            LGA = command.LGA,
            LongLat = command.LongLat,
            UserType = command.UserType
        };
        await repository.Add(profile);
        await repository.SaveChanges();

        string token = jWTGenerator.GenerateJWTToken(command.Email, profile.Id);
        return new AuthResult(profile, token);
    }

}
