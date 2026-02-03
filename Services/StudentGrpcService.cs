using Grpc.Core;
using Libs.Core.Public.Protos.StudentService;
using Libs.Core.Public.src.Interfaces;

namespace Students.Service.Services;

public sealed class StudentGrpcService(IStudentGrpcService studentService, ILogger<StudentGrpcService> logger) : StudentGrpc.StudentGrpcBase
{
    private readonly IStudentGrpcService _studentService = studentService;
    private readonly ILogger<StudentGrpcService> _logger = logger;

    public async override Task<GrpcStudentCreateResponse> CreateStudent(GrpcStudentCreateRequest request, ServerCallContext context)
    {}
}