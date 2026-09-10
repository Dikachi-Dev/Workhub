using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Commands;

public record ChangePasswordCommand(string UserId, string Password, string OldPassword) : IRequest<ErrorOr<bool>>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<bool>>
{
    private readonly IProfileRepository _repository;

    public ChangePasswordCommandHandler(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.ChangePass(request.Password, request.UserId, request.OldPassword);
        if (!result)
        {
            return Error.Validation("Password.ChangeFailed", "Failed to change password. Please verify your current password.");
        }

        return true;
    }
}
