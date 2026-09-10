namespace Workhub.Application.Common.Models;

public record SubResult(
    DateTime Expires,
    bool CompanySubActive,
    bool AccountActive,
    double amountDollar,
    double AmountNGN,
    string PaypalSecret,
    string PaypalKey,
    string NotoKashId);
