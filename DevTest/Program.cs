using System;
using System.Collections.Generic;
using System.Text;
using ML.FichaTecnica.Services;

namespace DevTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Console.WriteLine("4586 {0}", HumanFriendlyInteger.IntegerToWritten(4586));

            var numeros = new List<int>
            {
                0, 1, 2, 5, 8, 12, 15, 18, 20, 21,22,23,29, 30, 40, 42, 46, 98, 100, 101,111, 200,222, 300,343,400, 404,411,421,431,441, 444,451,461, 471, 
                481, 491,500,508, 600, 666, 700, 745, 800,816,900, 901,915, 999, 1000, 1001, 1010, 1100,
                1203, 1220, 1224, 10000, 20000, 20123, 
                21312, 45456, 53135,
                1000000, 1111111, 1180000, 1180400, 1180430,
                1200000, 1250000, 2000000, 12000000, 12345678, -3
            };
            numeros.ForEach(x => Console.WriteLine("{0} {1}", x, NumerosLeibles.Int2Espanol(x)));
        }
    }

    
}
