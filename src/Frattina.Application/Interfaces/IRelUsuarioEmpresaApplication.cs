using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IRelUsuarioEmpresaApplication : IDisposable
    {
        Task<RelUsuarioEmpresaViewModel> Adicionar(RelUsuarioEmpresaViewModel relUsuarioEmpresaViewModel);

        Task Remover(Guid id);

        Task<RelUsuarioEmpresaViewModel> ObterRelUsuarioEmpresa(Guid id);

        Task<List<RelUsuarioEmpresaViewModel>> ObterRelUsuarioEmpresaPorUsuario(Guid usuarioId);

        Task<List<RelUsuarioEmpresaViewModel>> ObterRelUsuarioEmpresaPorEmpresa(int empresaId);
    }
}
