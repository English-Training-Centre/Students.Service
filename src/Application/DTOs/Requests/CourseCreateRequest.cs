namespace Students.Service.src.Application.DTOs.Requests;

public sealed record CourseCreateRequest
(
    Guid StudentId,
    string Modality,
    string Package,
    string Level,
    string Time
);