using Frattina.CrossCutting.Configuration;
using Frattina.CrossCutting.UsuarioIdentity;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using KissLog;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class SapBaseService : ISapBaseService
    {
        private readonly IAutenticacaoSapService _autenticacaoSapService;
        private readonly IUsuarioIdentityService _usuarioIdentityService;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public SapBaseService(IAutenticacaoSapService autenticacaoSapService,
                              IUsuarioIdentityService usuarioIdentityService,
                              IHttpClientFactory httpClientFactory)
        {
            _autenticacaoSapService = autenticacaoSapService;
            _usuarioIdentityService = usuarioIdentityService;
            _httpClient = httpClientFactory.CreateClient("sap");
            _logger = Logger.Factory.Get();
        }

        public async Task<HttpResponseMessage> EnviarRequest(HttpRequestMessage request)
        {
            var policy = CriarPoliticaRenovacaoLogin();

            return await policy.ExecuteAsync(async () =>
            {
                var requestMessage = new HttpRequestMessage(request.Method, request.RequestUri);

                foreach (var header in request.Headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }

                requestMessage.Headers.Add("Cookie", await GetSapCookie());

                requestMessage.Content = request.Content;

                return await _httpClient.SendAsync(requestMessage);
            });
        }

        public async Task<bool> ResponseContemErro(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return false;

            await GravarLog(response);

            return true;
        }

        public async Task<SapError> ObterErroResponse(HttpResponseMessage response)
        {
            var erro = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SapError>(erro);
        }

        public async Task<IEnumerable<T>> ResolverResultadoResponse<T>(HttpResponseMessage response)
        {
            var responseJson = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseJson)) return null;

            var dataObject = JObject.Parse(responseJson);

            if (dataObject["value"] != null)
            {
                return await ObterResponseResultList<T>(dataObject);
            }
            else
            {
                return new List<T>
                {
                    await ObterResponseResultSingle<T>(dataObject)
                };
            }
        }

        private async Task<List<T>> ObterResponseResultList<T>(JObject dataObject)
        {
            var dataValue = dataObject["value"].Children().ToList();

            var dataJson = JsonConvert.SerializeObject(dataValue);

            var responseResult = JsonConvert.DeserializeObject<List<T>>(dataJson);

            var nextLink = dataObject["odata.nextLink"];

            if (nextLink == null) return responseResult;

            var response = await EnviarRequest(new HttpRequestMessage(HttpMethod.Get, nextLink.ToString()));

            responseResult.AddRange(await ResolverResultadoResponse<T>(response));

            return responseResult;
        }

        private Task<T> ObterResponseResultSingle<T>(JObject dataObject)
        {
            var dataJson = JsonConvert.SerializeObject(dataObject);

            return Task.FromResult(JsonConvert.DeserializeObject<T>(dataJson));
        }

        private RetryPolicy<HttpResponseMessage> CriarPoliticaRenovacaoLogin()
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(1, async (message, retryCount) =>
                {
                    await _autenticacaoSapService.Login();
                });

            return policy;
        }

        private async Task<List<string>> GetSapCookie()
        {
            var userClaims = await _usuarioIdentityService.GetClaimsUserAuth();

            var sessionId = userClaims?.FirstOrDefault(c => c.Type == "SapSessionId");

            return new List<string>
            {
                new string($"B1SESSION={ sessionId?.Value }"),
                new string($"CompanyDB={ AppSettings.Current.SapCompanyDB }"),
                new string($"ROUTEID={ AppSettings.Current.SapRouteId }")
            };
        }

        private async Task GravarLog(HttpResponseMessage response)
        {
            _logger.Error(await response.Content.ReadAsStringAsync());
        }

        public void Dispose()
        {
            _autenticacaoSapService?.Dispose();
            _usuarioIdentityService?.Dispose();
            _httpClient?.Dispose();
        }
    }
}
