namespace Students.Service.src.Application.DTOs.Requests;

public sealed record MonthlyTuitionRequest
(
    Guid CourseId,
    Guid PaymentId,
    string Description,
    DateTime ReferenceMonthDate,
    DateTime DueDate,
    string Status
);