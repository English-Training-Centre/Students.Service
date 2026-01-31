namespace Students.Service.src.Application.DTOs.Requests;

public sealed record StudentCreateRequest
(
    Guid UserId,
    Guid FlyerId,
    string Gender,
    DateTime BirthDate,
    string ResidencialAddress,
    bool IsEnrolled,
    Guid PaymentId
);