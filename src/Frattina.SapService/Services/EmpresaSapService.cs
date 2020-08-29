using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class EmpresaSapService : IEmpresaSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public EmpresaSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=BPLID,BPLName,Disabled,FederalTaxID,AddressType,Street,StreetNo,Building,ZipCode,Block,City,State,Country,AliasName";

        public async Task<EmpresaSap> ObterEmpresa(int BPLID)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPlaces({BPLID}){query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<EmpresaSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<EmpresaSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPlaces{query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<EmpresaSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
