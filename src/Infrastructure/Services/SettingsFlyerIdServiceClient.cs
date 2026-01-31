using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Libs.Core.Internal.Protos.SettingService;
using Libs.Core.Internal.src.Interfaces;

namespace Students.Service.src.Infrastructure.Services;

public sealed class SettingsFlyerIdServiceClient(SettingFlyerIdGrpc.SettingFlyerIdGrpcClient client, ILogger<SettingsFlyerIdServiceClient> logger) : ISettingFlyerIdGrpcService
{
    private readonly SettingFlyerIdGrpc.SettingFlyerIdGrpcClient _client = client;
    private readonly ILogger<SettingsFlyerIdServiceClient> _logger = logger;

    public async Task<Guid> GetFlyerId(CancellationToken ct)
    {
        GrpcSettingsFlyerIdResponse grpcResponse;

        try
        {
            grpcResponse = await _client.GetFlyerIdAsync(new Empty(), new CallOptions(
                deadline: DateTime.UtcNow.AddSeconds(10),
                cancellationToken: ct
            ));

            if (!Guid.TryParse(grpcResponse.Id, out var flyerId))
            {
                _logger.LogWarning("invalid Id");
                return Guid.Empty;
            }

            return flyerId;
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "gRPC failure in SettingsFlyerIdServiceClient.GetFlyerId");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while calling gRPC GetFlyerId");
            return Guid.Empty;
        }
    }
}