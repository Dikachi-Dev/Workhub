using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Profiless.Commands;

public record SendMessageCommand(string UserId, string Priority, string Subject, string Message) : IRequest<ErrorOr<bool>>;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ErrorOr<bool>>
{
    private readonly IProfileRepository _repository;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public SendMessageCommandHandler(IProfileRepository repository, IEmailSender emailSender, IConfiguration configuration)
    {
        _repository = repository;
        _emailSender = emailSender;
        _configuration = configuration;
    }

    public async Task<ErrorOr<bool>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repository.GetById(request.UserId);
        if (profile == null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        string to = _configuration["Smtp:Email"] ?? "admin@workhub.com";
        string body = $"<p>{request.Message}</p>";
        await _emailSender.SendEmailAsync(to, $"Priority:{request.Priority} From: {profile.Email}, Subject: {request.Subject}", body);

        return true;
    }
}
