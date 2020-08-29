namespace Frattina.CrossCutting.StringTools
{
    public class TratarTexto
    {
        public static string SomenteNumeros(string valor)
        {
            if (valor == null) return null;

            var onlyNumber = string.Empty;

            foreach (var s in valor)
            {
                if (char.IsDigit(s)) onlyNumber += s;
            }

            return onlyNumber.Trim();
        }
    }
}
