using System;
using System.Collections.Generic;
using System.Linq;

namespace Metodos.Extensores.Genericidad
{
    internal delegate bool Predicado<T>(T x);

    internal static class MisMetodos
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> items, Predicado<T> filtro)
        {
            foreach (T x in items)
                if (filtro(x)) yield return x;
        }

        public static T Mayor<T>(this IEnumerable<T> items) where T : IComparable
        {
            T max = default(T);
            bool empty = true;
            foreach (T x in items)
            {
                max = x; empty = false; break;
            }
            if (empty) throw new Exception("The source cannot be empty");
            foreach (T x in items)
            {
                if (x.CompareTo(max) > 0)
                    max = x;
            }
            return max;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<string> colores = new List<string> { "amarillo", "verde","rojo",  "azul", "blanco", "negro", "naranja",
                                                      "mostaza", "rosa", "gris", "morado", "marrón", "violeta"};
            int[] numeros = { 10, -60, 20, -50, 30, -40 };
            Console.WriteLine("\nColores de nombre corto");
            foreach (string s in MisMetodos.Where(colores, x => x.Length <= 5))
                Console.WriteLine(s);
            Console.WriteLine("\nPositivos");
            foreach (int i in MisMetodos.Where(numeros, x => x > 0))
                Console.WriteLine(i);

            Console.WriteLine("\nDe los colores de nombre corto el mayor alfabéticamente");
            Console.WriteLine(MisMetodos.Mayor(MisMetodos.Where(colores, x => x.Length <= 5)));

            #region Con notación punto usando extensores

            //Convertir en extensores los métodos poniendo el this
            //Mostrar el uso del Intellisense

            Console.WriteLine("\nColores de nombre corto");
            foreach (string s in colores.Where(x => x.Length <= 5))
                Console.WriteLine(s);
            Console.WriteLine("\nNegativos");
            foreach (int i in numeros.Where(x => x < 0))
                Console.WriteLine(i);

            Console.WriteLine("\nDe los colores de nombre largo el mayor alfabéticamente");
            Console.WriteLine(colores.Where(x => x.Length > 5).Mayor());

            Console.WriteLine("\nDe los negativos el mayor");
            Console.WriteLine(numeros.Where(x => x < 0).Mayor());

            #endregion Con notación punto usando extensores

            #region Uso del GroupBy Ejemplo de inferencia de tipo

            var coloresAgrupados = colores.GroupBy(s => s.Length);
            Console.WriteLine("\nGrupos de colores según longitud");
            foreach (var g in coloresAgrupados)
            {
                Console.WriteLine(g.Key);
                foreach (var s in g)
                    Console.WriteLine("  {0}", s);
            }

            //Más declarativo
            var coloresPorLetra = from c in colores group c by c[0];
            Console.WriteLine("\nGrupos de colores según primera letra");
            foreach (var g in coloresPorLetra)
            {
                Console.WriteLine(g.Key);
                foreach (var s in g)
                    Console.WriteLine("  {0}", s);
            }

            #endregion Uso del GroupBy Ejemplo de inferencia de tipo

            Console.ReadLine();
        }
    }
}