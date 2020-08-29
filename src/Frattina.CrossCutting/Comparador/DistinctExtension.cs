using System;
using System.Collections.Generic;
using System.Linq;

namespace Frattina.CrossCutting.Comparador
{
    public static class DistinctExtension
    {
        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, bool> metodoEquals,
            Func<TSource, int> metodoGetHashCode)
                => source.Distinct(
                    ComparadorGenerico<TSource>.Criar(
                        metodoEquals,
                        metodoGetHashCode)
                        );
    }
}
