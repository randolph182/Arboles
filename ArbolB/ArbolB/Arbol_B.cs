using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArbolB
{
    class Arbol_B
    {
        int orden;
        public Pagina raiz;

        public Arbol_B() { }

        public Arbol_B(int orden)
        {
            this.orden = orden;
            this.raiz = null;
        }

        public void insertar( int valor) 
        {
            insertar(ref raiz, valor);
        }

        public void insertar(ref Pagina raiz, int valor)
        {
            bool suber_arriba = false ;
            int  mediana = 0;
            Pagina p, nd = null;
            Empujar(raiz, valor, ref suber_arriba,ref mediana,ref nd);
            if(suber_arriba) //si se produjo una reorganizacion de nodos lo cual ser dividio la raiz entonces, la bandera sube_arriba lo indica 
            {
                p = new Pagina(orden);
                p.cuenta = 1;
                p.claves[1] = mediana;
                p.ramas[0] = raiz;
                p.ramas[1] = nd;
                raiz = p;
            }
        }


        public void Empujar(Pagina actual, int valor,ref bool sube_arriba, ref int mediana, ref Pagina nuevo)
        {
            int k = 0; //que rama irse
            if(actual == null)
            {
                sube_arriba = true;
                mediana = valor;
                nuevo = null;
            }
            else
            {
                bool esta;
                esta = buscarPagina(actual, valor,ref k); //k busca la rama por eso se pasa por referencia
                if(esta)
                {
                    System.Console.WriteLine("Clave Duplicada: " + valor);
                    sube_arriba = false;
                    return;
                }
                Empujar(actual.ramas[k], valor,ref sube_arriba,ref mediana,ref nuevo);
                /* devuelve control vuelve por el camino de busqueda*/
                if (sube_arriba)
                {
                    if (actual.pagina_llena(actual))
                    {
                        dividirNodo(actual, mediana, nuevo, k,ref  mediana, ref nuevo);
                    }
                    else
                    {
                        sube_arriba = false;
                        meterHoja(actual, mediana, nuevo, k);
                    }
                }
            }
        }

        public bool buscarPagina( Pagina actual, int valor, ref int k) //k determina la rama o camino 
        {
            /*tomar en cuenta que k es la direccion de las ramas por las que puede bajar la busqueda*/
            bool encontrado;
            if(valor < actual.claves[1]) //ese 1 significa que busca desde la primera posicion en claves por tanto si cumple el valor se va a los valores menores
            {
                k = 0;  //nos indica que bajaresmo por la rama 0
                encontrado = false;
            }
            else //examina las claves del nodo en orden descendente
            {
                k = actual.cuenta; //desde la clave actual
                while((valor < actual.claves[k]) && (k > 1)) //buscar una posicion hasta donde valor deje de ser menor ( por si vienen un valor menor a los que hay en el nodo )
                {
                    k--;
                }
                encontrado = valor == actual.claves[k]; //si la posicion encontrada es igual al valor ; lo cual clave repetida
            }
            return encontrado;
        }


        public void meterHoja(Pagina actual,int valor,Pagina rd,int k)
        {
            int i;
            /* desplza a la derecha los elementos para hcer un hueco*/
            for (i = actual.cuenta; i >= k + 1; i--)
            {
                actual.claves[i + 1] = actual.claves[i];
                actual.ramas[i + 1] = actual.ramas[i];
            }
            actual.claves[k + 1] = valor;
            actual.ramas[k + 1] = rd;
            actual.cuenta++;
        }

        public void dividirNodo(Pagina actual,int valor,Pagina rd,int k,ref int mediana,ref Pagina nuevo)
        {
            int i, posMdna;
            posMdna = (k <= orden / 2) ? orden / 2 : orden / 2 + 1;
            nuevo = new Pagina(orden); //se crea una nueva pagina

            for(i = posMdna +1; i < orden; i++)
            {
                /* es desplzada la mida derecha al nuevo nodo, la clave mediana se queda en el nodo origen*/
                nuevo.claves[i - posMdna] = actual.claves[i];
                nuevo.ramas[i - posMdna] = actual.ramas[i];
            }
            nuevo.cuenta = (orden - 1) - posMdna; /* numero de claves en el nuevo nodo*/
            actual.cuenta = posMdna; // numero claves en el nodo origen

            /* Es insertada la clave y rama en el nodo que le corresponde*/
            if(k <= orden / 2) //si k es menor al minimo de claves  que puede haber en la pagina
            {
                meterHoja(actual, valor, rd, k); //inserta en el nodo origen
            }
            else
            {
                meterHoja(nuevo, valor, rd, k - posMdna);  //se inserta el nuevo valor que traiamos en el nodo nuevo
            }

            /* se extrae clave mediana del nodo origen*/
            mediana = actual.claves[actual.cuenta];

            /* Rama0 del nuevo nodo es la rama de la mediana*/
            nuevo.ramas[0] = actual.ramas[actual.cuenta];
            actual.cuenta--;

        }

    }
}
