using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Jobber.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Jobber.Query;

internal class GetbyIdQueryHandler : IRequestHandler<GetbyIdQuery, ErrorOr<GetJobResult>>
{
    private readonly IJobRepository repository;
    private IMediator mediator;

    public GetbyIdQueryHandler(IJobRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<ErrorOr<GetJobResult>> Handle(GetbyIdQuery request, CancellationToken cancellationToken)
    {
        if (await repository.GetById(request.Id) is not Job job)
        {
            return Domain.Errors.Errors.Job.NotFound;
        }
        return new GetJobResult(job);
    }
}
