using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Query;

public class BuyerLoginQueryHandler : IRequestHandler<BuyerLoginQuery, ErrorOr<BuyerAuthResult>>
{

    private readonly IJWTGenerator jWTGenerator;
    private readonly IBuyerProfileRepository buyerProfileRepository;
    private readonly ISeriLogger logger;

    public BuyerLoginQueryHandler(IJWTGenerator jWTGenerator, IBuyerProfileRepository buyerProfileRepository, ISeriLogger logger)
    {

        this.jWTGenerator = jWTGenerator;
        this.buyerProfileRepository = buyerProfileRepository;
        this.logger = logger;
    }

    public async Task<ErrorOr<BuyerAuthResult>> Handle(BuyerLoginQuery request, CancellationToken cancellationToken)
    {
        if (buyerProfileRepository.GetBuyerProfileByEmail(request.Email) is not BuyerProfile profile)
        {
            logger.LogInError(request.Email, DateTime.UtcNow, "InValid Email");
            return Domain.Errors.Authentication.InvalidCredentials;
        }
        if (profile.Password != request.Password)
        {
            logger.LogInError(request.Email, DateTime.UtcNow, "InValid Password");
            return Domain.Errors.Authentication.InvalidCredentials;
        }
        var token = jWTGenerator.GenerateJWTToken(profile.Email, profile.Id);
        logger.LogInformation(profile.Email, DateTime.UtcNow);
        return new BuyerAuthResult(profile, token);
    }
}
