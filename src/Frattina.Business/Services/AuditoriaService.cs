using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaService(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Adicionar(Auditoria auditoria)
        {
            await _auditoriaRepository.Adicionar(auditoria);
        }

        public void Dispose()
        {
            _auditoriaRepository?.Dispose();
        }
    }
}
