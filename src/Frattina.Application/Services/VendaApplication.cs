using AutoMapper;
using Frattina.Application.Enums;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class VendaApplication : BaseApplication, IVendaApplication
    {
        private readonly IVendaSapService _vendaSapService;
        private readonly IVendedorSapService _vendedorSapService;
        private readonly IClienteApplication _clienteApplication;
        private readonly IAtendimentoApplication _atendimentoApplication;
        private readonly IMapper _mapper;

        public VendaApplication(
            IVendaSapService vendaSapService,
            IVendedorSapService vendedorSapService,
            IClienteApplication clienteApplication,
            IAtendimentoApplication atendimentoApplication,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _vendaSapService = vendaSapService;
            _vendedorSapService = vendedorSapService;
            _clienteApplication = clienteApplication;
            _atendimentoApplication = atendimentoApplication;
            _mapper = mapper;
        }

        public async Task<VendaSapViewModel> Adicionar(VendaSapViewModel vendaSapViewModel)
        {
            var clienteSapViewModel = await _clienteApplication.ObterCliente(vendaSapViewModel.ClienteId);
            if (clienteSapViewModel == null)
            {
                Notificar("Não foi possível obter os dados do cliente.");
                return null;
            }

            if (!await ClienteEstaAptoParaComprar(clienteSapViewModel))
            {
                Notificar("Para efetivar a venda será necessário adicionar algumas informações do cliente, verifique se o CPF/CNPJ e o endereço estão corretos.");
                return null;
            }

            await AtualizarTipoClienteParaConsumidor(clienteSapViewModel);

            if (!OperacaoValida()) return null;

            var vendaSap = await _vendaSapService.Adicionar(_mapper.Map<VendaSap>(vendaSapViewModel));

            if (!OperacaoValida()) return null;

            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimento(vendaSapViewModel.AtendimentoId);

            if (atendimentoViewModel == null)
            {
                Notificar("Não foi possível obter os dados do atendimento.");
                return null;
            }

            atendimentoViewModel.Vendido = new AtendimentoVendidoViewModel
            {
                VendaCodigo = vendaSap.DocEntry
            };

            await _atendimentoApplication.Vender(atendimentoViewModel);

            return _mapper.Map<VendaSapViewModel>(vendaSap);
        }

        public async Task<VendaSapViewModel> ObterVenda(int id)
        {
            var vendaViewModel = _mapper.Map<VendaSapViewModel>(await _vendaSapService.ObterVenda(id));

            return await PreencherVendedor(vendaViewModel);
        }

        public async Task<VendaSapViewModel> ObterPorNumeroNf(int numeroNf)
        {
            var vendaViewModel = _mapper.Map<VendaSapViewModel>(await _vendaSapService.ObterPorDocNum(numeroNf));

            return await PreencherVendedor(vendaViewModel);
        }

        public async Task<List<VendaSapViewModel>> ObterPorCliente(string clienteId)
        {
            var vendasViewModel = _mapper.Map<List<VendaSapViewModel>>(await _vendaSapService.ObterPorCardName(clienteId));

            foreach (var vendaViewModel in vendasViewModel)
            {
                await PreencherVendedor(vendaViewModel);
            }

            return vendasViewModel;
        }

        public async Task<List<VendaSapViewModel>> ObterPorVendedor(int vendedorId)
        {
            var vendasViewModel = _mapper.Map<List<VendaSapViewModel>>(await _vendaSapService.ObterPorSalesPersonCode(vendedorId));

            foreach (var vendaViewModel in vendasViewModel)
            {
                await PreencherVendedor(vendaViewModel);
            }

            return vendasViewModel;
        }

        public async Task<List<VendaSapViewModel>> ObterPorData(DateTime dataEmissaoIni, DateTime dataEmissaoFim)
        {
            var vendasViewModel = _mapper.Map<List<VendaSapViewModel>>(await _vendaSapService.ObterPorDocDate(dataEmissaoIni, dataEmissaoFim));

            foreach (var vendaViewModel in vendasViewModel)
            {
                await PreencherVendedor(vendaViewModel);
            }

            return vendasViewModel;
        }

        public Task<bool> ClienteEstaAptoParaComprar(ClienteSapViewModel cliente)
        {
            if (cliente.TipoPessoa == "PF" && cliente.DadosFiscais.Cpf == null) return Task.FromResult(false);

            if (cliente.TipoPessoa == "PJ" && cliente.DadosFiscais.Cnpj == null) return Task.FromResult(false);

            return Task.FromResult(cliente.Enderecos.Any());
        }

        private async Task AtualizarTipoClienteParaConsumidor(ClienteSapViewModel clienteSapViewModel)
        {
            if (clienteSapViewModel.Tipo == ClienteTipo.CONVIDADO)
            {
                clienteSapViewModel.Tipo = ClienteTipo.CLIENTE;

                await _clienteApplication.Atualizar(clienteSapViewModel);
            }
        }

        private async Task<VendaSapViewModel> PreencherVendedor(VendaSapViewModel vendaSapViewModel)
        {
            if (vendaSapViewModel == null) return vendaSapViewModel;

            vendaSapViewModel.Vendedor = _mapper.Map<VendedorSapViewModel>(await _vendedorSapService.ObterVendedor(vendaSapViewModel.VendedorId));

            return vendaSapViewModel;
        }

        public void Dispose()
        {
            _vendaSapService?.Dispose();
        }
    }
}
