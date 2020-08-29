using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.CrossCutting.Matematica;
using Frattina.CrossCutting.StringTools;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ClienteApplication : BaseApplication, IClienteApplication
    {
        private readonly IClienteSapService _clienteSapService;
        private readonly IClienteGrupoApplication _clienteGrupoApplication;
        private readonly IClienteAtendimentoApplication _clienteAtendimentoApplication;
        private readonly IMapper _mapper;

        public ClienteApplication(
            IClienteSapService clienteSapService,
            IClienteGrupoApplication clienteGrupoApplication,
            IClienteAtendimentoApplication clienteAtendimentoApplication,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _clienteSapService = clienteSapService;
            _clienteGrupoApplication = clienteGrupoApplication;
            _clienteAtendimentoApplication = clienteAtendimentoApplication;
            _mapper = mapper;
        }

        public async Task<ClienteSapViewModel> Adicionar(ClienteSapViewModel clienteSapViewModel)
        {
            var clienteSap = _mapper.Map<ClienteSap>(clienteSapViewModel);

            ClienteAtendimentoApplication.RemoverMascarasCliente(clienteSap);

            clienteSap = await _clienteSapService.Adicionar(clienteSap);

            return _mapper.Map<ClienteSapViewModel>(clienteSap);
        }

        public async Task Atualizar(ClienteSapViewModel clienteSapViewModel)
        {
            if (!string.IsNullOrEmpty(clienteSapViewModel.ConjugeNome))
            {
                if (!await ConjugeEhValido(clienteSapViewModel)) return;
            }

            var clienteSap = _mapper.Map<ClienteSap>(clienteSapViewModel);

            ClienteAtendimentoApplication.RemoverMascarasCliente(clienteSap);
            ClienteAtendimentoApplication.RemoverMascarasDadosFiscais(clienteSap.BPFiscalTaxIDCollection.FirstOrDefault());

            await _clienteSapService.Atualizar(clienteSap);
        }

        public async Task Remover(string id)
        {
            await _clienteSapService.Remover(id);
        }

        public async Task AdicionarEndereco(ClienteEnderecoSapViewModel clienteEnderecoSapViewModel)
        {
            var clienteEnderecoSap = _mapper.Map<ClienteEnderecoSap>(clienteEnderecoSapViewModel);

            ClienteAtendimentoApplication.RemoverMascarasEndereco(clienteEnderecoSap);

            await _clienteSapService.AdicionarEndereco(clienteEnderecoSap);
        }

        public async Task AtualizarEndereco(ClienteEnderecoSapViewModel clienteEnderecoSapViewModel)
        {
            var clienteEnderecoSap = _mapper.Map<ClienteEnderecoSap>(clienteEnderecoSapViewModel);

            ClienteAtendimentoApplication.RemoverMascarasEndereco(clienteEnderecoSap);

            await _clienteSapService.AtualizarEndereco(clienteEnderecoSap);
        }

        public async Task AdicionarContato(ClienteContatoSapViewModel clienteContatoSapViewModel)
        {
            var clienteContatoSap = _mapper.Map<ClienteContatoSap>(clienteContatoSapViewModel);

            ClienteAtendimentoApplication.RemoverMascarasContato(clienteContatoSap);

            await _clienteSapService.AdicionarContato(clienteContatoSap);
        }

        public async Task AtualizarContato(ClienteContatoSapViewModel clienteContatoSapViewModel)
        {
            var clienteContatoSap = _mapper.Map<ClienteContatoSap>(clienteContatoSapViewModel);

            ClienteAtendimentoApplication.RemoverMascarasContato(clienteContatoSap);

            await _clienteSapService.AtualizarContato(clienteContatoSap);
        }

        public async Task RemoverContato(string clienteId, int contatoId)
        {
            await _clienteSapService.RemoverContato(clienteId, contatoId);
        }

        public async Task<ClienteSapViewModel> ObterCliente(string id)
        {
            var clienteViewModel = _mapper.Map<ClienteSapViewModel>(await _clienteSapService.ObterCliente(id));

            if (clienteViewModel == null) return null;

            await PreencherConjuge(clienteViewModel);

            return clienteViewModel;
        }

        public async Task<ClienteSapViewModel> ObterPorCPF(string cpf)
        {
            return _mapper.Map<ClienteSapViewModel>(await _clienteSapService.ObterPorCPF(TratarTexto.SomenteNumeros(cpf)));
        }

        public async Task<ClienteSapViewModel> ObterPorCNPJ(string cnpj)
        {
            return _mapper.Map<ClienteSapViewModel>(await _clienteSapService.ObterPorCNPJ(TratarTexto.SomenteNumeros(cnpj)));
        }

        public async Task<List<ClienteSapViewModel>> ObterPorNome(string nome)
        {
            return _mapper.Map<List<ClienteSapViewModel>>(await _clienteSapService.ObterPorCardName(nome));
        }

        public async Task<List<ClienteSapViewModel>> ObterPorPartNome(string nome)
        {
            return _mapper.Map<List<ClienteSapViewModel>>(await _clienteSapService.ObterPorPartCardName(nome));
        }

        public async Task<ClienteVisaoViewModel> ObterVisao(string id)
        {
            var clienteVisaoViewModel = new ClienteVisaoViewModel
            {
                ClienteId = id
            };

            var cliente = await _clienteSapService.ObterCliente(id);

            if (cliente == null) return clienteVisaoViewModel;

            if (cliente.U_DATANASCIMENTO != null)
            {
                clienteVisaoViewModel.Idade = Calculos.CalcularIdade((DateTime)cliente.U_DATANASCIMENTO);

                clienteVisaoViewModel.ContagemDiasNiver = Calculos.CalcularQuantidadeDiasParaData((DateTime)cliente.U_DATANASCIMENTO);
            }

            await _clienteAtendimentoApplication.PopularVisaoClienteAtendimentos(clienteVisaoViewModel);

            await _clienteAtendimentoApplication.PopularVisaoClientePoderCompra(clienteVisaoViewModel);

            await _clienteAtendimentoApplication.PopularVisaoClienteProdutosAmados(clienteVisaoViewModel);

            return clienteVisaoViewModel;
        }

        private async Task<ClienteSapViewModel> PreencherConjuge(ClienteSapViewModel clienteViewModel)
        {
            if (!string.IsNullOrEmpty(clienteViewModel.ConjugeId))
            {
                var conjuge = await _clienteSapService.ObterCliente(clienteViewModel.ConjugeId);

                clienteViewModel.ConjugeNome = conjuge.CardName;
            }

            return clienteViewModel;
        }

        private async Task<bool> ConjugeEhValido(ClienteSapViewModel clienteSapViewModel)
        {
            var conjuges = await _clienteSapService.ObterPorCardName(clienteSapViewModel.ConjugeNome);

            if (conjuges == null)
            {
                Notificar("O conjuge informado não é válido, verifique se o mesmo possui cadastro.");
                return false;
            }

            var conjuge = conjuges.FirstOrDefault();

            if (clienteSapViewModel.Id == conjuge.CardCode)
            {
                Notificar("O cliente não pode ser conjuge dele mesmo, certo?");
                return false;
            }

            clienteSapViewModel.ConjugeId = conjuge.CardCode;

            return true;
        }

        public void Dispose()
        {
            _clienteSapService?.Dispose();
            _clienteGrupoApplication?.Dispose();
            _clienteAtendimentoApplication?.Dispose();
        }
    }
}
