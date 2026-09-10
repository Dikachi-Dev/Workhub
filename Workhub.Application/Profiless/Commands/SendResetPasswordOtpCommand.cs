using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Commands;

public record SendResetPasswordOtpCommand(string Email) : IRequest<ErrorOr<bool>>;

public class SendResetPasswordOtpCommandHandler : IRequestHandler<SendResetPasswordOtpCommand, ErrorOr<bool>>
{
    private readonly IProfileRepository _repository;

    public SendResetPasswordOtpCommandHandler(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<bool>> Handle(SendResetPasswordOtpCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.ResetPassCode(request.Email);
        if (!result)
        {
            return Error.NotFound("User.NotFound", "User not found with provided email address.");
        }

        return true;
    }
}
