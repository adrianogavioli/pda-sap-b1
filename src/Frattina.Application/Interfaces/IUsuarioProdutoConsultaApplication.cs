using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IUsuarioProdutoConsultaApplication : IDisposable
    {
        Task Adicionar(UsuarioProdutoConsultaViewModel usuarioProdutoConsultaViewModel);

        Task<List<UsuarioProdutoConsultaViewModel>> ObterUltimasVinteConsultasPorUsuarioAuth();
    }
}
