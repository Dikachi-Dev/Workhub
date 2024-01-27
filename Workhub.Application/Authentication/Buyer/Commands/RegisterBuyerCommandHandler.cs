using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Commands;

public class RegisterBuyerCommandHandler : IRequestHandler<RegisterBuyerCommand, ErrorOr<BuyerAuthResult>>
{
    //private readonly IMediator mediator;
    private readonly IJWTGenerator jwtGenerator;
    private readonly IBuyerProfileRepository repository;

    public RegisterBuyerCommandHandler(IJWTGenerator jwtGenerator, IBuyerProfileRepository repository)
    {
        // this.mediator = mediator;
        this.jwtGenerator = jwtGenerator;
        this.repository = repository;
    }

    public async Task<ErrorOr<BuyerAuthResult>> Handle(RegisterBuyerCommand command, CancellationToken cancellationToken)
    {
        if (repository.GetBuyerProfileByEmail(command.Email) != null)
        {
            return Domain.Errors.BuyerProfile.DuplicateEmail;
        }
        var buyer = new BuyerProfile
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Country = command.Country,
            State = command.State,
            Address = command.Address,
            Password = command.Password,
        };
        repository.Add(buyer);
        repository.SaveChanges();

        string token = jwtGenerator.GenerateJWTToken(command.Email, buyer.Id);
        return new BuyerAuthResult(buyer, token);
    }
}


