namespace SmsManager.Domain.Documents.Common.Interface;

public interface IDocument
{
}

public interface IDocument<out TKey> : IDocument where TKey : IEquatable<TKey>
{
    public TKey Id { get; }
    DateTime CreatedAt { get; set; }
    
    DateTime? UpdatedAt { get; set; }
}