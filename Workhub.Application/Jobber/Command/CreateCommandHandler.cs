using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Jobber.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Jobber.Command;

public class CreateCommandHandler : IRequestHandler<CreateCommand, ErrorOr<GetJobResult>>
{
    private readonly IJobRepository jobRepository;
    private readonly IProfileRepository profileRepository;
    private readonly INotificationSender sender;

    public CreateCommandHandler(IJobRepository jobRepository,IProfileRepository profileRepository, INotificationSender sender)
    {
        this.jobRepository = jobRepository;
        this.profileRepository = profileRepository;
        this.sender = sender;
    }

    public async Task<ErrorOr<GetJobResult>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.GetById(request.BuyerId);
        var job = new Job
        {
            BuyerId = request.BuyerId,
            SellerId = request.SellerId,
            BuyerName = request.BuyerName,
            SellerName = request.SellerName,
            Occupation = request.Occupation,
            Status = "Pending",
            Profile = profile
        };
        await jobRepository.Add(job);
        await jobRepository.SaveChanges();
        await sender.SendFcmMessage(profile.Token, "New Job Alert", job.Id, "newjob");

        return new GetJobResult(job);
    }
}
