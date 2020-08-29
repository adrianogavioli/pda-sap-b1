using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class AtendimentoVendidoService : BaseService, IAtendimentoVendidoService
    {
        private readonly IAtendimentoVendidoRepository _atendimentoVendidoRepository;

        public AtendimentoVendidoService(IAtendimentoVendidoRepository atendimentoVendidoRepository,
            INotificador notificador) : base(notificador)
        {
            _atendimentoVendidoRepository = atendimentoVendidoRepository;
        }

        public async Task Adicionar(AtendimentoVendido atendimentoVendido)
        {
            atendimentoVendido.Data = DateTime.Now;
            atendimentoVendido.Atendimento = null;

            if (!ExecutarValidacao(new AtendimentoVendidoValidation(), atendimentoVendido)) return;

            await _atendimentoVendidoRepository.Adicionar(atendimentoVendido);
        }

        public void Dispose()
        {
            _atendimentoVendidoRepository?.Dispose();
        }
    }
}
