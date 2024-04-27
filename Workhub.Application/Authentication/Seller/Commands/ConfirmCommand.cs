using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Commands;
public record ConfirmCommand(string email, string token) : IRequest<ErrorOr<ConfirmResponse>>;