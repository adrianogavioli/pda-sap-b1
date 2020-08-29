using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ClienteGrupoSapService : IClienteGrupoSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ClienteGrupoSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=Code,Name,Type";

        public async Task<ClienteGrupoSap> ObterGrupo(int code)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPartnerGroups({code}){query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ClienteGrupoSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ClienteGrupoSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPartnerGroups{query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ClienteGrupoSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService.Dispose();
        }
    }
}
