using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class RelUsuarioEmpresaApplication : BaseApplication, IRelUsuarioEmpresaApplication
    {
        private readonly IRelUsuarioEmpresaService _relUsuarioEmpresaService;
        private readonly IRelUsuarioEmpresaRepository _relUsuarioEmpresaRepository;
        private readonly IMapper _mapper;

        public RelUsuarioEmpresaApplication(IRelUsuarioEmpresaService relUsuarioEmpresaService,
                                            IRelUsuarioEmpresaRepository relUsuarioEmpresaRepository,
                                            IMapper mapper,
                                            INotificador notificador) : base(notificador)
        {
            _relUsuarioEmpresaService = relUsuarioEmpresaService;
            _relUsuarioEmpresaRepository = relUsuarioEmpresaRepository;
            _mapper = mapper;
        }

        public async Task<RelUsuarioEmpresaViewModel> Adicionar(RelUsuarioEmpresaViewModel relUsuarioEmpresaViewModel)
        {
            var relUsuarioEmpresa = _mapper.Map<RelUsuarioEmpresa>(relUsuarioEmpresaViewModel);

            await _relUsuarioEmpresaService.Adicionar(relUsuarioEmpresa);

            if (!OperacaoValida()) return null;

            relUsuarioEmpresaViewModel = await ObterRelUsuarioEmpresa(relUsuarioEmpresa.Id);

            if (relUsuarioEmpresaViewModel == null)
            {
                Notificar("Não foi possível adicionar o relacionamento entre o usuário e a empresa.");

                return null;
            }

            return relUsuarioEmpresaViewModel;
        }

        public async Task Remover(Guid id)
        {
            await _relUsuarioEmpresaService.Remover(id);
        }

        public async Task<RelUsuarioEmpresaViewModel> ObterRelUsuarioEmpresa(Guid id)
        {
            return _mapper.Map<RelUsuarioEmpresaViewModel>(await _relUsuarioEmpresaRepository.ObterRelUsuarioEmpresa(id));
        }

        public async Task<List<RelUsuarioEmpresaViewModel>> ObterRelUsuarioEmpresaPorUsuario(Guid usuarioId)
        {
            return _mapper.Map<List<RelUsuarioEmpresaViewModel>>(await _relUsuarioEmpresaRepository.ObterRelUsuarioEmpresaPorUsuario(usuarioId));
        }

        public async Task<List<RelUsuarioEmpresaViewModel>> ObterRelUsuarioEmpresaPorEmpresa(int empresaId)
        {
            return _mapper.Map<List<RelUsuarioEmpresaViewModel>>(await _relUsuarioEmpresaRepository.ObterRelUsuarioEmpresaPorEmpresa(empresaId));
        }

        public void Dispose()
        {
            _relUsuarioEmpresaService?.Dispose();
            _relUsuarioEmpresaRepository?.Dispose();
        }
    }
}
