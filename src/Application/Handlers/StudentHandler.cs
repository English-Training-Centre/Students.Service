using Libs.Core.Internal.src.Interfaces;
using Students.Service.src.Application.Interfaces;

namespace Students.Service.src.Application.Handlers;

public sealed class StudentHandler (IStudentRepository studentRep, IUserGrpcService userClient, ILogger<StudentHandler> logger)
{
    private readonly IStudentRepository _studentRep = studentRep;
    private readonly IUserGrpcService _userClient = userClient;
    private readonly ILogger<StudentHandler> _logger = logger;
}