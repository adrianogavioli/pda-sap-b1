using Frattina.CrossCutting.Configuration;
using Frattina.CrossCutting.UsuarioIdentity;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class AutenticacaoSapService : IAutenticacaoSapService
    {
        private readonly IUsuarioIdentityService _usuarioIdentityService;
        private readonly HttpClient _httpClient;
        private readonly SapCredencial sapCredencial;

        public AutenticacaoSapService(IUsuarioIdentityService usuarioIdentityService,
                                      IHttpClientFactory httpClientFactory)
        {
            _usuarioIdentityService = usuarioIdentityService;
            _httpClient = httpClientFactory.CreateClient("sap");

            sapCredencial = new SapCredencial
            {
                CompanyDB = AppSettings.Current.SapCompanyDB,
                UserName = AppSettings.Current.SapUserName,
                Password = AppSettings.Current.SapPassword,
                ROUTEID = AppSettings.Current.SapRouteId
            };
        }

        public async Task<bool> Login()
        {
            var response = await _httpClient.PostAsync("/b1s/v1/Login", new StringContent(JsonConvert.SerializeObject(sapCredencial)));

            if (!response.IsSuccessStatusCode) return false;

            await RemoverSessionClaims();

            return await AdicionarSessionClaims(response);
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync("/b1s/v1/Logout", new StringContent(string.Empty));
        }

        private async Task RemoverSessionClaims()
        {
            var userClaims = await _usuarioIdentityService.GetClaimsUserAuth();

            var b1Session = userClaims.FirstOrDefault(c => c.Type == "SapSessionId");

            if (b1Session != null) await _usuarioIdentityService.RemoveClaimUserAuth(b1Session);
        }

        private async Task<bool> AdicionarSessionClaims(HttpResponseMessage response)
        {
            var sapSession = JsonConvert.DeserializeObject<SapSession>(await response.Content.ReadAsStringAsync());

            return await _usuarioIdentityService.CreateClaimUserAuth(new Claim("SapSessionId", sapSession.SessionId));
        }

        public void Dispose()
        {
            _usuarioIdentityService?.Dispose();

            _httpClient?.Dispose();
        }
    }
}
