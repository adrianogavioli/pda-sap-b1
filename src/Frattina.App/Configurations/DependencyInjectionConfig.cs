using Frattina.App.Areas.Identity.Services;
using Frattina.App.Dashboard;
using Frattina.Application.Interfaces;
using Frattina.Application.Services;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Notificacoes;
using Frattina.Business.Services;
using Frattina.CrossCutting.Email;
using Frattina.CrossCutting.UsuarioIdentity;
using Frattina.Data.Context;
using Frattina.Data.Repositories;
using Frattina.SapService.Interfaces;
using Frattina.SapService.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Frattina.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            // Geral
            services.AddScoped<FrattinaDbContext>();
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IUsuarioIdentityService, UsuarioIdentityService>();
            services.AddScoped<IElementDashboard, ElementDashboard>();
            services.AddTransient<IEmailSender, EmailSenderIdentity>();
            services.AddTransient<EmailSenderApp>();
            services.AddTransient<HttpClient>();

            // Applications
            services.AddScoped<IAutenticacaoApplication, AutenticacaoApplication>();
            services.AddScoped<ICargoApplication, CargoApplication>();
            services.AddScoped<IClienteApplication, ClienteApplication>();
            services.AddScoped<IClienteAtendimentoApplication, ClienteAtendimentoApplication>();
            services.AddScoped<IClienteGrupoApplication, ClienteGrupoApplication>();
            services.AddScoped<IVendedorApplication, VendedorApplication>();
            services.AddScoped<IProdutoApplication, ProdutoApplication>();
            services.AddScoped<IProdutoGrupoApplication, ProdutoGrupoApplication>();
            services.AddScoped<IProdutoTipoApplication, ProdutoTipoApplication>();
            services.AddScoped<IProdutoMarcaApplication, ProdutoMarcaApplication>();
            services.AddScoped<IProdutoModeloApplication, ProdutoModeloApplication>();
            services.AddScoped<IProdutoFotoApplication, ProdutoFotoApplication>();
            services.AddScoped<IProdutoEstoqueApplication, ProdutoEstoqueApplication>();
            services.AddScoped<IProdutoPrecoApplication, ProdutoPrecoApplication>();
            services.AddScoped<IProdutoFichaTecnicaApplication, ProdutoFichaTecnicaApplication>();
            services.AddScoped<IEmpresaApplication, EmpresaApplication>();
            services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            services.AddScoped<IAtendimentoApplication, AtendimentoApplication>();
            services.AddScoped<IVendaApplication, VendaApplication>();
            services.AddScoped<IVendaApplication, VendaApplication>();
            services.AddScoped<IAuditoriaApplication, AuditoriaApplication>();
            services.AddScoped<IRelUsuarioEmpresaApplication, RelUsuarioEmpresaApplication>();
            services.AddScoped<IUsuarioProdutoConsultaApplication, UsuarioProdutoConsultaApplication>();
            services.AddScoped<IEstoqueApplication, EstoqueApplication>();

            // Services Frattina
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAtendimentoService, AtendimentoService>();
            services.AddScoped<IAtendimentoProdutoService, AtendimentoProdutoService>();
            services.AddScoped<IAtendimentoTarefaService, AtendimentoTarefaService>();
            services.AddScoped<IAtendimentoEncerradoService, AtendimentoEncerradoService>();
            services.AddScoped<IAtendimentoVendidoService, AtendimentoVendidoService>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<IRelUsuarioEmpresaService, RelUsuarioEmpresaService>();
            services.AddScoped<IUsuarioProdutoConsultaService, UsuarioProdutoConsultaService>();

            // Repositories
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();
            services.AddScoped<IAtendimentoProdutoRepository, AtendimentoProdutoRepository>();
            services.AddScoped<IAtendimentoTarefaRepository, AtendimentoTarefaRepository>();
            services.AddScoped<IAtendimentoEncerradoRepository, AtendimentoEncerradoRepository>();
            services.AddScoped<IAtendimentoVendidoRepository, AtendimentoVendidoRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IRelUsuarioEmpresaRepository, RelUsuarioEmpresaRepository>();
            services.AddScoped<IUsuarioProdutoConsultaRepository, UsuarioProdutoConsultaRepository>();

            // Services SAP
            services.AddScoped<ISapBaseService, SapBaseService>();
            services.AddScoped<IAutenticacaoSapService, AutenticacaoSapService>();
            services.AddScoped<IClienteGrupoSapService, ClienteGrupoSapService>();
            services.AddScoped<IClienteSapService, ClienteSapService>();
            services.AddScoped<IEmpresaSapService, EmpresaSapService>();
            services.AddScoped<IProdutoSapService, ProdutoSapService>();
            services.AddScoped<IProdutoGrupoSapService, ProdutoGrupoSapService>();
            services.AddScoped<IProdutoTipoSapService, ProdutoTipoSapService>();
            services.AddScoped<IProdutoMarcaSapService, ProdutoMarcaSapService>();
            services.AddScoped<IProdutoModeloSapService, ProdutoModeloSapService>();
            services.AddScoped<IProdutoFotoSapService, ProdutoFotoSapService>();
            services.AddScoped<IProdutoEstoqueSapService, ProdutoEstoqueSapService>();
            services.AddScoped<IProdutoPrecoSapService, ProdutoPrecoSapService>();
            services.AddScoped<IProdutoFichaTecnicaSapService, ProdutoFichaTecnicaSapService>();
            services.AddScoped<IUsuarioSapService, UsuarioSapService>();
            services.AddScoped<IVendaSapService, VendaSapService>();
            services.AddScoped<IVendedorSapService, VendedorSapService>();
            services.AddScoped<IEstoqueSapService, EstoqueSapService>();

            return services;
        }
    }
}
