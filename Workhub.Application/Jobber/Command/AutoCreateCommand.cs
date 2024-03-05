using ErrorOr;
using MediatR;
using Workhub.Application.Jobber.Common;

namespace Workhub.Application.Jobber.Command;

public record AutoCreateCommand(string UserId, string Occupation) : IRequest<ErrorOr<GetJobResult>>;

