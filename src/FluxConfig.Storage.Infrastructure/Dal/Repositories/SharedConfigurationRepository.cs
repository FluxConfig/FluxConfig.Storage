using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public class SharedConfigurationRepository: ISharedConfigurationRepository
    
{
    public async Task CreateNewConfigurationDocument(string configurationKey, string configurationTag, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task DeleteConfigurationDocument(string configurationKey, string configurationTag, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5), cancellationToken);
        throw new NotImplementedException();
    }
}