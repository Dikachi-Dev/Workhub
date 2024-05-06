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
        var buyer = await profileRepository.GetById(response.BuyerId);
        response.SellerAddress = $"{profile.Address}, {profile.State}, {profile.Country}";
        //response.BuyerAddeess = $"{buyer.Address}, {buyer.State}, {buyer.Country}";
        repository.Update(response);
        await sender.SendFcmMessage(profile.Token, "Job Accepted", response.Id, "accepted", $"Job accepted by {response.SellerName}");
        return new GetJobResult(response);
    }
}