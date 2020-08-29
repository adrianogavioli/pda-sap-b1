using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.CrossCutting.JSonTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class AuditoriaApplication : IAuditoriaApplication
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IMapper _mapper;

        public AuditoriaApplication(
            IAuditoriaService auditoriaService,
            IAuditoriaRepository auditoriaRepository,
            IMapper mapper)
        {
            _auditoriaService = auditoriaService;
            _auditoriaRepository = auditoriaRepository;
            _mapper = mapper;
        }

        public async Task Adicionar(AuditoriaViewModel auditoriaViewModel)
        {
            await _auditoriaService.Adicionar(_mapper.Map<Auditoria>(auditoriaViewModel));
        }

        public async Task<AuditoriaViewModel> ObterAuditoria(Guid id)
        {
            var auditoriaViewModel = _mapper.Map<AuditoriaViewModel>(await _auditoriaRepository.ObterAuditoria(id));

            auditoriaViewModel.ValoresAntigos = Utils.DeserializarObj<Dictionary<string, string>>(auditoriaViewModel.ValorAntigo);

            auditoriaViewModel.ValoresAtuais = Utils.DeserializarObj<Dictionary<string, string>>(auditoriaViewModel.ValorAtual);

            await IdentificarAlteracoes(auditoriaViewModel);

            return auditoriaViewModel;
        }

        public async Task<List<AuditoriaViewModel>> ObterAuditorias(string tabela, Guid chave)
        {
            return _mapper.Map<List<AuditoriaViewModel>>(await _auditoriaRepository.ObterAuditorias(tabela, chave));
        }

        private Task IdentificarAlteracoes(AuditoriaViewModel auditoriaViewModel)
        {
            if (auditoriaViewModel.ValoresAntigos != null)
            {
                foreach (var valorAntigo in auditoriaViewModel.ValoresAntigos)
                {
                    var valorAtual = auditoriaViewModel.ValoresAtuais.FirstOrDefault(v => v.Key == valorAntigo.Key);

                    if (valorAtual.Value != valorAntigo.Value)
                        auditoriaViewModel.ValoresAtuais[valorAtual.Key] = $"#{valorAtual.Value}";
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _auditoriaService?.Dispose();
            _auditoriaRepository?.Dispose();
        }
    }
}
