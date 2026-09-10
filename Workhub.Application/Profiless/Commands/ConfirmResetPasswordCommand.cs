using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Commands;

public record ConfirmResetPasswordCommand(string Email, string Code, string NewPassword) : IRequest<ErrorOr<bool>>;

public class ConfirmResetPasswordCommandHandler : IRequestHandler<ConfirmResetPasswordCommand, ErrorOr<bool>>
{
    private readonly IProfileRepository _repository;

    public ConfirmResetPasswordCommandHandler(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<bool>> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.ResetPassword(request.Email, request.Code, request.NewPassword);
        if (!result)
        {
            return Error.Validation("Password.ResetFailed", "Failed to reset password. The code may be invalid or expired.");
        }

        return true;
    }
}
