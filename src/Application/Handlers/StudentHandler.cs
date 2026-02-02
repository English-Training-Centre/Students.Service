using Libs.Core.Internal.src.DTOs.Requests;
using Libs.Core.Internal.src.Interfaces;
using Libs.Core.Public.src.DTOs.Requests;
using Libs.Core.Public.src.DTOs.Responses;
using Libs.Core.Public.src.Interfaces;
using Students.Service.src.Application.DTOs.Requests;
using Students.Service.src.Application.Interfaces;

namespace Students.Service.src.Application.Handlers;

public sealed class StudentHandler(IStudentRepository studentRep, IUserGrpcService userClient, ISettingFlyerIdGrpcService flyerClient, ILogger<StudentHandler> logger) : IStudentGrpcService
{
    private readonly IStudentRepository _studentRep = studentRep;
    private readonly IUserGrpcService _userClient = userClient;
    private readonly ISettingFlyerIdGrpcService _flyerClient = flyerClient;
    private readonly ILogger<StudentHandler> _logger = logger;

    public async Task<StudentGrpcCreateResponse> CreateAsync(StudentGrpcCreateRequest request, CancellationToken ct)
    {
        var userClientRequest = new UserCreateRequest
        (
            request.StudentFullName,
            request.StudentPhoneNumber,
            request.StudentEmail,
            request.StudentRoleId
        );

        try
        {
            Guid flyerId = await _flyerClient.GetFlyerId(ct);
            var userClient = await _userClient.CreateAsync(userClientRequest, ct);

            if (!userClient.IsSuccess || userClient.UserId.Equals(Guid.Empty) || userClient.Username is null || userClient.Password is null)
            {
                return new StudentGrpcCreateResponse { IsSuccess = false, Message = userClient.Message };
            }
            else if (flyerId.Equals(Guid.Empty))
            {
                return new StudentGrpcCreateResponse { IsSuccess = false, Message = "Flyer ID is null" };
            }

            var studentRequest = new StudentCreateRequest
            (
                userClient.UserId,
                flyerId,
                request.StudentGender,
                request.StudentBirthDate,
                request.ResidencialAddress
            );

            Guid studentId = await _studentRep.CreateStudentAsync(studentRequest, ct);
            if (studentId.Equals(Guid.Empty))
            {
                return new StudentGrpcCreateResponse { IsSuccess = false, Message = "Student ID is null" };
            }

            if (request.GuardianFullName is not null && request.GuardianPhoneNumber is not null)
            {
                var guardianRequest = new GuardianCreateRequest
                (
                    studentId,
                    request.GuardianFullName,
                    request.GuardianPhoneNumber
                );

                await _studentRep.CreateGuardianAsync(guardianRequest, ct);
            }

            var courseRequest = new CourseCreateRequest
            (
                studentId,
                request.CourseModality,
                request.CoursePackage,
                request.CourseLevel,
                request.CourseTime
            );

            Guid courseId = await _studentRep.CreateCourseAsync(courseRequest, ct);
            if (courseId.Equals(Guid.Empty))
            {
                return new StudentGrpcCreateResponse { IsSuccess = false, Message = "Course ID is null" };
            }

            var now = DateTime.UtcNow;
            var referenceMonthDate = new DateTime(now.Year, now.Month, 1);
            var dueDate = new DateTime(now.Year, now.Month, 10);

            string currentMonth = GetMonth(referenceMonthDate);

            var monthlyTuitionRequest = new MonthlyTuitionRequest
            (
                courseId,
                $"{currentMonth} Tuition Fee",
                GetMonthCode(currentMonth),
                referenceMonthDate,
                dueDate,
                GetStatus(dueDate)
            );

            await _studentRep.CreateMonthlyTuitionAsync(monthlyTuitionRequest, ct);

            return new StudentGrpcCreateResponse
            {
                IsSuccess = true,
                Message = "Student created.",
                Username = userClient.Username,
                Password = userClient.Password
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - An unexpected error occurred...");
            return new StudentGrpcCreateResponse { IsSuccess = false, Message = "An unexpected error occurred..." };
        }
    }

    private static string GetMonth(DateTime referenceMonth)
    {
        return $"{referenceMonth:MMMM}";
    }

    private static string GetMonthCode(string month)
    {
        if (string.IsNullOrWhiteSpace(month))
            throw new ArgumentException("Month is required", nameof(month));

        return $"{month.ToUpperInvariant()}_TUITION_FEE";
    }

    private static string GetStatus(DateTime dueDate)
    {
        var now = DateTime.Now;
        if (dueDate >= now)
        { return "Pending"; }
        else if (dueDate < now)
        { return "Overdue"; }
        else
        { return "Error"; }
    }
}