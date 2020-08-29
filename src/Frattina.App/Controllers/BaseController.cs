using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Frattina.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificador _notificador;

        protected BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected List<string> ObterNotificacoes()
        {
            return _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToList();
        }

        protected void SearchFiltersFactory(List<SearchFilter> searchFilters)
        {
            SetSearchFilters(searchFilters);

            GetSearchFilters(searchFilters);
        }

        protected void CleanSearchFilters(List<SearchFilter> searchFilters)
        {
            foreach (var filter in searchFilters.Where(f => f.ValueFilter != null))
            {
                TempData.Remove(filter.KeyFilter);
                ViewData.Remove(filter.KeyFilter);
            }
        }

        private void SetSearchFilters(List<SearchFilter> searchFilters)
        {
            if (searchFilters.Any(f => f.ValueFilter != null))
            {
                foreach (var filter in searchFilters)
                {
                    TempData[filter.KeyFilter] = filter.ValueFilter;

                    if (filter.ValueFilter != null)
                        ViewData.Add(filter.KeyFilter, filter.ValueFilter);
                }
            }
        }

        private void GetSearchFilters(List<SearchFilter> searchFilters)
        {
            foreach (var filter in searchFilters)
            {
                if (filter.ValueFilter == null && TempData[filter.KeyFilter] != null)
                {
                    filter.ValueFilter = TempData[filter.KeyFilter];
                    TempData[filter.KeyFilter] = filter.ValueFilter;
                    ViewData.Add(filter.KeyFilter, filter.ValueFilter);
                }
            }
        }

        protected class SearchFilter
        {
            public string KeyFilter { get; set; }

            public object ValueFilter { get; set; }
        }
    }
}
