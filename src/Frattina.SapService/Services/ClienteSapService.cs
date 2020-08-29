using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Services;
using Frattina.CrossCutting.Configuration;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Models;
using Frattina.SapService.Models.Enums;
using Frattina.SapService.Models.Validations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Services
{
    public class ClienteSapService : BaseService, IClienteSapService
    {
        private readonly ISapBaseService _sapBaseService;
        private readonly IAtendimentoRepository _atendimentoRepository;

        public ClienteSapService(ISapBaseService sapBaseService,
            IAtendimentoRepository atendimentoRepository,
            INotificador notificador) : base(notificador)
        {
            _sapBaseService = sapBaseService;
            _atendimentoRepository = atendimentoRepository;
        }

        private readonly string query = "?$select=CardCode,CardName,CardType,GroupCode,Phone1,Phone2,Cellular,EmailAddress,Series,AliasName,ShipToDefault,BilltoDefault,Fax,U_SAUDACAO,U_DATACASAMENTO,U_DATANASCIMENTO,U_FRTPODERCOMPRACODE,U_SEXO,U_CONJUGE,BPAddresses,ContactEmployees,BPFiscalTaxIDCollection";

        public async Task<ClienteSap> Adicionar(ClienteSap cliente)
        {
            cliente.CardType = CardTypeSap.cLid.ToString();
            cliente.Series = Convert.ToInt32(AppSettings.Current.SapSerieCliente);

            if (!ExecutarValidacao(new ClienteSapValidation(), cliente)) return null;

            var request = new HttpRequestMessage(HttpMethod.Post, "/b1s/v1/BusinessPartners");

            request.Content = new StringContent(JsonConvert.SerializeObject(cliente));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível adicionar o cliente. {erroResponse.error.message.value}");

                return null;
            }

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ClienteSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task Atualizar(ClienteSap cliente)
        {
            var clienteDb = await ObterCliente(cliente.CardCode);

            if (clienteDb == null)
            {
                Notificar("Não foi possível obter as informações do cliente.");
                return;
            }

            var codConjugeDb = clienteDb.U_CONJUGE;

            clienteDb.CardName = cliente.CardName;
            clienteDb.CardType = cliente.CardType;
            clienteDb.GroupCode = cliente.GroupCode;
            clienteDb.Phone1 = cliente.Phone1;
            clienteDb.Phone2 = cliente.Phone2;
            clienteDb.Cellular = cliente.Cellular;
            clienteDb.EmailAddress = cliente.EmailAddress;
            clienteDb.AliasName = cliente.AliasName;
            clienteDb.U_SAUDACAO = cliente.U_SAUDACAO;
            clienteDb.U_DATACASAMENTO = cliente.U_DATACASAMENTO;
            clienteDb.U_DATANASCIMENTO = cliente.U_DATANASCIMENTO;
            clienteDb.U_FRTPODERCOMPRACODE = cliente.U_FRTPODERCOMPRACODE;
            clienteDb.U_SEXO = cliente.U_SEXO;
            clienteDb.U_CONJUGE = cliente.U_CONJUGE;

            if (!ExecutarValidacao(new ClienteSapValidation(), clienteDb)) return;

            var clienteFiscalDb = clienteDb.BPFiscalTaxIDCollection.FirstOrDefault();

            var clienteFiscal = cliente.BPFiscalTaxIDCollection.FirstOrDefault();

            clienteFiscalDb.TaxId0 = clienteFiscal.TaxId0;
            clienteFiscalDb.TaxId1 = clienteFiscal.TaxId1;
            clienteFiscalDb.TaxId2 = clienteFiscal.TaxId2;
            clienteFiscalDb.TaxId3 = clienteFiscal.TaxId3;
            clienteFiscalDb.TaxId4 = clienteFiscal.TaxId4;
            clienteFiscalDb.TaxId5 = clienteFiscal.TaxId5;

            if (!ExecutarValidacao(new ClienteFiscalSapValidation(), clienteFiscalDb)) return;

            clienteFiscalDb.TaxId0 = FormatarCnpj(clienteFiscalDb.TaxId0);
            clienteFiscalDb.TaxId4 = FormatarCpf(clienteFiscalDb.TaxId4);

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{clienteDb.CardCode}')");

            request.Content = new StringContent(JsonConvert.SerializeObject(clienteDb));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível atualizar o cliente. {erroResponse.error.message.value}");
            }

            if (!string.IsNullOrEmpty(cliente.U_CONJUGE) && cliente.U_CONJUGE != codConjugeDb)
                await AtualizarConjugeReverso(cliente);

            else if (string.IsNullOrEmpty(cliente.U_CONJUGE) && !string.IsNullOrEmpty(codConjugeDb))
                await RemoverConjugeReverso(codConjugeDb);
        }

        public async Task Remover(string cardCode)
        {
            if (await ClientePossuiAtendimentos(cardCode))
            {
                Notificar("Este cliente não pode ser removido.");
                return;
            }

            var clienteDb = await ObterCliente(cardCode);

            if (!string.IsNullOrEmpty(clienteDb?.U_CONJUGE))
                await RemoverConjugeReverso(clienteDb.U_CONJUGE);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/b1s/v1/BusinessPartners('{cardCode}')");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível remover o cliente. {erroResponse.error.message.value}");
            }
        }

        public async Task AdicionarEndereco(ClienteEnderecoSap clienteEndereco)
        {
            clienteEndereco.AddressType = AddressType.bo_ShipTo.ToString();
            clienteEndereco.AddressName = "ENTREGA";

            if (!ExecutarValidacao(new ClienteEnderecoSapValidation(), clienteEndereco)) return;

            var enderecoCobranca = new ClienteEnderecoSap
            {
                AddressType = AddressType.bo_BillTo.ToString(),
                AddressName = "COBRANÇA",
                Block = clienteEndereco.Block,
                BPCode = clienteEndereco.BPCode,
                BuildingFloorRoom = clienteEndereco.BuildingFloorRoom,
                City = clienteEndereco.City,
                Country = clienteEndereco.Country,
                State = clienteEndereco.State,
                Street = clienteEndereco.Street,
                StreetNo = clienteEndereco.StreetNo,
                ZipCode = clienteEndereco.ZipCode
            };

            var enderecos = new ClienteEnderecoSapCollection
            {
                BPAddresses = new List<ClienteEnderecoSap>
                {
                    clienteEndereco,
                    enderecoCobranca
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{clienteEndereco.BPCode}')");
            request.Headers.Add("B1S-ReplaceCollectionsOnPatch", "true");
            request.Content = new StringContent(JsonConvert.SerializeObject(enderecos));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível adicionar o endereço. {erroResponse.error.message.value}");
            }
        }

        public async Task AtualizarEndereco(ClienteEnderecoSap clienteEndereco)
        {
            if (!ExecutarValidacao(new ClienteEnderecoSapValidation(), clienteEndereco)) return;

            var clienteDb = await ObterCliente(clienteEndereco.BPCode);

            if (clienteDb == null)
            {
                Notificar("Não foi possível obter as informações do cliente.");
                return;
            }

            var enderecoDb = clienteDb.BPAddresses.FirstOrDefault(e => e.RowNum == clienteEndereco.RowNum);

            if (enderecoDb == null)
            {
                Notificar("Não foi possível obter as informações do endereço.");
                return;
            }

            enderecoDb.Block = clienteEndereco.Block;
            enderecoDb.BuildingFloorRoom = clienteEndereco.BuildingFloorRoom;
            enderecoDb.City = clienteEndereco.City;
            enderecoDb.Country = clienteEndereco.Country;
            enderecoDb.State = clienteEndereco.State;
            enderecoDb.Street = clienteEndereco.Street;
            enderecoDb.StreetNo = clienteEndereco.StreetNo;
            enderecoDb.ZipCode = clienteEndereco.ZipCode;

            var enderecos = new ClienteEnderecoSapCollection
            {
                BPAddresses = clienteDb.BPAddresses.ToList()
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{clienteEndereco.BPCode}')");
            request.Headers.Add("B1S-ReplaceCollectionsOnPatch", "true");
            request.Content = new StringContent(JsonConvert.SerializeObject(enderecos));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível atualizar o endereço. {erroResponse.error.message.value}");
            }
        }

        public async Task AdicionarContato(ClienteContatoSap clienteContato)
        {
            if (!ExecutarValidacao(new ClienteContatoSapValidation(), clienteContato)) return;

            var clienteDb = await ObterCliente(clienteContato.CardCode);

            if (clienteDb == null)
            {
                Notificar("Não foi possível obter as informações do cliente.");
                return;
            }

            var contatos = new ClienteContatoSapCollection
            {
                ContactEmployees = new List<ClienteContatoSap>()
            };

            contatos.ContactEmployees.AddRange(clienteDb.ContactEmployees);
            contatos.ContactEmployees.Add(clienteContato);

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{clienteContato.CardCode}')");
            request.Headers.Add("B1S-ReplaceCollectionsOnPatch", "true");
            request.Content = new StringContent(JsonConvert.SerializeObject(contatos));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível adicionar o contato. {erroResponse.error.message.value}");
            }
        }

        public async Task AtualizarContato(ClienteContatoSap clienteContato)
        {
            if (!ExecutarValidacao(new ClienteContatoSapValidation(), clienteContato)) return;

            var clienteDb = await ObterCliente(clienteContato.CardCode);

            if (clienteDb == null)
            {
                Notificar("Não foi possível obter as informações do cliente.");
                return;
            }

            var contatoDb = clienteDb.ContactEmployees.FirstOrDefault(c => c.InternalCode == clienteContato.InternalCode);

            if (contatoDb == null)
            {
                Notificar("Não foi possível obter as informações do contato.");
                return;
            }

            contatoDb.Name = clienteContato.Name;
            contatoDb.Phone1 = clienteContato.Phone1;
            contatoDb.Phone2 = clienteContato.Phone2;
            contatoDb.MobilePhone = clienteContato.MobilePhone;
            contatoDb.E_Mail = clienteContato.E_Mail;
            contatoDb.Remarks1 = clienteContato.Remarks1;
            contatoDb.FirstName = clienteContato.FirstName;
            contatoDb.LastName = clienteContato.LastName;

            var contatos = new ClienteContatoSapCollection
            {
                ContactEmployees = clienteDb.ContactEmployees.ToList()
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{clienteContato.CardCode}')");
            request.Headers.Add("B1S-ReplaceCollectionsOnPatch", "true");
            request.Content = new StringContent(JsonConvert.SerializeObject(contatos));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível atualizar o contato. {erroResponse.error.message.value}");
            }
        }

        public async Task RemoverContato(string cardCode, int internalCode)
        {
            var clienteDb = await ObterCliente(cardCode);

            if (clienteDb == null)
            {
                Notificar("Não foi possível obter as informações do cliente.");
                return;
            }

            var contatos = new ClienteContatoSapCollection
            {
                ContactEmployees = clienteDb.ContactEmployees.ToList()
            };

            contatos.ContactEmployees.RemoveAll(c => c.InternalCode == internalCode);

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{cardCode}')");
            request.Headers.Add("B1S-ReplaceCollectionsOnPatch", "true");
            request.Content = new StringContent(JsonConvert.SerializeObject(contatos));

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response))
            {
                var erroResponse = await _sapBaseService.ObterErroResponse(response);

                Notificar($"Não foi possível remover o contato. {erroResponse.error.message.value}");
            }
        }

        public async Task<ClienteSap> ObterCliente(string cardCode)
        {
            var filter = "&$filter=CardType ne 'cSupplier'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPartners('{cardCode}'){query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ClienteSap>(response);

            return responseResult.FirstOrDefault();
        }

        public async Task<ClienteSap> ObterPorCPF(string cpf)
        {
            var crossJoin = $"$crossjoin(BusinessPartners,BusinessPartners/BPFiscalTaxIDCollection)" +
                            $"?$expand=BusinessPartners($select=CardCode as CardCode)" +
                            $"&$filter=BusinessPartners/CardCode eq BusinessPartners/BPFiscalTaxIDCollection/BPCode and BusinessPartners/BPFiscalTaxIDCollection/TaxId4 eq '{ FormatarCpf(cpf) }'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{crossJoin}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ClienteSapCrossJoin>(response);

            if (responseResult.Count() == 0) return null;

            var clienteCrossJoin  = responseResult.FirstOrDefault();

            return await ObterCliente(clienteCrossJoin.BusinessPartners.CardCode);
        }

        public async Task<ClienteSap> ObterPorCNPJ(string cnpj)
        {
            var crossJoin = $"$crossjoin(BusinessPartners,BusinessPartners/BPFiscalTaxIDCollection)" +
                            $"?$expand=BusinessPartners($select=CardCode as CardCode)" +
                            $"&$filter=BusinessPartners/CardCode eq BusinessPartners/BPFiscalTaxIDCollection/BPCode and BusinessPartners/BPFiscalTaxIDCollection/TaxId0 eq '{ FormatarCnpj(cnpj) }'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/{crossJoin}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            var responseResult = await _sapBaseService.ResolverResultadoResponse<ClienteSapCrossJoin>(response);

            if (responseResult.Count() == 0) return null;

            var clienteCrossJoin = responseResult.FirstOrDefault();

            return await ObterCliente(clienteCrossJoin.BusinessPartners.CardCode);
        }

        public async Task<IEnumerable<ClienteSap>> ObterPorCardName(string cardName)
        {
            var filter = $"&$filter=CardType ne 'cSupplier' and CardName eq '{cardName}'";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPartners{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ClienteSap>(response);
        }

        public async Task<IEnumerable<ClienteSap>> ObterPorPartCardName(string cardName)
        {
            var filter = $"&$filter=CardType ne 'cSupplier' and contains(CardName, '{cardName}')";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/b1s/v1/BusinessPartners{query}{filter}");

            var response = await _sapBaseService.EnviarRequest(request);

            if (await _sapBaseService.ResponseContemErro(response)) return null;

            return await _sapBaseService.ResolverResultadoResponse<ClienteSap>(response);
        }

        private async Task<bool> ClientePossuiAtendimentos(string cardCode)
        {
            var atendimentos = await _atendimentoRepository.Buscar(a => a.ClienteId == cardCode);

            return atendimentos.Any();
        }

        private async Task AtualizarConjugeReverso(ClienteSap cliente)
        {
            var conjugeDb = await ObterCliente(cliente.U_CONJUGE);

            if (conjugeDb == null) return;

            conjugeDb.U_CONJUGE = cliente.CardCode;
            conjugeDb.U_SAUDACAO = cliente.U_SAUDACAO;
            conjugeDb.U_DATACASAMENTO = cliente.U_DATACASAMENTO;

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{conjugeDb.CardCode}')");

            request.Content = new StringContent(JsonConvert.SerializeObject(conjugeDb));

            await _sapBaseService.EnviarRequest(request);
        }

        private async Task RemoverConjugeReverso(string codConjugeDb)
        {
            var conjugeDb = await ObterCliente(codConjugeDb);

            if (conjugeDb == null) return;

            conjugeDb.U_CONJUGE = null;
            conjugeDb.U_SAUDACAO = null;
            conjugeDb.U_DATACASAMENTO = null;

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/b1s/v1/BusinessPartners('{conjugeDb.CardCode}')");

            request.Content = new StringContent(JsonConvert.SerializeObject(conjugeDb));

            await _sapBaseService.EnviarRequest(request);
        }

        public string FormatarCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return string.Empty;

            if (cpf.Contains(".")) return cpf;

            return string.Format(@"{0:000\.000\.000\-00}", Convert.ToUInt64(cpf));
        }

        public string FormatarCnpj(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj)) return string.Empty;

            if (cnpj.Contains(".")) return cnpj;

            return string.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToUInt64(cnpj));
        }

        public void Dispose()
        {
            _sapBaseService?.Dispose();
        }
    }
}
