using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class EstoqueSapService : IEstoqueSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public EstoqueSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=WarehouseCode,WarehouseName,Nettable,BusinessPlaceID,Inactive";

        public async Task<IEnumerable<EstoqueSap>> ObterPorEmpresa(int BusinessPlaceID)
        {
            var filter = $"&$filter = BusinessPlaceID eq {BusinessPlaceID}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Warehouses{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<EstoqueSap>(response);
        }

        public async Task<IEnumerable<EstoqueSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Warehouses{query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<EstoqueSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
