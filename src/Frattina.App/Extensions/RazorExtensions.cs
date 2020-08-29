using Frattina.CrossCutting.Configuration;
using Frattina.CrossCutting.StringTools;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Frattina.App.Extensions
{
    public static class RazorExtensions
    {
        public static string HashEmailForGravatar(this RazorPage page, string email)
        {
            if (string.IsNullOrEmpty(email)) return string.Empty;

            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();

            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string FormataTelefone(this RazorPage page, string telefone)
        {
            telefone = TratarTexto.SomenteNumeros(telefone);

            if (string.IsNullOrEmpty(telefone)) return string.Empty;

            var mascara = "{0:(00) 0000-0000}";

            if (telefone.Length == 11) mascara = "{0:(00) 00000-0000}";

            return string.Format(mascara, Convert.ToUInt64(telefone));
        }

        public static string FormataMoeda(this RazorPage page, decimal? valor)
        {
            if (valor == null) return null;

            return Convert.ToDecimal(valor).ToString("c");
        }

        public static string FormataDecimal1Casa(this RazorPage page, decimal? valor)
        {
            if (valor == null) return null;

            return Convert.ToDecimal(valor).ToString("n1");
        }

        public static string FormataDecimal2Casas(this RazorPage page, decimal? valor)
        {
            if (valor == null) return null;

            return Convert.ToDecimal(valor).ToString("n2");
        }

        public static string FormataData(this RazorPage page, DateTime? data)
        {
            if (data == null) return null;

            return Convert.ToDateTime(data).ToShortDateString();
        }

        public static string FormataDataHora(this RazorPage page, DateTime? data)
        {
            if (data == null) return null;

            var dataFormat = Convert.ToDateTime(data);

            return $"{dataFormat.ToShortDateString()} {dataFormat.ToShortTimeString()}";
        }

        public static string FormataPercentual(this RazorPage page, decimal? percentual)
        {
            if (percentual == null) return null;

            return Convert.ToDecimal(percentual).ToString("n1") + " %";
        }

        public static string FormataCpf(this RazorPage page, string cpf)
        {
            cpf = TratarTexto.SomenteNumeros(cpf);

            if (string.IsNullOrEmpty(cpf)) return string.Empty;

            return string.Format(@"{0:000\.000\.000\-00}", Convert.ToUInt64(cpf));
        }

        public static string FormataCnpj(this RazorPage page, string cnpj)
        {
            cnpj = TratarTexto.SomenteNumeros(cnpj);

            if (string.IsNullOrEmpty(cnpj)) return string.Empty;

            return string.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToUInt64(cnpj));
        }

        public static string FormataCep(this RazorPage page, string cep)
        {
            cep = TratarTexto.SomenteNumeros(cep);

            if (string.IsNullOrEmpty(cep)) return string.Empty;

            return string.Format(@"{0:00000\-000}", Convert.ToUInt64(cep));
        }

        public static string CarregaImagemPadrao(this RazorPage page, string imagem)
        {
            return string.Concat(AppSettings.Current.SapImageDefaultPath, imagem);
        }

        public static string CarregaImagemMiniatura(this RazorPage page, string imagem)
        {
            return string.Concat(AppSettings.Current.SapImageThumbnailPath, imagem);
        }
    }
}
