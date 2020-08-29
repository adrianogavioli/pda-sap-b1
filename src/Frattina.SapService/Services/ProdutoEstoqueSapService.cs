using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoEstoqueSapService : IProdutoEstoqueSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoEstoqueSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=ItemWarehouseInfoCollection";

        public async Task<IEnumerable<ProdutoEstoqueSap>> ObterPorItemCode(string itemCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Items('{itemCode}'){query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseJson = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseJson)) return null;

            var queryResult = JsonConvert.DeserializeObject<QueryResult>(responseJson);

            return queryResult.ItemWarehouseInfoCollection;
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }

        private class QueryResult
        {
            public IEnumerable<ProdutoEstoqueSap> ItemWarehouseInfoCollection { get; set; }
        }
    }
}
