using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Commands;

public class RegisterSellerCommandHandler : IRequestHandler<RegisterSellerCommand, ErrorOr<SellerAuthResult>>
{
    private readonly IMediator mediator;
    private readonly IJWTGenerator jWTGenerator;
    private readonly ISellerProfileRepository repository;

    public RegisterSellerCommandHandler(ISellerProfileRepository repository, IMediator mediator, IJWTGenerator jWTGenerator)
    {
        this.repository = repository;
        this.mediator = mediator;
        this.jWTGenerator = jWTGenerator;
    }


    public async Task<ErrorOr<SellerAuthResult>> Handle(RegisterSellerCommand command, CancellationToken cancellationToken)
    {
        if (repository.GetSellerProfileByEmail(command.Email) != null)
        {
            return Domain.Errors.SellerProfile.DuplicateEmail;
        }
        var seller = new SellerProfile
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
            Experience = command.Experience

        };
        repository.Add(seller);
        repository.SaveChanges();

        string token = jWTGenerator.GenerateJWTToken(command.Email, seller.Id);
        return new SellerAuthResult(seller, token);
    }

}
