using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Jobber.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Jobber.Command
{
    internal class CreateCommandHandler : IRequestHandler<CreateCommand, ErrorOr<GetResult>>
    {
        private readonly IJobRepository jobRepository;
        private readonly IMediator mediator;

        public CreateCommandHandler(IJobRepository jobRepository, IMediator mediator)
        {
            this.jobRepository = jobRepository;
            this.mediator = mediator;
        }

        public async Task<ErrorOr<GetResult>> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var job = new Job
            {
                BuyerId = request.BuyerId,
                SellerId = request.SellerId,
                BuyerName = request.BuyerName,
                SellerName = request.SellerName,
                Occupation = request.Occupation,
            };
            await jobRepository.Add(job);
            await jobRepository.SaveChanges();

            return new GetResult(job);
        }
    }
}
