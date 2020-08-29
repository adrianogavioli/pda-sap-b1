using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoTipoSapService : IProdutoTipoSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoTipoSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=Code,Name";
        private readonly string filter = "&$filter=U_STATUS eq 'a' or U_STATUS eq 'A'";

        public async Task<ProdutoTipoSap> ObterTipo(int Code)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/U_FRTTIPO({Code}){query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ProdutoTipoSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ProdutoTipoSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/U_FRTTIPO{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoTipoSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
