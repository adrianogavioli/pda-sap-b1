using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoGrupoSapService : IProdutoGrupoSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoGrupoSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=Number,GroupName";

        public async Task<ProdutoGrupoSap> ObterGrupo(int Number)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/ItemGroups({Number}){query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ProdutoGrupoSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ProdutoGrupoSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/ItemGroups{query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoGrupoSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
