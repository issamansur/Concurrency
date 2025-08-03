namespace MyOwnTests.ProducerConsumer;

public abstract class BaseProducerConsumer<T>: IProducerConsumer<T>
{
    #region IProducerConsumer

    /// <summary>
    /// Представляет действие Producer - отправить сообщение.
    /// </summary>
    /// <param name="message">Сообщение, которое необходимо обработать</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таска</returns>
    public abstract Task EnqueueMessage(T message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Представляет действие Consumer - обрабатывать сообщения, пока имеются.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таска</returns>
    public abstract Task ProcessMessages(CancellationToken cancellationToken = default);

    #endregion

    /// <summary>
    /// Представляет действие Consumer - обработать ОДНО сообщение.
    /// </summary>
    /// <param name="message">Сообщение, которое необходимо обработать.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Таска</returns>
    protected virtual async Task ProcessMessage(T message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(5000, cancellationToken);
        Console.WriteLine("[Consumer] Message was handled");
    }

    /// <summary>
    /// Метод, который должен быть вызван при остановке.
    /// </summary>
    /// <returns></returns>
    public virtual Task OnStopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}