using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        public void eliminar(int valor)
        {
            eliminar(ref raiz, valor);
        }

        public void eliminar(ref Pagina raiz, int valor)
        {
            bool encontrado = false;
            eliminarRegistro(raiz,valor,ref encontrado);
            if(encontrado)
            {
                Console.WriteLine("INFO: clave eliminada: " + valor.ToString());
                if(raiz.cuenta == 0)
                {
                    /* la raiz esta vacia, se libera el nodo y se establece la nueva raiz*/
                    Pagina p = raiz;
                    raiz = raiz.ramas[0];
                }
            }
            else
            {
                Console.WriteLine("INFO: la clave " + valor.ToString() + "no se encuentra" );
            }
        }

        public void eliminarRegistro(Pagina actual, int valor, ref bool encontrado)
        {
            int k = 0;
            if(actual != null)
            {
                encontrado = buscarPagina(actual, valor,ref k);
                if(encontrado)
                {
                    if(actual.ramas[k - 1] == null)
                    {
                        quitar(actual, k);
                    }
                    else
                    {
                        sucesor(actual, k);
                        eliminarRegistro(actual.ramas[k], actual.claves[k], ref encontrado);
                    }
                }
                else
                {
                    eliminarRegistro(actual.ramas[k], valor, ref encontrado);
                }

                /*las llamadas recursivas devuelven control a este punto
                Se comprueba el numero de claves del nodo descendiente,
                desde el nodo actual en la ruta de busqueda seguida*/
                if (actual.ramas[k] != null)
                {
                    if(actual.ramas[k].cuenta  < orden/2)
                    {
                        restablecer(actual, k);
                    }
                }
            }
            else
            {
                encontrado = false;
            }
        }

        public void quitar(Pagina actual, int k)
        {
            int j;

            for(j = k + 1; j <= actual.cuenta; j++) 
            {
                actual.claves[j - 1] = actual.claves[j]; //practicamente borramos la clave con el corrimeinto
                actual.ramas[j - 1] = actual.ramas[j];
            }
            actual.cuenta--;
        }

        public void sucesor(Pagina actual, int k)
        {
            Pagina q = null;
            q = actual.ramas[k];
            while(q.ramas[0] != null)
            {
                q = q.ramas[0];
            }
            /* q referencia al nodo hoja*/
            actual.claves[k] = q.claves[1];
        }

        public void restablecer(Pagina actual, int k)
        {
            if(k > 0) //tiene hermano izquierdo
            {
                if(actual.ramas[k-1].cuenta > orden/2)
                {
                    moverDerecha(actual, k);
                }
                else
                {
                    combinar(actual, k);
                }
            }
            else //solo tine hermano derecho
            {
                if(actual.ramas[1].cuenta  > orden/2)
                {
                    moverIzquierda(actual, 1);
                }
                else
                {
                    combinar(actual, 1);
                }
            }
        }


        public void moverDerecha(Pagina actual, int k)
        {
            int j = 0;
            Pagina nodoProblema = actual.ramas[k];
            Pagina nodoIzquierdo = actual.ramas[k - 1];

            for (j = nodoProblema.cuenta; j >= 1; j--)
            {
                nodoProblema.claves[j + 1] = nodoProblema.claves[j];
                nodoProblema.ramas[j + 1] = nodoProblema.ramas[j];
            }

            nodoProblema.cuenta++;
            nodoProblema.ramas[1] = nodoProblema.ramas[0];
            /* baaja la clave del nodo padre*/
            nodoProblema.claves[1] = actual.claves[k];
            /*  sube a clave desde el hermano izquierdo al nodo padre, para reemplazar la que antes bajo*/
            actual.claves[k] = nodoIzquierdo.claves[nodoIzquierdo.cuenta];
            nodoProblema.ramas[0] = nodoIzquierdo.ramas[nodoIzquierdo.cuenta];
            nodoIzquierdo.cuenta--;
        }

        public void moverIzquierda(Pagina actual, int k)
        {
            int j = 0;
            Pagina nodoProblema = actual.ramas[k - 1];
            Pagina nodoDerecho = actual.ramas[k];

            nodoProblema.cuenta++;
            nodoProblema.claves[nodoProblema.cuenta] = actual.claves[k];
            nodoProblema.ramas[nodoProblema.cuenta] = nodoDerecho.ramas[0];

            /* sube la clave desde le hermano derecho al nodo padre
            para reemplazar la que antes bajo*/

            actual.claves[k] = nodoDerecho.claves[1];
            nodoDerecho.ramas[1] = nodoDerecho.ramas[0];
            nodoDerecho.cuenta--;

            for (j = 1; j < nodoDerecho.cuenta; j++)
            {
                nodoDerecho.claves[j] = nodoDerecho.claves[j + 1];
                nodoDerecho.ramas[j] = nodoDerecho.ramas[j + 1];
            }
        }


        public void combinar(Pagina actual, int k)
        {
            int j = 0;
            Pagina nodoIzquierdo, q = null;
            q = actual.ramas[k];
            nodoIzquierdo = actual.ramas[k - 1];
                        /* baja la clave mediana desde el node padre*/
            nodoIzquierdo.cuenta++;
            nodoIzquierdo.claves[nodoIzquierdo.cuenta] = actual.claves[k];
            nodoIzquierdo.ramas[nodoIzquierdo.cuenta] = q.ramas[0];
            /* mueve claves del nodo derecho */
            for(j = 1; j <= q.cuenta; j++)
            {
                nodoIzquierdo.cuenta++;
                nodoIzquierdo.claves[nodoIzquierdo.cuenta] = q.claves[j];
                nodoIzquierdo.ramas[nodoIzquierdo.cuenta] = q.ramas[j];
            }
            /* se ajustan claves y ramas del nodo padre debido a que
                        antes descendio la clave k*/
            for(j = k; j <= actual.cuenta -1; j++)
            {
                actual.claves[j] = actual.claves[j + 1];
                actual.ramas[j] = actual.ramas[j + 1];
            }
            actual.cuenta--;
            //un free
        }
    }
}
