namespace Students.Service.src.Application.DTOs.Requests;

public sealed record GuardianCreateRequest
(
    Guid StudentId,
    string FullName,
    string PhoneNumber
);