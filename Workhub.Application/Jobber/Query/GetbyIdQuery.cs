using ErrorOr;
using MediatR;
using Workhub.Application.Jobber.Common;

namespace Workhub.Application.Jobber.Query;

public record GetbyIdQuery(string Id) : IRequest<ErrorOr<GetJobResult>>;
