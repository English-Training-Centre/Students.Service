using Npgsql;
using Students.Service.src.Application.DTOs.Requests;
using Students.Service.src.Application.Interfaces;

namespace Students.Service.src.Infrastructure.Repositories;

public sealed class StudentRepository (IPostgresDB db, ILogger<StudentRepository> logger) : IStudentRepository
{
    private readonly IPostgresDB _db = db;
    private readonly ILogger<StudentRepository> _logger = logger;

    public async Task<Guid> CreateStudentAsync(StudentCreateRequest request, CancellationToken ct)
    {
        const string sql = @"
        INSERT INTO tbStudent (user_id, flyer_id, gender, birth_date, residential_address)
        VALUES (@UserId, @FlyerId, @Gender::gender_enum, @BirthDate, @ResidencialAddress)
        RETURNING id
        ";

        try
        {
            return await _db.ExecuteScalarAsync<Guid>(
                sql,
                ct,
                new NpgsqlParameter("@UserId", request.UserId),
                new NpgsqlParameter("@FlyerId", request.FlyerId),
                new NpgsqlParameter("@Gender", request.Gender),
                new NpgsqlParameter("@BirthDate", request.BirthDate),
                new NpgsqlParameter("@ResidencialAddress", request.ResidencialAddress)
            );
        }
        catch (PostgresException pgEx)
        {
            _logger.LogError(pgEx, " - Unexpected PostgreSQL Error.");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - Unexpected error during transaction operation.");
            return Guid.Empty;
        }
    }

    public async Task CreateGuardianAsync(GuardianCreateRequest request, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO tbGuardian (student_id, full_name, phone_number) 
            VALUES(@StudentId, @FullName, @PhoneNumber);
        ";

        try
        {
            await _db.ExecuteAsync(sql, request, ct);
        }
        catch (PostgresException pgEx)
        {
            _logger.LogError(pgEx, " - Unexpected PostgreSQL Error.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - Unexpected error during transaction operation.");
        }
    }

    public async Task<Guid> CreateCourseAsync(CourseCreateRequest request, CancellationToken ct)
    {
        const string sql = @"
        INSERT INTO tbCourse (student_id, modality, package, level, time)
        VALUES (@StudentId, @Modality::modality_enum, @Package::package_enum, @Level::level_enum, @Time)
        RETURNING id
        ";

        try
        {
            return await _db.ExecuteScalarAsync<Guid>(
                sql,
                ct,
                new NpgsqlParameter("@StudentId", request.StudentId),
                new NpgsqlParameter("@Modality", request.Modality),
                new NpgsqlParameter("@Package", request.Package),
                new NpgsqlParameter("@Level", request.Level),
                new NpgsqlParameter("@Time", request.Time)
            );
        }
        catch (PostgresException pgEx)
        {
            _logger.LogError(pgEx, " - Unexpected PostgreSQL Error.");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - Unexpected error during transaction operation.");
            return Guid.Empty;
        }
    }

    public async Task CreateMonthlyTuitionAsync(MonthlyTuitionRequest request, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO tbMonthlyTuition (course_id, payment_id, description, reference_month_date, due_date, status) 
            VALUES(@CourseId, @PaymentId, @Description, @ReferenceMonthDate, @DueDate, @Status::status_monthly_tuition_enum);
        ";

        try
        {
            await _db.ExecuteAsync(sql, request, ct);
        }
        catch (PostgresException pgEx)
        {
            _logger.LogError(pgEx, " - Unexpected PostgreSQL Error.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - Unexpected error during transaction operation.");
        }
    }

    public async Task<Guid> CreatePaymentAsync(PaymentCreateRequest request, CancellationToken ct)
    {
        const string sql = @"
        INSERT INTO tbPayments (received_from, description_pt, description_code, method, total_paid, in_words_pt, in_words_code)
        VALUES (@ReceivedFrom, @DescriptionPt, @DescriptionCode, @Method::pymt_method, @TotalPaid, @InWordsPt, @InWordsCode)
        RETURNING id
        ";

        try
        {
            return await _db.ExecuteScalarAsync<Guid>(
                sql,
                ct,
                new NpgsqlParameter("@ReceivedFrom", request.ReceivedFrom),
                new NpgsqlParameter("@DescriptionPt", request.DescriptionPt),
                new NpgsqlParameter("@DescriptionCode", request.DescriptionCode),
                new NpgsqlParameter("@Method", request.Method),
                new NpgsqlParameter("@TotalPaid", request.TotalPaid),
                new NpgsqlParameter("@InWordsPt", request.InWordsPt),
                new NpgsqlParameter("@InWordsCode", request.InWordsCode)
            );
        }
        catch (PostgresException pgEx)
        {
            _logger.LogError(pgEx, " - Unexpected PostgreSQL Error.");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " - Unexpected error during transaction operation.");
            return Guid.Empty;
        }
    }
}