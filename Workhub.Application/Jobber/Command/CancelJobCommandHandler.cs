using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Jobber.Command;
public class CancelJobCommandHandler : IRequestHandler<CanCelJobCommand>
{
    private readonly IJobRepository repository;
    private readonly INotificationSender sender;
    private readonly IProfileRepository profileRepository;
    
    public CancelJobCommandHandler(IJobRepository repository, INotificationSender sender, IProfileRepository profileRepository)
    {
        this.repository = repository;
        this.sender = sender;
        this.profileRepository = profileRepository;
    }

    public async  Task Handle(CanCelJobCommand request, CancellationToken cancellationToken)
    {
       var job = await repository.GetById(request.jobId);
       string buyer = job.BuyerId;
       string seller = job.SellerId;
       if (request.userId == buyer)
       {
        var profile = await profileRepository.GetById(seller);
        await sender.SendFcmMessage(profile.Token, "Job Canceled", request.jobId, "Cancelled");
        }
        else{
        var profile = await profileRepository.GetById(buyer);
        await sender.SendFcmMessage(profile.Token, "Job Canceled", request.jobId, "Cancelled");
        }
       repository.Cancel(request.jobId);
    }
}