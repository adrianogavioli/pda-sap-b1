using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class UsuarioSapService : IUsuarioSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public UsuarioSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=InternalKey,UserName,Locked";
        private readonly string filter = "&$filter=Group ne 'ug_Deleted'";

        public async Task<UsuarioSap> ObterUsuario(int internalKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Users({internalKey}){query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<UsuarioSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<UsuarioSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Users{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<UsuarioSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
