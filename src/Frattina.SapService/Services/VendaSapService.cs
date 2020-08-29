using Frattina.Business.Interfaces;
using Frattina.Business.Services;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using Frattina.SapService.Models.Validations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class VendaSapService : BaseService, IVendaSapService
    {
        private readonly ISapBaseService _sapBaseService;

        public VendaSapService(ISapBaseService sapBaseService,
            INotificador notificador) : base(notificador)
        {
            _sapBaseService = sapBaseService;
        }

        private readonly string query = "?$select=DocEntry,DocNum,DocDate,CardCode,CardName,DocTotal,DocCurrency,DocTime,SalesPersonCode,DocumentLines";

        public async Task<VendaSap> Adicionar(VendaSap venda)
        {
            if (!ExecutarValidacao(new VendaSapValidation(), venda)) return null;

            venda.DocDate = DateTime.Today;
            venda.DocTime = DateTime.Now.TimeOfDay.ToString();
            venda.DocTotal = venda.DocumentLines.Sum(p => p.Price);

            foreach (var item in venda.DocumentLines)
            {
                item.Usage = 28; // Definir "utilização" (item consignado ou não)
                item.AccountCode = "3.01.01.01.01"; // Definir conta contábil (provável remover)
                item.TaxCode = "5102-001"; // Definir como será a identificação dos impostos (provável remover)
                item.WarehouseCode = "FROFVIT"; // Definir através dos estoques permitidos na Filial
                item.Quantity = 1;
                item.Price = item.UnitPrice;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "/b1s/v1/Invoices");

            request.Content = new StringContent(JsonConvert.SerializeObject(venda));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível adicionar a venda. {erroResponse.error.message.value}");

                return null;
            }

            var responseResult = await _sapBaseService.ResolverResultadoResponse<VendaSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<VendaSap> ObterVenda(int docEntry)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Invoices({docEntry}){query}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<VendaSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<VendaSap> ObterPorDocNum(int docNum)
        {
            var filter = $"&$filter=DocNum eq {docNum}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Invoices{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<VendaSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<IEnumerable<VendaSap>> ObterPorCardName(string cardName)
        {
            var filter = $"&$filter=CardName eq '{cardName}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Invoices{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<VendaSap>(response);
        }

        public async Task<IEnumerable<VendaSap>> ObterPorSalesPersonCode(int salesPersonCode)
        {
            var filter = $"&$filter=SalesPersonCode eq {salesPersonCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Invoices{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<VendaSap>(response);
        }

        public async Task<IEnumerable<VendaSap>> ObterPorDocDate(DateTime docDateIni, DateTime docDateFim)
        {
            var filter = $"&$filter=DocDate ge '{docDateIni:yyyy-MM-dd}' and DocDate le '{docDateFim:yyyy-MM-dd}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/Invoices{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<VendaSap>(response);
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
