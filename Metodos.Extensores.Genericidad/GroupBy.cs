using System.Collections;
using System.Collections.Generic;

namespace Metodos.Extensores.Genericidad
{
    #region GroupBy Iteradores + Genericidad EJEMPLO de Query methods usando iteradores y genericidad

    //Delegate genérico. Métodos que reciben un parámetro de tipo T y devuelven un valor de tipo R
    public delegate R Func<T, R>(T x);

    #region Group

    public class Group<K, T> : IEnumerable<T>
    {
        private readonly K key;
        private IEnumerable<T> g;

        public Group(K groupKey, IEnumerable<T> g)
        {
            this.key = groupKey;
            this.g = g;
        }

        public K Key { get { return key; } }

        public IEnumerator<T> GetEnumerator()
        {
            return g.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    #endregion Group

    public static class Query
    {
        #region GROUP BY method. Iterador sobre Iterador

        /*
           * NO es una implementación eficiente, solo como ejemplo
           * Dado un selector que a partir de un T obtiene un K y un IEnumerable de T, devuelve un IEnumerable de grupos segun el K
           * y todos los T del IEnumerable que al aplicar el selector se obtiene K.
           * Admirar la belleza del efecto lazy del yield return
        */

        public static IEnumerable<Group<K, T>> GroupBy<T, K>(this IEnumerable<T> source,
                                                             Func<T, K> selector)
        {
            List<K> processedKeys = new List<K>();
            foreach (T t in source)
            {
                K key = selector(t);
                if (!processedKeys.Contains(key))
                {
                    processedKeys.Add(key);
                    yield return new Group<K, T>(key, Grouping(source, selector, key));
                }
            }
        }

        private static IEnumerable<T> Grouping<T, K>(IEnumerable<T> source,
                                             Func<T, K> selector, K key)
        {
            foreach (T t in source)
                if (selector(t).Equals(key)) yield return t;
        }

        #endregion GROUP BY method. Iterador sobre Iterador
    }

    #endregion GroupBy Iteradores + Genericidad EJEMPLO de Query methods usando iteradores y genericidad
}