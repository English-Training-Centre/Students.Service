using Students.Service.src.Application.DTOs.Requests;

namespace Students.Service.src.Application.Interfaces;

public interface IStudentRepository
{
    Task<Guid> CreateStudentAsync(StudentCreateRequest request, CancellationToken ct);
    Task CreateGuardianAsync(GuardianCreateRequest request, CancellationToken ct);
    Task<Guid> CreateCourseAsync(CourseCreateRequest request, CancellationToken ct);
    Task CreateMonthlyTuitionAsync(MonthlyTuitionRequest request, CancellationToken ct);
    Task<Guid> CreatePaymentAsync(PaymentCreateRequest request, CancellationToken ct);
}