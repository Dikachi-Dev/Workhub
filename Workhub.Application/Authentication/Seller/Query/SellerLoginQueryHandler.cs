using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query;

public class SellerLoginQueryHandler : IRequestHandler<SellerLoginQuery, ErrorOr<SellerAuthResult>>
{
    private readonly IMediator mediator;
    private readonly ISellerProfileRepository repository;
    private readonly IJWTGenerator jWTGenerator;
    private readonly ISeriLogger logger;

    public SellerLoginQueryHandler(IJWTGenerator jWTGenerator, ISellerProfileRepository repository, IMediator mediator, ISeriLogger logger)
    {
        this.jWTGenerator = jWTGenerator;
        this.repository = repository;
        this.mediator = mediator;
        this.logger = logger;
    }


    public async Task<ErrorOr<SellerAuthResult>> Handle(SellerLoginQuery request, CancellationToken cancellationToken)
    {
        if (repository.GetSellerProfileByEmail(request.Email) is not SellerProfile profile)
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

        return new SellerAuthResult(profile, token);
    }
}
