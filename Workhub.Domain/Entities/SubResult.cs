namespace Workhub.Domain.Entities;

public record SubResult(DateTime Expires, bool CompanySubActive, bool AccountActive, double amountDollar, double AmountNGN, string PaypalSecret, string PaypalKey, string NotoKashId);
