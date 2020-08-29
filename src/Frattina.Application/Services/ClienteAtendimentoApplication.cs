using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Business.Models.Enums;
using Frattina.CrossCutting.StringTools;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ClienteAtendimentoApplication : BaseApplication, IClienteAtendimentoApplication
    {
        private readonly IAtendimentoRepository _atendimentoRepository;
        private readonly IAtendimentoProdutoRepository _atendimentoProdutoRepository;
        private readonly IClienteSapService _clienteSapService;
        private readonly IMapper _mapper;

        public ClienteAtendimentoApplication(
            IAtendimentoRepository atendimentoRepository,
            IAtendimentoProdutoRepository atendimentoProdutoRepository,
            IClienteSapService clienteSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _atendimentoRepository = atendimentoRepository;
            _atendimentoProdutoRepository = atendimentoProdutoRepository;
            _clienteSapService = clienteSapService;
            _mapper = mapper;
        }

        public async Task<ClienteVisaoViewModel> PopularVisaoClienteAtendimentos(ClienteVisaoViewModel clienteVisaoViewModel)
        {
            var atendimentos = await _atendimentoRepository.Buscar(a => a.ClienteId == clienteVisaoViewModel.ClienteId);

            if (atendimentos == null || atendimentos.Count() == 0) return clienteVisaoViewModel;

            clienteVisaoViewModel.QuantidadeAtendimentos = atendimentos.Count();
            clienteVisaoViewModel.QuantidadeVendas = atendimentos.Count(a => a.Etapa == AtendimentoEtapa.Vendido);
            clienteVisaoViewModel.TaxaConversaoVenda = (clienteVisaoViewModel.QuantidadeVendas / clienteVisaoViewModel.QuantidadeAtendimentos) * 100;

            var ultimoAtendimento = atendimentos.OrderByDescending(a => a.Data).FirstOrDefault();
            clienteVisaoViewModel.DataUltimoAtendimento = ultimoAtendimento.Data;
            clienteVisaoViewModel.VendedorUltimoAtendimento = ultimoAtendimento.VendedorNome;

            return clienteVisaoViewModel;
        }

        public async Task<ClienteVisaoViewModel> PopularVisaoClientePoderCompra(ClienteVisaoViewModel clienteVisaoViewModel)
        {
            var atendimentoProdutos = await _atendimentoProdutoRepository.Buscar(p => p.Atendimento.ClienteId == clienteVisaoViewModel.ClienteId
                                                                                && p.Atendimento.Etapa == AtendimentoEtapa.Vendido
                                                                                && !p.RemovidoVenda);

            if (atendimentoProdutos == null || atendimentoProdutos.Count() == 0) return clienteVisaoViewModel;

            clienteVisaoViewModel.ValorCompras = atendimentoProdutos.Sum(p => p.ValorNegociado);

            clienteVisaoViewModel.TicketMedio = clienteVisaoViewModel.ValorCompras / atendimentoProdutos.Count();

            return clienteVisaoViewModel;
        }

        public async Task<ClienteVisaoViewModel> PopularVisaoClienteProdutosAmados(ClienteVisaoViewModel clienteVisaoViewModel)
        {
            clienteVisaoViewModel.ProdutosAmados = await ObterAtendimentoProdutosAmadosPorCliente(clienteVisaoViewModel.ClienteId, true);

            return clienteVisaoViewModel;
        }

        public async Task<AtendimentoViewModel> GerenciarClienteAtendimento(AtendimentoViewModel atendimentoViewModel)
        {
            var clienteSap = _clienteSapService.ObterPorCardName(atendimentoViewModel.ClienteNome).Result.FirstOrDefault();

            if (clienteSap == null)
                await AdicionarCliente(atendimentoViewModel);
            else
                await AtualizarCliente(atendimentoViewModel, _mapper.Map<ClienteSapViewModel>(clienteSap));

            return atendimentoViewModel;
        }

        public async Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosAmadosPorCliente(string clienteId, bool removerDuplicados)
        {
            var atendimentoProdutos = await _atendimentoProdutoRepository.ObterAtendimentoProdutosAmadosPorCliente(clienteId);

            if (removerDuplicados)
                atendimentoProdutos = await RemoverProdutosAmadosDuplicados(atendimentoProdutos);

            return _mapper.Map<List<AtendimentoProdutoViewModel>>(atendimentoProdutos);
        }

        private async Task<AtendimentoViewModel> AdicionarCliente(AtendimentoViewModel atendimentoViewModel)
        {
            var clienteSapViewModel = new ClienteSapViewModel
            {
                Nome = atendimentoViewModel.ClienteNome,
                Email = atendimentoViewModel.ClienteEmail,
                Telefone1 = atendimentoViewModel.ClienteTelefone,
                TipoPessoa = atendimentoViewModel.TipoPessoa,
                DataNascimento = atendimentoViewModel.ClienteNiver,
                Contribuinte = atendimentoViewModel.Contribuinte
            };

            var clienteSap = await _clienteSapService.Adicionar(_mapper.Map<ClienteSap>(clienteSapViewModel));

            if (clienteSap == null) return atendimentoViewModel;

            atendimentoViewModel.ClienteId = clienteSap.CardCode;

            return atendimentoViewModel;
        }

        private async Task<AtendimentoViewModel> AtualizarCliente(AtendimentoViewModel atendimentoViewModel, ClienteSapViewModel clienteSapViewModel)
        {
            var atualizarCliente = false;

            if (!string.IsNullOrEmpty(atendimentoViewModel.ClienteEmail))
            {
                clienteSapViewModel.Email = atendimentoViewModel.ClienteEmail;
                atualizarCliente = true;
            }

            if (atendimentoViewModel.ClienteNiver != null)
            {
                clienteSapViewModel.DataNascimento = atendimentoViewModel.ClienteNiver;
                atualizarCliente = true;
            }

            if (!string.IsNullOrEmpty(atendimentoViewModel.ClienteTelefone))
            {
                var clienteTelefone = TratarTexto.SomenteNumeros(atendimentoViewModel.ClienteTelefone);

                if (clienteTelefone != clienteSapViewModel.Telefone1
                    && clienteTelefone != clienteSapViewModel.Telefone2
                    && clienteTelefone != clienteSapViewModel.Telefone3)
                {
                    clienteSapViewModel.Telefone3 = clienteTelefone;
                    atualizarCliente = true;
                }
            }

            if (atendimentoViewModel.Contribuinte != clienteSapViewModel.Contribuinte)
            {
                clienteSapViewModel.Contribuinte = atendimentoViewModel.Contribuinte;
                atualizarCliente = true;
            }

            if (atualizarCliente)
            {
                var clienteSap = _mapper.Map<ClienteSap>(clienteSapViewModel);

                RemoverMascarasCliente(clienteSap);
                RemoverMascarasDadosFiscais(clienteSap.BPFiscalTaxIDCollection.FirstOrDefault());

                await _clienteSapService.Atualizar(clienteSap);
            }

            atendimentoViewModel.ClienteId = clienteSapViewModel.Id;
            atendimentoViewModel.ClienteEmail = clienteSapViewModel.Email;
            atendimentoViewModel.ClienteTelefone = clienteSapViewModel.Telefone2;
            atendimentoViewModel.ClienteNiver = clienteSapViewModel.DataNascimento;
            atendimentoViewModel.Contribuinte = clienteSapViewModel.Contribuinte;

            return atendimentoViewModel;
        }

        private Task<List<AtendimentoProduto>> RemoverProdutosAmadosDuplicados(IEnumerable<AtendimentoProduto> atendimentoProdutos)
        {
            var produtosAmados = new List<AtendimentoProduto>();

            atendimentoProdutos
                .Select(p => (p.ProdutoSapId, p.Imagem))
                .Distinct()
                .ToList()
                .ForEach(p =>
                    produtosAmados.Add(
                        new AtendimentoProduto
                        {
                            ProdutoSapId = p.ProdutoSapId,
                            Imagem = p.Imagem
                        }));

            return Task.FromResult(produtosAmados);
        }

        public static ClienteSap RemoverMascarasCliente(ClienteSap clienteSap)
        {
            clienteSap.Phone1 = TratarTexto.SomenteNumeros(clienteSap.Phone1);
            clienteSap.Phone2 = TratarTexto.SomenteNumeros(clienteSap.Phone2);
            clienteSap.Cellular = TratarTexto.SomenteNumeros(clienteSap.Cellular);

            return clienteSap;
        }

        public static ClienteFiscalSap RemoverMascarasDadosFiscais(ClienteFiscalSap clienteFiscalSap)
        {
            if (clienteFiscalSap != null)
            {
                clienteFiscalSap.TaxId0 = TratarTexto.SomenteNumeros(clienteFiscalSap.TaxId0);
                clienteFiscalSap.TaxId1 = TratarTexto.SomenteNumeros(clienteFiscalSap.TaxId1);
                clienteFiscalSap.TaxId2 = TratarTexto.SomenteNumeros(clienteFiscalSap.TaxId2);
                clienteFiscalSap.TaxId3 = TratarTexto.SomenteNumeros(clienteFiscalSap.TaxId3);
                clienteFiscalSap.TaxId4 = TratarTexto.SomenteNumeros(clienteFiscalSap.TaxId4);
            }

            return clienteFiscalSap;
        }

        public static ClienteEnderecoSap RemoverMascarasEndereco(ClienteEnderecoSap clienteEnderecoSap)
        {
            clienteEnderecoSap.ZipCode = TratarTexto.SomenteNumeros(clienteEnderecoSap.ZipCode);

            return clienteEnderecoSap;
        }

        public static ClienteContatoSap RemoverMascarasContato(ClienteContatoSap clienteContatoSap)
        {
            clienteContatoSap.Phone1 = TratarTexto.SomenteNumeros(clienteContatoSap.Phone1);
            clienteContatoSap.Phone2 = TratarTexto.SomenteNumeros(clienteContatoSap.Phone2);
            clienteContatoSap.MobilePhone = TratarTexto.SomenteNumeros(clienteContatoSap.MobilePhone);

            return clienteContatoSap;
        }

        public void Dispose()
        {
            _atendimentoRepository?.Dispose();
            _atendimentoProdutoRepository?.Dispose();
            _clienteSapService?.Dispose();
        }
    }
}
