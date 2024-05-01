using ErrorOr;
using MediatR;
using Newtonsoft.Json;
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

    public CreateCommandHandler(IJobRepository jobRepository, IProfileRepository profileRepository, INotificationSender sender)
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
        };
        await jobRepository.Add(job);
        await jobRepository.SaveChanges();
        string body = JsonConvert.SerializeObject(new { jobId = job.Id, buyerName = job.BuyerName });
        await sender.SendFcmMessage(profile.Token, "New Job Alert", body, "newjob", $"You have new Hire Request from {job.BuyerName}");

        return new GetJobResult(job);
    }
}
