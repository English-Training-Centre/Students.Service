namespace Students.Service.src.Application.DTOs.Requests;

public sealed record PaymentCreateRequest
(
    string ReceivedFrom,
    string DescriptionEn,
    string DescriptionCode,
    string Method,
    long TotalPaid,
    string AmountInWordsEn,
    string AmountInWordsCode
);