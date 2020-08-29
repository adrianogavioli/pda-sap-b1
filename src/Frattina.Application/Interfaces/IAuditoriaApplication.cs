using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IAuditoriaApplication : IDisposable
    {
        Task Adicionar(AuditoriaViewModel auditoriaViewModel);

        Task<AuditoriaViewModel> ObterAuditoria(Guid id);

        Task<List<AuditoriaViewModel>> ObterAuditorias(string tabela, Guid chave);
    }
}
