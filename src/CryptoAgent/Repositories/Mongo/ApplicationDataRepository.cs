﻿using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Bit.CryptoAgent.Repositories.Mongo
{
    public class ApplicationDataRepository : BaseRepository<ApplicationDataRepository.ApplicationData>,
        IApplicationDataRepository
    {
        public ApplicationDataRepository(CryptoAgentSettings settings)
            : base(settings, "ApplicationData")
        { }

        public async Task<string> ReadSymmetricKeyAsync()
        {
            var document = await Collection.Find(new BsonDocument()).FirstOrDefaultAsync();
            return document.SymmetricKey;
        }

        public async Task UpdateSymmetricKeyAsync(string key)
        {
            var document = await Collection.Find(new BsonDocument()).FirstOrDefaultAsync();
            document.SymmetricKey = key;
            await Collection.ReplaceOneAsync(d => d.Id == document.Id, document, new ReplaceOptions
            {
                IsUpsert = true
            });
        }

        public class ApplicationData
        {
            public ObjectId Id { get; set; } = ObjectId.Empty;
            public string SymmetricKey { get; set; }
        }
    }
}