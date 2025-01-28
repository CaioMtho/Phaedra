using MongoDB.Driver;
using Phaedra.Server.Models.Entities.Documents;

namespace Phaedra.Server.Data
{
    public interface IDocumentContext
    {
        public IMongoCollection<Document> Documents { get; }
        public IMongoCollection<ContentBlock> ContentBlocks { get; }
        public IMongoCollection<TextSegment> TextSegments { get; }
        public IMongoCollection<TextStyle> TextStyles { get; }
    }
}
