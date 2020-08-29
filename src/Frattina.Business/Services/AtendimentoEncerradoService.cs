using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class AtendimentoEncerradoService : BaseService, IAtendimentoEncerradoService
    {
        private readonly IAtendimentoEncerradoRepository _atendimentoEncerradoRepository;

        public AtendimentoEncerradoService(IAtendimentoEncerradoRepository atendimentoEncerradoRepository,
            INotificador notificador) : base(notificador)
        {
            _atendimentoEncerradoRepository = atendimentoEncerradoRepository;
        }

        public async Task Adicionar(AtendimentoEncerrado atendimentoEncerrado)
        {
            atendimentoEncerrado.Data = DateTime.Now;
            atendimentoEncerrado.Atendimento = null;

            if (!ExecutarValidacao(new AtendimentoEncerradoValidation(), atendimentoEncerrado)) return;

            await _atendimentoEncerradoRepository.Adicionar(atendimentoEncerrado);
        }

        public void Dispose()
        {
            _atendimentoEncerradoRepository?.Dispose();
        }
    }
}
