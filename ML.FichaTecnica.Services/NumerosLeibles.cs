using System;
using System.Text;

namespace ML.FichaTecnica.Services
{
    public interface INumbersToLanguage
    {
        string Int2Espanol(string n);
        string Int2Espanol(int n);
    }
    public class NumerosLeiblesService  : INumbersToLanguage
    {
        private readonly string[] Unidades = new string[] { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" , "diez", "once", "doce",
            "trece", "catorce", "quince" };
        private readonly string[] Decenas = new string[] { "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };


        private string Espanol(int numero)
        {
            if (numero < 16)
                return Unidades[numero];
            if (numero < 20)
                return $"dieci{Espanol(numero - 10)}";
            if (numero == 20)
                return "veinte";
            if (numero < 30)
                return $"veinti{Espanol(numero - 20)}";
            //decenas
            switch (numero)
            {
                case 30:
                case 40:
                case 50:
                case 60:
                case 70:
                case 80:
                case 90:
                    return Decenas[numero / 10 - 3];
            }

            if (numero < 100) //entre 31 y 99 pero no termina en 0, 
            {
                Math.DivRem(numero, 10, out int resto10);
                return $"{Espanol(numero - resto10)} y {Espanol(resto10)}";
            }
            //cientos
            switch (numero)
            {
                case 100:
                    return "cien";
                case 200:
                case 300:
                case 400:
                    return $"{Espanol(numero / 100)}cientos";
                case 500:
                    return "quinientos";
                case 600:
                    return $"{Espanol(numero / 100)}cientos";
                case 700:
                    return "setecientos";
                case 800:
                    return $"{Espanol(numero / 100)}cientos";
                case 900:
                    return "novecientos";
                case 1000:
                    return "mil";
            }
            //entre 100 y 200
            if (numero < 200)
                return $"ciento {Espanol(numero - 100)}";

            //menor que mil, ignorado hasta ahora por ej doscientos + resto
            if (numero < 1000)
            {
                Math.DivRem(numero, 100, out int resto100);
                return $"{Espanol(numero - resto100)} {Espanol(resto100)}";
            }


            //entre mil y 2000, mil + resto
            if (numero < 2000)
                return $"mil {Espanol(numero % 1000)}";

            if (numero < 1000000)
            {
                //splitear en miles
                var result = new StringBuilder(Espanol(numero / 1000));
                //excepcion al 21mil
                if (result.ToString() == "veintiuno")
                    result = new StringBuilder("veintiun");
                result.Append(" mil");
                if ((numero % 1000) > 0)
                {
                    result.Append(" ");
                    result.Append(Espanol(numero % 1000));
                }

                return result.ToString();
            }

            //llegamos al millon
            if (numero == 1000000)
                return "un millón";
            //menos de 2: tengo que usar un millon , en vez de n millones
            if (numero < 2000000)
                return $"un millón {Espanol(numero % 1000000)}";

            //partir en millones
            var millones = new StringBuilder(Espanol(numero / 1000000));
            millones.Append(" millones");
            Math.DivRem(numero, 1000000, out int resto);
            if (resto > 0)
            {
                millones.Append(" ");
                millones.Append(Espanol(resto));
            }

            return millones.ToString();
        }

        /// <summary>
        /// Convierte un numero a palabras (2 = dos)
        /// Strings no convertibles, devuelve NaN
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string Int2Espanol(string n)
        {
            if (!int.TryParse(n, out int number))
                return "NaN";
            return Int2Espanol(number);
        }
        /// <summary>
        /// Convierte un numero a palabras (2 = dos)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string Int2Espanol(int n)
        {
            if (n < 0)
            {
                return "menos " + Int2Espanol(-n);
            }

            return Espanol(n);
        }
    }
}
