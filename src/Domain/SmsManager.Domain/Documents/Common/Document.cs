using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmsManager.Domain.Documents.Common.Interface;

namespace SmsManager.Domain.Documents.Common;

public abstract class Document : IDocument<string>
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    [BsonElement(Order = 0)]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement(Order = 101)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement(Order = 102)]
    public DateTime? UpdatedAt { get; set; }
}