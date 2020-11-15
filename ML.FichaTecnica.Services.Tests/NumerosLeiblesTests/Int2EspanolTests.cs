using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ML.FichaTecnica.Services.Tests.NumerosLeiblesTests
{

    public class Int2EspanolTests
    {
        private List<int> _numeros;
        private List<string> _leibles;

        [SetUp]
        public void SetUp()
        {
            _numeros = new List<int>
            {
                0, 1, 2, 5, 8, 12, 15, 18, 20, 21,22,23,29, 30, 40, 42, 46, 98, 100, 101,111, 200,222, 300,343,400, 404,411,421,431,441, 444,451,461, 471,
                481, 491,500,508, 600, 666, 700, 745, 800,816,900, 901,915, 999, 1000, 1001, 1010, 1100,
                1203, 1220, 1224, 10000, 20000, 20123,
                21312, 45456, 53135,100000,
                1000000, 1111111, 1180000, 1180400, 1180430,
                1200000, 1250000, 2000000, 12000000, 12345678, -3
            };
            _leibles = new List<string>
            {
                "cero", "uno", "dos", "cinco", "ocho", "doce", "quince", "dieciocho", "veinte", "veintiuno", "veintidos", "veintitres", "veintinueve",
                "treinta", "cuarenta", "cuarenta y dos",
                "cuarenta y seis", "noventa y ocho", "cien", "ciento uno", "ciento once", "doscientos", "doscientos veintidos", "trescientos", "trescientos cuarenta y tres", "cuatrocientos", "cuatrocientos cuatro",
                "cuatrocientos once", "cuatrocientos veintiuno", "cuatrocientos treinta y uno", "cuatrocientos cuarenta y uno", "cuatrocientos cuarenta y cuatro", "cuatrocientos cincuenta y uno",
                "cuatrocientos sesenta y uno", "cuatrocientos setenta y uno", "cuatrocientos ochenta y uno", "cuatrocientos noventa y uno", "quinientos", "quinientos ocho",
                "seiscientos", "seiscientos sesenta y seis", "setecientos", "setecientos cuarenta y cinco", "ochocientos", "ochocientos dieciseis", "novecientos", "novecientos uno",
                "novecientos quince", "novecientos noventa y nueve", "mil", "mil uno", "mil diez", "mil cien", "mil doscientos tres", "mil doscientos veinte", "mil doscientos veinticuatro",
                "diez mil", "veinte mil", "veinte mil ciento veintitres", "veintiun mil trescientos doce", "cuarenta y cinco mil cuatrocientos cincuenta y seis", "cincuenta y tres mil ciento treinta y cinco",
                "cien mil", "un millón", "un millón ciento once mil ciento once", "un millón ciento ochenta mil", "un millón ciento ochenta mil cuatrocientos", "un millón ciento ochenta mil cuatrocientos treinta",
                "un millón doscientos mil", "un millón doscientos cincuenta mil", "dos millones", "doce millones", "doce millones trescientos cuarenta y cinco mil seiscientos setenta y ocho", 
                "menos tres"
            };
        }

        [Test]
        public void Success_Integers_Ok()
        {
            var svc = new NumerosLeiblesService();
            for (int index = 0; index < _numeros.Count; index++)
            {
                Assert.AreEqual(_leibles[index], svc.Int2Espanol(_numeros[index]));
            }
        }

        [Test]
        public void Success_Strings_Ok()
        {
            var svc = new NumerosLeiblesService();
            for (int index = 0; index < _numeros.Count; index++)
            {
                Assert.AreEqual(_leibles[index], svc.Int2Espanol(_numeros[index].ToString()));
            }
        }
    }
}
