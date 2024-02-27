using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Profiless.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Profiless.Query;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ErrorOr<GetAllResult>>
{
    private readonly IMediator mediator;
    private readonly IProfileRepository repository;

    public GetAllQueryHandler(IMediator mediator, IProfileRepository repository)
    {
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task<ErrorOr<GetAllResult>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Profile> profiles = repository.GetAll();
        return new GetAllResult(profiles);
    }
}
