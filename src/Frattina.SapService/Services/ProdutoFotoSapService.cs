using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoFotoSapService : IProdutoFotoSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoFotoSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "$crossjoin(U_FRTFOTOITEM,U_FRTGRUPOFOTOSITEM)" +
                                        "?$expand=U_FRTFOTOITEM($select=Code,U_IMG,U_ITEMCODE,U_PRINCIPAL)," +
                                                "U_FRTGRUPOFOTOSITEM($select=Name)" +
                                        "&$filter=U_FRTFOTOITEM/U_FRTGRUPOFOTOSCODE eq U_FRTGRUPOFOTOSITEM/Code";

        public async Task<ProdutoFotoSap> ObterFoto(int Code)
        {
            var filter = $" and U_FRTFOTOITEM/Code eq '{Code}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ProdutoFotoSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ProdutoFotoSap>> ObterPorItemCode(string itemCode)
        {
            var filter = $" and U_FRTFOTOITEM/U_ITEMCODE eq '{itemCode}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoFotoSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
