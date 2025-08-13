using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KolomiietsEktosTest.UI.Models
{
    public class HeaderBytes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("header")]
        public byte[] Header { get; set; }
    }

}
