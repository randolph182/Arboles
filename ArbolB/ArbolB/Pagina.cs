using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolB
{
    class Pagina
    {
        public int m;
        public int cuenta;
        public int [] claves;
        public Pagina[] ramas;
        public Pagina() { }

        public Pagina(int orden)
        {
            m = orden;
            claves = new int[m]; //por el momento solo con claves vamos a trabajar
            ramas = new Pagina[m];

            //llenamos las paginas
            for (int i = 0; i < m; i++)
            {
                ramas[i] = null;
            }
        }

        public bool pagina_llena(Pagina actual)
        {
            return actual.cuenta == m - 1;
        }

        public bool pagina_semi_llena(Pagina actual)
        {
            return actual.cuenta < m / 2;
        }

        public void escribe_pagina(Pagina actual)
        {
            int k;
            Console.Write("Nodo: ");
            for (k = 0; k <= actual.cuenta; k++)
            {
                Console.WriteLine(actual.claves[k]);
            }
        }
    }
}
