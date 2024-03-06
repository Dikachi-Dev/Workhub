using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Authentication.Seller.Query;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthResult>>
{
    private readonly IMediator mediator;
    private readonly IProfileRepository repository;
    private readonly IJWTGenerator jWTGenerator;
    private readonly ISeriLogger logger;

    public LoginQueryHandler(IJWTGenerator jWTGenerator, IProfileRepository repository, IMediator mediator, ISeriLogger logger)
    {
        this.jWTGenerator = jWTGenerator;
        this.repository = repository;
        this.mediator = mediator;
        this.logger = logger;
    }


    public async Task<ErrorOr<AuthResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var claims = await repository.Login(request.Email, request.Password);
        if (claims.Count == 0)
        {
            logger.LogInError(request.Email, DateTime.UtcNow, "InValid Email");
            return Domain.Errors.Errors.Authentication.InvalidCredentials;
        }

        var token = jWTGenerator.GenerateJWTToken(claims);
        logger.LogInformation(request.Email, DateTime.UtcNow);

        return new AuthResult(token);
    }
}
