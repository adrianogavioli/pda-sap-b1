using Frattina.CrossCutting.Comparador;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoModeloSapService : IProdutoModeloSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoModeloSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=Code,Name";
        private readonly string filter = "&$filter=U_STATUS eq 'a' or U_STATUS eq 'A'";
        private readonly string crossJoin = $"$crossjoin(U_FRTTIPO,U_FRTMARCA,U_FRTMODELO,U_FRTMARCAMODELOTIPO)" +
                                            $"?$expand=U_FRTMODELO($select=Code,Name)" +
                                            $"&$filter=U_FRTTIPO/Code eq U_FRTMARCAMODELOTIPO/U_FRTTIPOCODE " +
                                                $"and U_FRTMARCA/Code eq U_FRTMARCAMODELOTIPO/U_FRTMARCACODE " +
                                                $"and U_FRTMODELO/Code eq U_FRTMARCAMODELOTIPO/U_FRTMODELOCODE " +
                                                $"and (U_FRTTIPO/U_STATUS eq 'a' or U_FRTTIPO/U_STATUS eq 'A') " +
                                                $"and (U_FRTMARCA/U_STATUS eq 'a' or U_FRTMARCA/U_STATUS eq 'A') " +
                                                $"and (U_FRTMODELO/U_STATUS eq 'a' or U_FRTMODELO/U_STATUS eq 'A')";

        public async Task<ProdutoModeloSap> ObterModelo(int Code)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/U_FRTMODELO({Code}){query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ProdutoModeloSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ProdutoModeloSap>> ObterPorTipo(int tipoCode)
        {
            var crossJoinFilter = $"and U_FRTMARCAMODELOTIPO/U_FRTTIPOCODE eq {tipoCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{crossJoin} {crossJoinFilter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var produtoModelosCrossJoin = await _sapBaseService.ResolverResultadoResponse<ProdutoModeloSapCrossJoin>(response);

            return produtoModelosCrossJoin.Select(m => m.U_FRTMODELO).Distinct(
                (modeloA, modeloB) => modeloA.Code == modeloB.Code,
                modelo => modelo.Code.GetHashCode());
        }

        public async Task<IEnumerable<ProdutoModeloSap>> ObterPorMarca(int marcaCode)
        {
            var crossJoinFilter = $"and U_FRTMARCAMODELOTIPO/U_FRTMARCACODE eq {marcaCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{crossJoin} {crossJoinFilter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var produtoModelosCrossJoin = await _sapBaseService.ResolverResultadoResponse<ProdutoModeloSapCrossJoin>(response);

            return produtoModelosCrossJoin.Select(m => m.U_FRTMODELO).Distinct(
                (modeloA, modeloB) => modeloA.Code == modeloB.Code,
                modelo => modelo.Code.GetHashCode());
        }

        public async Task<IEnumerable<ProdutoModeloSap>> ObterPorTipoMarca(int tipoCode, int marcaCode)
        {
            var crossJoinFilter = $"and U_FRTMARCAMODELOTIPO/U_FRTTIPOCODE eq {tipoCode} " +
                                    $"and U_FRTMARCAMODELOTIPO/U_FRTMARCACODE eq {marcaCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{crossJoin} {crossJoinFilter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var produtoModelosCrossJoin = await _sapBaseService.ResolverResultadoResponse<ProdutoModeloSapCrossJoin>(response);

            return produtoModelosCrossJoin.Select(m => m.U_FRTMODELO);
        }

        public async Task<IEnumerable<ProdutoModeloSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/U_FRTMODELO{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoModeloSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
