namespace Students.Service.src.Application.DTOs.Requests;

public sealed record MonthlyTuitionRequest
(
    Guid CourseId,
    string DescriptionEn,
    string DescriptionCode,
    DateTime ReferenceMonthDate,
    DateTime DueDate,
    string Status
);