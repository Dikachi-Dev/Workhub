using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthResult>>
{
    private readonly IMediator mediator;
    private readonly IJWTGenerator jWTGenerator;
    private readonly IProfileRepository repository;
    private readonly ISeriLogger logger;
  

    public RegisterCommandHandler(IProfileRepository repository, IMediator mediator, IJWTGenerator jWTGenerator, ISeriLogger logger)
    {
        this.repository = repository;
        this.mediator = mediator;
        this.jWTGenerator = jWTGenerator;
        this.logger = logger;
        
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
            NIN = command.Nin,
            LongLat = command.LongLat,
            UserType = command.UserType
        };
        var user = await repository.Register(profile);
        var claims = await repository.Login(profile.Email, profile.Password);
        if (claims.Count == 0)
        {
            logger.LogInError(profile.Email, DateTime.UtcNow, "InValid Email");
            return Domain.Errors.Errors.Authentication.InvalidCredentials;
        }
        var token = jWTGenerator.GenerateJWTToken(claims);
        logger.LogInformation(profile.Email, DateTime.UtcNow);
        return new AuthResult(token);
    }

}
