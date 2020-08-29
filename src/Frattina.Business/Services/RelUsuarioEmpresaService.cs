using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class RelUsuarioEmpresaService : BaseService, IRelUsuarioEmpresaService
    {
        private readonly IRelUsuarioEmpresaRepository _relUsuarioEmpresaRepository;

        public RelUsuarioEmpresaService(IRelUsuarioEmpresaRepository relUsuarioEmpresaRepository,
                                        INotificador notificador) : base(notificador)
        {
            _relUsuarioEmpresaRepository = relUsuarioEmpresaRepository;
        }

        public async Task Adicionar(RelUsuarioEmpresa relUsuarioEmpresa)
        {
            if (!ExecutarValidacao(new RelUsuarioEmpresaValidation(_relUsuarioEmpresaRepository), relUsuarioEmpresa)) return;

            await _relUsuarioEmpresaRepository.Adicionar(relUsuarioEmpresa);
        }

        public async Task Remover(Guid id)
        {
            var relUsuarioEmpresa = await _relUsuarioEmpresaRepository.ObterPorId(id);

            if (relUsuarioEmpresa == null)
            {
                Notificar("Não foi possível remover o relacionamento entre o usuário e a empresa.");

                return;
            }

            relUsuarioEmpresa.Removido = true;

            await _relUsuarioEmpresaRepository.Atualizar(relUsuarioEmpresa);
        }

        public void Dispose()
        {
            _relUsuarioEmpresaRepository?.Dispose();
        }
    }
}
