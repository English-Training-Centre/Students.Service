namespace Students.Service.src.Application.DTOs.Requests;

public sealed record PaymentCreateRequest
(
    string ReceivedFrom,
    string DescriptionPt,
    string Method,
    long TotalPaid,
    string InWordsPt,
    string InWordsCode
);