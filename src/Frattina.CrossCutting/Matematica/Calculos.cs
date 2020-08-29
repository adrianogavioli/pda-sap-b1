using System;

namespace Frattina.CrossCutting.Matematica
{
    public static class Calculos
    {
        public static decimal CalcularDesconto(decimal valorCheio, decimal valorEfetivo)
        {
            if (valorCheio == 0) return 0;

            return ((valorCheio - valorEfetivo) / valorCheio) * 100;
        }

        public static decimal CalcularTaxaConversao(decimal valorEfetivo, decimal valorTotal)
        {
            if (valorTotal == 0) return 0;

            return (valorEfetivo / valorTotal) * 100;
        }

        public static int CalcularIdade(DateTime dataNascimento)
        {
            if (dataNascimento > DateTime.Now) return 0;

            var idade = DateTime.Now.Year - dataNascimento.Year;

            if (DateTime.Now.DayOfWeek < dataNascimento.DayOfWeek) idade--;

            return idade;
        }

        public static int CalcularQuantidadeDiasParaData(DateTime data)
        {
            var today = DateTime.Today;
            var next = data.AddYears(today.Year - data.Year);

            if (next < today)
                next = next.AddYears(1);

            return (next - today).Days;
        }
    }
}
