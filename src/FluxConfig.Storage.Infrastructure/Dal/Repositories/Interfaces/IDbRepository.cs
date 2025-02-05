using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories.Interfaces;

public interface IDbRepository
{
    public Task<IClientSession> CreateSessionAsync(CancellationToken cancellationToken);
}