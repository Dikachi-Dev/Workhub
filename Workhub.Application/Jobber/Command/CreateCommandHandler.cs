using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Jobber.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Jobber.Command;

public class CreateCommandHandler : IRequestHandler<CreateCommand, ErrorOr<GetJobResult>>
{
    private readonly IJobRepository jobRepository;
    private readonly IProfileRepository profileRepository;
    private readonly IMediator mediator;

    public CreateCommandHandler(IJobRepository jobRepository, IMediator mediator, IProfileRepository profileRepository)
    {
        this.jobRepository = jobRepository;
        this.mediator = mediator;
        this.profileRepository = profileRepository;
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

        return new GetJobResult(job);
    }
}
