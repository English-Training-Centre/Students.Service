using Grpc.Core;
using Libs.Core.Public.Protos.StudentService;
using Libs.Core.Public.src.DTOs.Requests;
using Libs.Core.Public.src.Interfaces;

namespace Students.Service.Services;

public sealed class StudentGrpcService(IStudentGrpcService studentHandler, ILogger<StudentGrpcService> logger) : StudentGrpc.StudentGrpcBase
{
    private readonly IStudentGrpcService _studentHandler = studentHandler;
    private readonly ILogger<StudentGrpcService> _logger = logger;

    public async override Task<GrpcStudentCreateResponse> CreateStudent(GrpcStudentCreateRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.StudentRoleId, out var studentRoleId))
        {
            throw new RpcException(new Status(StatusCode.Internal, "Student Role ID Invalid..."));
        }

        var parameter = new StudentGrpcCreateRequest
        (
            request.CourseModality,
            request.CoursePackage,
            request.CourseLevel,
            request.CourseTime,
            studentRoleId,
            request.StudentFullName,
            request.StudentGender,
            request.StudentBirthDate.ToDateTime(),
            request.StudentEmail,
            request.StudentPhoneNumber,       
            request.StudentResidencialAddress,
            request.GuardianFullName,
            request.GuardianPhoneNumber
        );

        try
        {
            var result = await _studentHandler.CreateAsync(parameter, context.CancellationToken);

            var protoResponse = new GrpcStudentCreateResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Username = result.Username,
                Password = result.Password
            };

            return protoResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: StudentGrpcService -> CreateStudent(....)");
            throw new RpcException(new Status(StatusCode.Internal, "Failed to create student"));
        }
    }
}