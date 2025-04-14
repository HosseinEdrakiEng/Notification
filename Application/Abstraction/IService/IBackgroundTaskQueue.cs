namespace Application.Abstraction.IService
{
    public interface IBackgroundTaskQueue
    {
        ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem);
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }
}
