using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using todo.Models;

namespace todo
{
    public class CosmosDBRepository<T> where T : class
    {

        private static AppConfiguration _option;
        private static DocumentClient _client;

        public CosmosDBRepository(AppConfiguration option)
        {
            _option = option;

            // 接続ポリシーの作成
            ConnectionPolicy cp = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
#if RELEASE
                ConnectionProtocol = Protocol.Tcp
#endif
            };

            // このAppのリージョンを追加
            cp.PreferredLocations.Add(_option.Region);

            // CosmosDB接続クライアント作成
            _client = new DocumentClient(new Uri(_option.Endpoint), _option.Key, cp);
            _client.OpenAsync(); // パフォーマンス改善のため一度接続しておく
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_option.DatabaseId, _option.CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_option.DatabaseId, _option.CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_option.DatabaseId, _option.CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_option.DatabaseId, _option.CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_option.DatabaseId, _option.CollectionId, id));
        }

        public EndPointViewModel GetEndpoints()
        {
            var vm = new EndPointViewModel
            {
                WriteEndPoint = _client.WriteEndpoint.Host,
                ReadEndPoint = _client.ReadEndpoint.Host
            };
            return vm;
        }


    }
}
