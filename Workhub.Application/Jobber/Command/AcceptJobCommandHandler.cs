using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Jobber.Common;

namespace Workhub.Application.Jobber.Command;
public class AcceptJobCommandHandler : IRequestHandler<AcceptJobCommand, ErrorOr<GetJobResult>>
{
    private readonly IJobRepository repository;

    private readonly INotificationSender sender;
    private readonly IProfileRepository profileRepository;

    public AcceptJobCommandHandler(IJobRepository repository, IProfileRepository profileRepository, INotificationSender sender)
    {
        this.repository = repository;
        this.profileRepository = profileRepository;
        this.sender = sender;
    }

    public async Task<ErrorOr<GetJobResult>> Handle(AcceptJobCommand request, CancellationToken cancellationToken)
    {
        var response = await repository.Accept(request.jobId);
        var profile = await profileRepository.GetById(response.SellerId);
        await sender.SendFcmMessage(profile.Token,"Job Accepted",response.Id,"Accepted");
        return new GetJobResult(response);
    }
}