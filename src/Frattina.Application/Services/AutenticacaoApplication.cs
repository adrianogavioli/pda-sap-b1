using Frattina.Application.Interfaces;
using Frattina.SapService.Interfaces;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class AutenticacaoApplication : IAutenticacaoApplication
    {
        private readonly IAutenticacaoSapService _autenticacaoSapService;

        public AutenticacaoApplication(IAutenticacaoSapService autenticacaoSapService)
        {
            _autenticacaoSapService = autenticacaoSapService;
        }

        public async Task<bool> Login()
        {
            return await _autenticacaoSapService.Login();
        }

        public async Task Logout()
        {
            await _autenticacaoSapService.Logout();
        }

        public void Dispose()
        {
            _autenticacaoSapService?.Dispose();
        }
    }
}
