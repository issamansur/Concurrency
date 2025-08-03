namespace MyOwnTests.ProducerConsumer;

/// <summary>
/// Интерфейс для паттерна/технологии ProducerConsumer.
/// Более универсальная реализация потом.
/// </summary>
public interface IProducerConsumer<in T>
{
    /// <summary>
    /// Представляет действие Producer - отправить сообщение.
    /// </summary>
    /// <param name="message">Сообщение, которое необходимо обработать.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Таска</returns>
    public Task EnqueueMessage(T message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Представляет действие Consumer - обрабатывать сообщения, пока имеются.
    /// </summary>
    /// <returns>Таска</returns>
    public Task ProcessMessages(CancellationToken cancellationToken = default);
}