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
    public class UsuarioProdutoConsultaApplication : BaseApplication, IUsuarioProdutoConsultaApplication
    {
        private readonly IUsuarioProdutoConsultaService _usuarioProdutoConsultaService;
        private readonly IUsuarioProdutoConsultaRepository _usuarioProdutoConsultaRepository;
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IMapper _mapper;

        public UsuarioProdutoConsultaApplication(
            IUsuarioProdutoConsultaService usuarioProdutoConsultaService,
            IUsuarioProdutoConsultaRepository usuarioProdutoConsultaRepository,
            IUsuarioApplication usuarioApplication,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _usuarioProdutoConsultaService = usuarioProdutoConsultaService;
            _usuarioProdutoConsultaRepository = usuarioProdutoConsultaRepository;
            _usuarioApplication = usuarioApplication;
            _mapper = mapper;
        }

        public async Task Adicionar(UsuarioProdutoConsultaViewModel usuarioProdutoConsultaViewModel)
        {
            var usuarioAuth = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioAuth == null)
            {
                Notificar("Não foi possível adicionar o item ao histórico de consultas.");

                return;
            }

            var usuarioProdutoConsulta = _mapper.Map<UsuarioProdutoConsulta>(usuarioProdutoConsultaViewModel);

            usuarioProdutoConsulta.UsuarioId = usuarioAuth.Id;
            usuarioProdutoConsulta.DataCadastro = DateTime.Now;

            await _usuarioProdutoConsultaService.Adicionar(usuarioProdutoConsulta);
        }

        public async Task<List<UsuarioProdutoConsultaViewModel>> ObterUltimasVinteConsultasPorUsuarioAuth()
        {
            var usuarioAuth = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioAuth == null) return null;

            return _mapper.Map<List<UsuarioProdutoConsultaViewModel>>(await _usuarioProdutoConsultaRepository.ObterUltimasVinteConsultasPorUsuario(usuarioAuth.Id));
        }

        public void Dispose()
        {
            _usuarioProdutoConsultaService?.Dispose();
            _usuarioProdutoConsultaRepository?.Dispose();
            _usuarioApplication?.Dispose();
        }
    }
}
