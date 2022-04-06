using CommandApi.Models;

namespace CommandApi.GrpcDataServices.Abstractions
{
    public interface IPlatformGrpcClient
    {
        Task<IEnumerable<Platform>> ReturnAllPlatforms();
    }
}