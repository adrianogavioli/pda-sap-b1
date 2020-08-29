using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IVendaSapService : IDisposable
    {
        Task<VendaSap> Adicionar(VendaSap venda);

        Task<VendaSap> ObterVenda(int docEntry);

        Task<VendaSap> ObterPorDocNum(int docNum);

        Task<IEnumerable<VendaSap>> ObterPorCardName(string cardName);

        Task<IEnumerable<VendaSap>> ObterPorSalesPersonCode(int salesPersonCode);

        Task<IEnumerable<VendaSap>> ObterPorDocDate(DateTime docDateIni, DateTime docDateFim);
    }
}
