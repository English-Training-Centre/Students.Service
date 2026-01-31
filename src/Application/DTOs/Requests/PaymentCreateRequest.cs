namespace Students.Service.src.Application.DTOs.Requests;

public sealed record PaymentCreateRequest
(
    string ReceivedFrom,
    string DescriptionPt,
    string DescriptionCode,
    string Method,
    long TotalPaid,
    string InWordsPt,
    string InWordsCode
);