using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoMarcaSapService : IProdutoMarcaSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoMarcaSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=Code,Name";
        private readonly string filter = "&$filter=U_STATUS eq 'a' or U_STATUS eq 'A'";
        private readonly string crossJoin = $"$crossjoin(U_FRTTIPO,U_FRTMARCA,U_FRTTIPOMARCA)" +
                                            $"?$expand=U_FRTMARCA($select=Code,Name)" +
                                            $"&$filter=U_FRTTIPO/Code eq U_FRTTIPOMARCA/U_FRTTIPOCODE " +
                                                $"and U_FRTMARCA/Code eq U_FRTTIPOMARCA/U_FRTMARCACODE " +
                                                $"and (U_FRTTIPO/U_STATUS eq 'a' or U_FRTTIPO/U_STATUS eq 'A') " +
                                                $"and (U_FRTMARCA/U_STATUS eq 'a' or U_FRTMARCA/U_STATUS eq 'A')";

        public async Task<ProdutoMarcaSap> ObterMarca(int Code)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/U_FRTMARCA({Code}){query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ProdutoMarcaSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ProdutoMarcaSap>> ObterPorTipo(int tipoCode)
        {
            var crossJoinFilter = $"and U_FRTTIPOMARCA/U_FRTTIPOCODE eq {tipoCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{crossJoin} {crossJoinFilter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var produtoMarcasCrossJoin = await _sapBaseService.ResolverResultadoResponse<ProdutoMarcaSapCrossJoin>(response);

            return produtoMarcasCrossJoin.Select(m => m.U_FRTMARCA);
        }

        public async Task<IEnumerable<ProdutoMarcaSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/U_FRTMARCA{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoMarcaSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
