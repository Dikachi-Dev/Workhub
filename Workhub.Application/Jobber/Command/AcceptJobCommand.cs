using ErrorOr;
using MediatR;
using Workhub.Application.Jobber.Common;

namespace Workhub.Application.Jobber.Command;
public record AcceptJobCommand(string jobId) : IRequest<ErrorOr<GetJobResult>>;