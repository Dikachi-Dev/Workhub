using ErrorOr;
using MediatR;
using Workhub.Application.Jobber.Common;

namespace Workhub.Application.Jobber.Command;

public record CreateCommand(string BuyerName,
    string SellerName,
    string SellerId,
    string BuyerId,
    string Occupation) : IRequest<ErrorOr<GetJobResult>>;

