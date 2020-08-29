using Frattina.CrossCutting.Configuration;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ProdutoSapService : IProdutoSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public ProdutoSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "$crossjoin(Items,ItemGroups,U_FRTTIPO,U_FRTMARCA,U_FRTMODELO,U_FRTFOTOITEM)"
                                        + "?$expand=Items($select=ItemCode,ItemName,SupplierCatalogNo,U_REFINTERNA,U_SEMI),"
                                                + "ItemGroups($select=Number,GroupName),"
                                                + "U_FRTTIPO($select=Code,Name),"
                                                + "U_FRTMARCA($select=Code,Name),"
                                                + "U_FRTMODELO($select=Code,Name),"
                                                + "U_FRTFOTOITEM($select=Code,U_IMG)"
                                        + "&$filter=Items/ItemsGroupCode eq ItemGroups/Number "
                                                + "and Items/U_FRTTIPOCODE eq U_FRTTIPO/Code "
                                                + "and Items/U_FRTMARCACODE eq U_FRTMARCA/Code "
                                                + "and Items/U_FRTMODELOCODE eq U_FRTMODELO/Code "
                                                + "and Items/ItemCode eq U_FRTFOTOITEM/U_ITEMCODE "
                                                + $"and U_FRTFOTOITEM/U_FRTGRUPOFOTOSCODE eq { AppSettings.Current.SapGrupoFotoPrincipal }"
                                                + "and U_FRTFOTOITEM/U_PRINCIPAL eq 'Y'";

        public async Task<ProdutoSap> ObterProduto(string itemCode)
        {
            var filter = $"and Items/ItemCode eq '{itemCode}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query} {filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<ProdutoSap>> ObterPorMasterCode(string masterCode)
        {
            var filter = $"and Items/U_REFINTERNA eq '{masterCode}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query} {filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);
        }

        public async Task<IEnumerable<ProdutoSap>> ObterPorTipoCode(int code)
        {
            var filter = $"and U_FRTTIPO/Code eq {code}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query} {filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);
        }

        public async Task<IEnumerable<ProdutoSap>> ObterPorMarcaCode(int code)
        {
            var filter = $"and U_FRTMARCA/Code eq {code}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query} {filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);
        }

        public async Task<IEnumerable<ProdutoSap>> ObterPorModeloCode(int code)
        {
            var filter = $"and U_FRTMODELO/Code eq {code}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query} {filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);
        }

        public async Task<IEnumerable<ProdutoSap>> ObterPorSupplierCatalog(string supplierCatalog)
        {
            var filter = $"and contains(Items/SupplierCatalogNo, '{supplierCatalog}')";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query} {filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);
        }

        public async Task<IEnumerable<ProdutoSap>> ObterPorTipoMarcaModeloCodes(int? tipoCode, int? marcaCode, int? modeloCode)
        {
            var filter = string.Empty;

            if (tipoCode != null)
                filter += $" and U_FRTTIPO/Code eq {tipoCode}";

            if (marcaCode != null)
                filter += $" and U_FRTMARCA/Code eq {marcaCode}";

            if (modeloCode != null)
                filter += $" and U_FRTMODELO/Code eq {modeloCode}";

            if (filter == string.Empty) return null;

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ProdutoSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
