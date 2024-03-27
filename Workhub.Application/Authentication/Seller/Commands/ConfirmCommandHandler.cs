using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Authentication.Seller.Commands;
public class ConfirmCommandHandler : IRequestHandler<ConfirmCommand, ErrorOr<ConfirmResponse>>
{
    private readonly IMediator mediator;
    private readonly IJWTGenerator jWTGenerator;
    private readonly ICheckVerify verify;
    private readonly ISeriLogger logger;

    public ConfirmCommandHandler(IMediator mediator, IJWTGenerator jWTGenerator, ICheckVerify verify, ISeriLogger logger)
    {
        this.mediator = mediator;
        this.jWTGenerator = jWTGenerator;
        this.verify = verify;
        this.logger = logger;
    }

    public async Task<ErrorOr<ConfirmResponse>> Handle(ConfirmCommand request, CancellationToken cancellationToken)
    {
       if(await verify.checkVerifyStats(request.email) == true)
       {
        return Domain.Errors.Errors.Authentication.NotVerified;
       }
       var result = await verify.ConfirmEmail(request.token, request.email);
       if (result == true)
       {
        return new ConfirmResponse(true);
       }
       return Domain.Errors.Errors.Authentication.NotVerified;
    }
}