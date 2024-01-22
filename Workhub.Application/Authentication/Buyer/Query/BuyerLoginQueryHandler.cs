using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Query;

public class BuyerLoginQueryHandler : IRequestHandler<BuyerLoginQuery, ErrorOr<BuyerAuthResult>>
{

    private readonly IJWTGenerator jWTGenerator;
    private readonly IBuyerProfileRepository buyerProfileRepository;

    public BuyerLoginQueryHandler(IJWTGenerator jWTGenerator, IBuyerProfileRepository buyerProfileRepository)
    {

        this.jWTGenerator = jWTGenerator;
        this.buyerProfileRepository = buyerProfileRepository;
    }

    public async Task<ErrorOr<BuyerAuthResult>> Handle(BuyerLoginQuery request, CancellationToken cancellationToken)
    {
        if (buyerProfileRepository.GetBuyerProfileByEmail(request.Email) is not BuyerProfile profile)
        {
            return Domain.Errors.Authentication.InvalidCredentials;
        }
        if (profile.Password != request.Password)
        {
            return Domain.Errors.Authentication.InvalidCredentials;
        }
        var token = jWTGenerator.GenerateJWTToken(profile.Email, profile.Id);
        return new BuyerAuthResult(profile, token);
    }
}
