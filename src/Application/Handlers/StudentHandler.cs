using Libs.Core.Internal.src.DTOs.Requests;
using Libs.Core.Internal.src.Interfaces;
using Libs.Core.Public.src.DTOs.Requests;
using Libs.Core.Public.src.DTOs.Responses;
using Libs.Core.Public.src.Interfaces;
using Students.Service.src.Application.DTOs.Requests;
using Students.Service.src.Application.Interfaces;

namespace Students.Service.src.Application.Handlers;

public sealed class StudentHandler (IStudentRepository studentRep, IUserGrpcService userClient, ILogger<StudentHandler> logger) : IStudentGrpcService
{
    private readonly IStudentRepository _studentRep = studentRep;
    private readonly IUserGrpcService _userClient = userClient;
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
            var userClient = await _userClient.CreateAsync(userClientRequest, ct);

            if (!userClient.IsSuccess || userClient.UserId.Equals(Guid.Empty) || userClient.Username is null || userClient.Password is null)
            {
                return new StudentGrpcCreateResponse { IsSuccess = false, Message = userClient.Message };
            }

            var studentRequest = new StudentCreateRequest
            (
                userClient.UserId,
                Guid.Empty,
                request.StudentGender,
                request.StudentBirthDate,
                request.ResidencialAddress
            );   
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - An unexpected error occurred...");
            return new StudentGrpcCreateResponse { IsSuccess = false, Message = "An unexpected error occurred..." };
        }
    }
}