using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Authentication.Buyer.Commands;

public class RegisterBuyerCommandHandler : IRequestHandler<RegisterBuyerCommand, ErrorOr<BuyerAuthResult>>
{
    private readonly IMediator mediator;
    private readonly IJWTGenerator jwtGenerator;
    private readonly IBuyerProfileRepository buyerProfileRepository;

    public RegisterBuyerCommandHandler(IMediator mediator, IJWTGenerator jwtGenerator, IBuyerProfileRepository buyerProfileRepository)
    {
        this.mediator = mediator;
        this.jwtGenerator = jwtGenerator;
        this.buyerProfileRepository = buyerProfileRepository;
    }

    public async Task<ErrorOr<BuyerAuthResult>> Handle(RegisterBuyerCommand request, CancellationToken cancellationToken)
    {

    }
}
