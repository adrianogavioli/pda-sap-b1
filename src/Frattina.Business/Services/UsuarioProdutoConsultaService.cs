using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class UsuarioProdutoConsultaService : BaseService, IUsuarioProdutoConsultaService
    {
        private readonly IUsuarioProdutoConsultaRepository _usuarioProdutoConsultaRepository;

        public UsuarioProdutoConsultaService(IUsuarioProdutoConsultaRepository usuarioProdutoConsultaRepository,
                                                INotificador notificador) : base(notificador)
        {
            _usuarioProdutoConsultaRepository = usuarioProdutoConsultaRepository;
        }

        public async Task Adicionar(UsuarioProdutoConsulta usuarioProdutoConsulta)
        {
            if (!ExecutarValidacao(new UsuarioProdutoConsultaValidation(), usuarioProdutoConsulta)) return;

            await _usuarioProdutoConsultaRepository.Adicionar(usuarioProdutoConsulta);
        }

        public void Dispose()
        {
            _usuarioProdutoConsultaRepository?.Dispose();
        }
    }
}
