namespace FluxConfig.Storage.Domain.Extensions;

public static class TaskExt
{
    public static async Task WhenAll(IEnumerable<Task> tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            await allTasks;
        }
        catch (Exception) {}

        if (allTasks.Exception != null)
        {
            throw allTasks.Exception;
        }
    }
    
    public static async Task<IEnumerable<T>> WhenAll<T>(IEnumerable<Task<T>> tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            return await allTasks;
        }
        catch (Exception) {}

        throw allTasks.Exception ?? throw new ArgumentException("Not possible");
    }
}