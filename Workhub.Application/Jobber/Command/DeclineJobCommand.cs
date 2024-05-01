using MediatR;

namespace Workhub.Application.Jobber.Command;
public record DeclineJobCommand(string jobId) : IRequest;