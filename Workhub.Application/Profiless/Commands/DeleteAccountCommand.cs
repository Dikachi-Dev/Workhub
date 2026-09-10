using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Commands;

public record DeleteAccountCommand(string UserId) : IRequest<ErrorOr<Success>>;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, ErrorOr<Success>>
{
    private readonly IProfileRepository repository;

    public DeleteAccountCommandHandler(IProfileRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetById(request.UserId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        profile.SoftDelete();
        repository.Update(profile);
        await repository.SaveChanges();

        return Result.Success;
    }
}
