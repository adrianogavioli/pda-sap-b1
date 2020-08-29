using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class VendedorSapService : IVendedorSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public VendedorSapService(ISapBaseService sapBaseService)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=SalesEmployeeCode,SalesEmployeeName,Locked";

        public async Task<VendedorSap> ObterVendedor(int SalesEmployeeCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/SalesPersons({SalesEmployeeCode}){query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<VendedorSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<VendedorSap>> ObterTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/SalesPersons{query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<VendedorSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
