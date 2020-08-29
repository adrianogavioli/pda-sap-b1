using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoFichaTecnicaSapService : IProdutoFichaTecnicaSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoFichaTecnicaSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "$crossjoin(U_FRTCARACDETITEM,U_FRTDETALHE,U_FRTCARACTERISTICA)" +
                                        "?$expand=U_FRTCARACDETITEM($select=Code,Name,U_MEDIDA,U_VALOR)," +
                                                "U_FRTDETALHE($select=Code,Name)," +
                                                "U_FRTCARACTERISTICA($select=Code,Name)" +
                                        "&$filter=U_FRTCARACDETITEM/U_FRTDETALHECODE eq U_FRTDETALHE/Code " +
                                                "and U_FRTDETALHE/U_FRTCARACTERISTICACODE eq U_FRTCARACTERISTICA/Code ";

        public async Task<IEnumerable<ProdutoFichaTecnicaSap>> ObterPorItemCode(string itemCode)
        {
            var filter = $"and U_FRTCARACDETITEM/U_ITEMCODE eq '{itemCode}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoFichaTecnicaSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
