using AutoMapper;
using Frattina.Application.Enums;
using Frattina.Application.ViewModels;
using Frattina.Business.Models;
using Frattina.CrossCutting.Configuration;
using Frattina.CrossCutting.UsuarioIdentity;
using Frattina.SapService.Models;
using Frattina.SapService.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Frattina.App.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            var SapGrupoClienteContribuinte = Convert.ToInt32(AppSettings.Current.SapGrupoClienteContribuinte);
            var SapGrupoClienteNaoContribuinte = Convert.ToInt32(AppSettings.Current.SapGrupoClienteNaoContribuinte);

            // Frattina
            CreateMap<Atendimento, AtendimentoViewModel>().ReverseMap();
            CreateMap<AtendimentoProduto, AtendimentoProdutoViewModel>().ReverseMap();
            CreateMap<AtendimentoTarefa, AtendimentoTarefaViewModel>().ReverseMap();
            CreateMap<AtendimentoEncerrado, AtendimentoEncerradoViewModel>().ReverseMap();
            CreateMap<AtendimentoVendido, AtendimentoVendidoViewModel>().ReverseMap();
            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
            CreateMap<IdentityUser, UsuarioIdentityViewModel>().ReverseMap();
            CreateMap<Claim, ClaimViewModel>().ReverseMap();
            CreateMap<Cargo, CargoViewModel>().ReverseMap();
            CreateMap<Auditoria, AuditoriaViewModel>().ReverseMap();
            CreateMap<RelUsuarioEmpresa, RelUsuarioEmpresaViewModel>().ReverseMap();
            CreateMap<UsuarioProdutoConsulta, UsuarioProdutoConsultaViewModel>().ReverseMap();

            // SAP
            CreateMap<UsuarioSap, UsuarioSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.InternalKey))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dst => dst.Bloqueado, opt => opt.MapFrom(src => src.Locked))
                .ReverseMap();

            CreateMap<ClienteSap, ClienteSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.CardCode))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.CardName))
                .ForMember(dst => dst.Tipo, opt => opt.MapFrom(src => (ClienteTipo)Enum.Parse(typeof(CardTypeSap), src.CardType)))
                .ForMember(dst => dst.Contribuinte, opt => opt.MapFrom(src => src.GroupCode == SapGrupoClienteContribuinte))
                .ForMember(dst => dst.Telefone1, opt => opt.MapFrom(src => src.Phone1))
                .ForMember(dst => dst.Telefone2, opt => opt.MapFrom(src => src.Phone2))
                .ForMember(dst => dst.Telefone3, opt => opt.MapFrom(src => src.Cellular))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dst => dst.SerieCodigo, opt => opt.MapFrom(src => src.Series))
                .ForMember(dst => dst.NomeFantasia, opt => opt.MapFrom(src => src.AliasName))
                .ForMember(dst => dst.IdentificacaoEnderecoEntrega, opt => opt.MapFrom(src => src.ShipToDefault))
                .ForMember(dst => dst.IdentificacaoEnderecoCobranca, opt => opt.MapFrom(src => src.BilltoDefault))
                .ForMember(dst => dst.TipoPessoa, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dst => dst.Saudacao, opt => opt.MapFrom(src => src.U_SAUDACAO))
                .ForMember(dst => dst.DataCasamento, opt => opt.MapFrom(src => src.U_DATACASAMENTO))
                .ForMember(dst => dst.DataNascimento, opt => opt.MapFrom(src => src.U_DATANASCIMENTO))
                .ForMember(dst => dst.PoderCompraId, opt => opt.MapFrom(src => src.U_FRTPODERCOMPRACODE))
                .ForMember(dst => dst.Genero, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.U_SEXO) ? null : (ClienteGenero?)Enum.Parse<SexoSap>(src.U_SEXO.ToUpper())))
                .ForMember(dst => dst.ConjugeId, opt => opt.MapFrom(src => src.U_CONJUGE))
                .ForMember(dst => dst.DadosFiscais, opt => opt.MapFrom(src => src.BPFiscalTaxIDCollection.FirstOrDefault()))
                .ForMember(dst => dst.Enderecos, opt => opt.MapFrom(src => src.BPAddresses))
                .ForMember(dst => dst.Contatos, opt => opt.MapFrom(src => src.ContactEmployees))
                .ReverseMap()
                .ForPath(src => src.U_SEXO, opt => opt.MapFrom(src => (SexoSap?)src.Genero))
                .ForPath(src => src.CardType, opt => opt.MapFrom(src => (CardTypeSap)src.Tipo))
                .ForPath(src => src.BPFiscalTaxIDCollection, opt => opt.MapFrom(src => new List<ClienteFiscalSapViewModel> { src.DadosFiscais }))
                .ForPath(src => src.GroupCode, opt => opt.MapFrom(src => src.Contribuinte ? SapGrupoClienteContribuinte : SapGrupoClienteNaoContribuinte));

            CreateMap<ClienteContatoSap, ClienteContatoSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.InternalCode))
                .ForMember(dst => dst.ClienteId, opt => opt.MapFrom(src => src.CardCode))
                .ForMember(dst => dst.Identificacao, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Telefone1, opt => opt.MapFrom(src => src.Phone1))
                .ForMember(dst => dst.Telefone2, opt => opt.MapFrom(src => src.Phone2))
                .ForMember(dst => dst.Telefone3, opt => opt.MapFrom(src => src.MobilePhone))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.E_Mail))
                .ForMember(dst => dst.Observacao, opt => opt.MapFrom(src => src.Remarks1))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.Sobrenome, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap();

            CreateMap<ClienteEnderecoSap, ClienteEnderecoSapViewModel>()
                .ForMember(dst => dst.ClienteId, opt => opt.MapFrom(src => src.BPCode))
                .ForMember(dst => dst.NumeroLinha, opt => opt.MapFrom(src => src.RowNum))
                .ForMember(dst => dst.Identificacao, opt => opt.MapFrom(src => src.AddressName))
                .ForMember(dst => dst.Logradouro, opt => opt.MapFrom(src => src.Street))
                .ForMember(dst => dst.Bairro, opt => opt.MapFrom(src => src.Block))
                .ForMember(dst => dst.Cep, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dst => dst.Cidade, opt => opt.MapFrom(src => src.City))
                .ForMember(dst => dst.Pais, opt => opt.MapFrom(src => src.Country))
                .ForMember(dst => dst.Estado, opt => opt.MapFrom(src => src.State))
                .ForMember(dst => dst.Complemento, opt => opt.MapFrom(src => src.BuildingFloorRoom))
                .ForMember(dst => dst.LogradouroNumero, opt => opt.MapFrom(src => src.StreetNo))
                .ReverseMap();

            CreateMap<ClienteFiscalSap, ClienteFiscalSapViewModel>()
                .ForMember(dst => dst.ClienteId, opt => opt.MapFrom(src => src.BPCode))
                .ForMember(dst => dst.Cnpj, opt => opt.MapFrom(src => src.TaxId0))
                .ForMember(dst => dst.InscricaoEstadual, opt => opt.MapFrom(src => src.TaxId1))
                .ForMember(dst => dst.InscricaoEstadualST, opt => opt.MapFrom(src => src.TaxId2))
                .ForMember(dst => dst.InscricaoMunicipal, opt => opt.MapFrom(src => src.TaxId3))
                .ForMember(dst => dst.Cpf, opt => opt.MapFrom(src => src.TaxId4))
                .ForMember(dst => dst.Passaporte, opt => opt.MapFrom(src => src.TaxId5))
                .ReverseMap();

            CreateMap<ClienteGrupoSap, ClienteGrupoSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Tipo, opt => opt.MapFrom(src => src.Type))
                .ReverseMap();

            CreateMap<VendedorSap, VendedorSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.SalesEmployeeCode))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.SalesEmployeeName))
                .ForMember(dst => dst.Bloqueado, opt => opt.MapFrom(src => src.Locked == "tYES" ? "SIM" : "NÃO"))
                .ReverseMap()
                .ForPath(src => src.Locked, opt => opt.MapFrom(src => src.Bloqueado == "SIM" ? "tYES" : "tNO"));

            CreateMap<ProdutoSap, ProdutoSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Items.ItemCode))
                .ForMember(dst => dst.Descricao, opt => opt.MapFrom(src => src.Items.ItemName))
                .ForMember(dst => dst.Referencia, opt => opt.MapFrom(src => src.Items.SupplierCatalogNo))
                .ForMember(dst => dst.CodigoMaster, opt => opt.MapFrom(src => src.Items.U_REFINTERNA))
                .ForMember(dst => dst.SemiNovo, opt => opt.MapFrom(src => src.Items.U_SEMI))
                .ForMember(dst => dst.Grupo, opt => opt.MapFrom(src => src.ItemGroups))
                .ForMember(dst => dst.Tipo, opt => opt.MapFrom(src => src.U_FRTTIPO))
                .ForMember(dst => dst.Marca, opt => opt.MapFrom(src => src.U_FRTMARCA))
                .ForMember(dst => dst.Modelo, opt => opt.MapFrom(src => src.U_FRTMODELO))
                .ForMember(dst => dst.Imagem, opt => opt.MapFrom(src => src.U_FRTFOTOITEM.U_IMG))
                .ReverseMap();

            CreateMap<ProdutoGrupoSap, ProdutoGrupoSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Number))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.GroupName))
                .ReverseMap();

            CreateMap<ProdutoTipoSap, ProdutoTipoSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<ProdutoMarcaSap, ProdutoMarcaSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<ProdutoModeloSap, ProdutoModeloSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<ProdutoFotoSap, ProdutoFotoSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.U_FRTFOTOITEM.Code))
                .ForMember(dst => dst.Imagem, opt => opt.MapFrom(src => src.U_FRTFOTOITEM.U_IMG))
                .ForMember(dst => dst.ProdutoId, opt => opt.MapFrom(src => src.U_FRTFOTOITEM.U_ITEMCODE))
                .ForMember(dst => dst.Principal, opt => opt.MapFrom(src => src.U_FRTFOTOITEM.U_PRINCIPAL == "Y"))
                .ForMember(dst => dst.Grupo, opt => opt.MapFrom(src => src.U_FRTGRUPOFOTOSITEM.Name))
                .ReverseMap()
                .ForPath(src => src.U_FRTFOTOITEM.U_PRINCIPAL, opt => opt.MapFrom(src => src.Principal == true ? "Y" : "N"));

            CreateMap<ProdutoEstoqueSap, ProdutoEstoqueSapViewModel>()
                .ForMember(dst => dst.Estoque, opt => opt.MapFrom(src => src.WarehouseCode))
                .ForMember(dst => dst.Quantidade, opt => opt.MapFrom(src => src.InStock))
                .ReverseMap();

            CreateMap<ProdutoPrecoSap, ProdutoPrecoSapViewModel>()
                .ForMember(dst => dst.Parcela, opt => opt.MapFrom(src => src.PriceList))
                .ForMember(dst => dst.Valor, opt => opt.MapFrom(src => src.Price == null ? 0 : src.Price))
                .ReverseMap();

            CreateMap<ProdutoFichaTecnicaSap, ProdutoFichaTecnicaSapViewModel>()
                .ForMember(dst => dst.CaracteristicaId, opt => opt.MapFrom(src => src.U_FRTCARACTERISTICA.Code))
                .ForMember(dst => dst.Caracteristica, opt => opt.MapFrom(src => src.U_FRTCARACTERISTICA.Name))
                .ForMember(dst => dst.DetalheId, opt => opt.MapFrom(src => src.U_FRTDETALHE.Code))
                .ForMember(dst => dst.Detalhe, opt => opt.MapFrom(src => src.U_FRTDETALHE.Name))
                .ForMember(dst => dst.Descricao, opt => opt.MapFrom(src => src.U_FRTCARACDETITEM.Name))
                .ForMember(dst => dst.UnidadeMedida, opt => opt.MapFrom(src => src.U_FRTCARACDETITEM.U_MEDIDA))
                .ForMember(dst => dst.Quantidade, opt => opt.MapFrom(src => src.U_FRTCARACDETITEM.U_VALOR))
                .ReverseMap();

            CreateMap<EmpresaSap, EmpresaSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.BPLID))
                .ForMember(dst => dst.RazaoSocial, opt => opt.MapFrom(src => src.BPLName))
                .ForMember(dst => dst.Inativa, opt => opt.MapFrom(src => src.Disabled == "tNO" ? "NÃO" : "SIM"))
                .ForMember(dst => dst.Cnpj, opt => opt.MapFrom(src => src.FederalTaxID))
                .ForMember(dst => dst.TipoEndereco, opt => opt.MapFrom(src => src.AddressType))
                .ForMember(dst => dst.Logradouro, opt => opt.MapFrom(src => src.AddressType + " " + src.Street))
                .ForMember(dst => dst.LogradouroNumero, opt => opt.MapFrom(src => src.StreetNo))
                .ForMember(dst => dst.Complemento, opt => opt.MapFrom(src => src.Building))
                .ForMember(dst => dst.Cep, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dst => dst.Bairro, opt => opt.MapFrom(src => src.Block))
                .ForMember(dst => dst.Cidade, opt => opt.MapFrom(src => src.City))
                .ForMember(dst => dst.Estado, opt => opt.MapFrom(src => src.State))
                .ForMember(dst => dst.Pais, opt => opt.MapFrom(src => src.Country))
                .ForMember(dst => dst.NomeFantasia, opt => opt.MapFrom(src => src.AliasName))
                .ReverseMap()
                .ForPath(src => src.Disabled, opt => opt.MapFrom(src => src.Inativa == "NÃO" ? "tNO" : "tYES"))
                .ForPath(src => src.Street, opt => opt.MapFrom(src => src.Logradouro));

            CreateMap<VendaSap, VendaSapViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.DocEntry))
                .ForMember(dst => dst.NumeroNf, opt => opt.MapFrom(src => src.DocNum))
                .ForMember(dst => dst.DataEmissao, opt => opt.MapFrom(src => src.DocDate))
                .ForMember(dst => dst.EmpresaId, opt => opt.MapFrom(src => src.BPL_IDAssignedToInvoice))
                .ForMember(dst => dst.EmpresaNome, opt => opt.MapFrom(src => src.BPLName))
                .ForMember(dst => dst.EmpresaCnpj, opt => opt.MapFrom(src => src.VATRegNum))
                .ForMember(dst => dst.ClienteId, opt => opt.MapFrom(src => src.CardCode))
                .ForMember(dst => dst.ClienteNome, opt => opt.MapFrom(src => src.CardName))
                .ForMember(dst => dst.ValorTotal, opt => opt.MapFrom(src => src.DocTotal))
                .ForMember(dst => dst.Moeda, opt => opt.MapFrom(src => src.DocCurrency))
                .ForMember(dst => dst.HoraEmissao, opt => opt.MapFrom(src => src.DocTime))
                .ForMember(dst => dst.VendedorId, opt => opt.MapFrom(src => src.SalesPersonCode))
                .ForMember(dst => dst.Produtos, opt => opt.MapFrom(src => src.DocumentLines))
                .ReverseMap();

            CreateMap<VendaItemSap, VendaItemSapViewModel>()
                .ForMember(dst => dst.NumeroLinha, opt => opt.MapFrom(src => src.LineNum))
                .ForMember(dst => dst.ProdutoId, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dst => dst.ProdutoDescricao, opt => opt.MapFrom(src => src.ItemDescription))
                .ForMember(dst => dst.Quantidade, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dst => dst.ValorTotal, opt => opt.MapFrom(src => src.Price))
                .ForMember(dst => dst.Moeda, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dst => dst.DescontoPercent, opt => opt.MapFrom(src => src.DiscountPercent))
                .ForMember(dst => dst.EstoqueCodigo, opt => opt.MapFrom(src => src.WarehouseCode))
                .ForMember(dst => dst.Cfop, opt => opt.MapFrom(src => src.CFOPCode))
                .ForMember(dst => dst.ValorUnitario, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dst => dst.VendaId, opt => opt.MapFrom(src => src.DocEntry))
                .ForMember(dst => dst.Ncm, opt => opt.MapFrom(src => src.NCMCode))
                .ForMember(dst => dst.CodigoImposto, opt => opt.MapFrom(src => src.TaxCode))
                .ForMember(dst => dst.UtilizacaoId, opt => opt.MapFrom(src => src.Usage))
                .ReverseMap();

            CreateMap<EstoqueSap, EstoqueSapViewModel>()
                .ForMember(dst => dst.Codigo, opt => opt.MapFrom(src => src.WarehouseCode))
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.WarehouseName))
                .ForMember(dst => dst.PermiteVenda, opt => opt.MapFrom(src => src.Nettable == "tYES"))
                .ForMember(dst => dst.Inativo, opt => opt.MapFrom(src => src.Inactive == "tYES"))
                .ForMember(dst => dst.EmpresaId, opt => opt.MapFrom(src => src.BusinessPlaceID))
                .ReverseMap()
                .ForPath(src => src.Nettable, opt => opt.MapFrom(src => src.PermiteVenda ? "tYES" : "tNO"))
                .ForPath(src => src.Inactive, opt => opt.MapFrom(src => src.Inativo ? "tYES" : "tNO"));
        }
    }
}
