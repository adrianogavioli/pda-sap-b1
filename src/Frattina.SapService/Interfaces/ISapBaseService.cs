using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface ISapBaseService : IDisposable
    {
        Task<HttpResponseMessage> EnviarRequest(HttpRequestMessage request);

        Task<bool> ResponseContemErro(HttpResponseMessage response);

        Task<SapError> ObterErroResponse(HttpResponseMessage response);

        Task<IEnumerable<T>> ResolverResultadoResponse<T>(HttpResponseMessage response);
    }
}
