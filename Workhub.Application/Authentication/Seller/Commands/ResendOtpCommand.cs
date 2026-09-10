using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Authentication.Seller.Commands;

public record ResendOtpCommand(string Email) : IRequest<ErrorOr<bool>>;

public class ResendOtpCommandHandler : IRequestHandler<ResendOtpCommand, ErrorOr<bool>>
{
    private readonly ICheckVerify verify;

    public ResendOtpCommandHandler(ICheckVerify verify)
    {
        this.verify = verify;
    }

    public async Task<ErrorOr<bool>> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        bool result = await verify.ResendOTP(request.Email);
        return result;
    }
}
