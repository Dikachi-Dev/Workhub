using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Jobber.Command;
public class DeclineJobCommandHandler : IRequestHandler<DeclineJobCommand>
{
    private readonly IJobRepository repository;

    private readonly INotificationSender sender;
    private readonly IProfileRepository profileRepository;

    public DeclineJobCommandHandler(IJobRepository repository, INotificationSender sender, IProfileRepository profileRepository)
    {
        this.repository = repository;
        this.sender = sender;
        this.profileRepository = profileRepository;
    }

    public async Task Handle(DeclineJobCommand request, CancellationToken cancellationToken)
    {
        var response = await repository.Decline(request.jobId);
        var profile = await profileRepository.GetById(response.SellerId);
        await sender.SendFcmMessage(profile.Token, "Job Declined", request.jobId, "declined", $"Job Declined by {response.SellerName}");
    }
}