using MediatR;

namespace Workhub.Application.Jobber.Command;
public record CanCelJobCommand(string jobId, string userId) : IRequest;