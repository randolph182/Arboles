using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolB
{
    class Grafo
    {
        public void generar_grafo_arbolB(Pagina raiz)
        {
            String acum = "digraph G\n" +
                  "{\n node	[shape = record,height=.1];\n";

            if(raiz != null)
            {
                String acumEnlace = "";

                int contNodo = 0;
                int contAux = 0;
                Queue<Pagina> cola = new Queue<Pagina>();

                cola.Enqueue(raiz);

                while(cola.Count != 0)
                {
                    Pagina tmp = cola.Peek();
                    cola.Dequeue();
                    imprimir(tmp, ref acum, ref contNodo, ref contAux, ref acumEnlace);

                    for(int i = 0; i<= tmp.cuenta; ++i)
                    {
                        if(tmp.ramas[i] != null)
                        {
                            cola.Enqueue(tmp.ramas[i]);
                        }
                    }
                    contNodo++;
                }
                acum += "\n" + acumEnlace;
            }
            acum += "}\n";


            StreamWriter sw = new StreamWriter("arbolB.dot");
            sw.WriteLine(acum);
            sw.Close();

            Process a = new Process();
            a.StartInfo.FileName = "\"C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe\"";
            a.StartInfo.Arguments = "-Tjpg arbolB.dot -o arbolB.png";
            a.StartInfo.UseShellExecute = false;
            a.Start();
            a.WaitForExit();
        }




        public void imprimir(Pagina actual, ref String acum, ref int contNodo, ref int contAux,ref String enlace)
        {
            acum += "node" + contNodo.ToString() + "[label=\"";

            acum += "<r0>";
            if(actual.ramas[0] != null)
            {
                enlace += "\"node" + contNodo.ToString() + "\":r0 ->";
                contAux += 1;
                enlace += "\"node" + contAux.ToString() + "\"\n";
            }

            for (int i = 1; i <= actual.cuenta; i++)
            {
                acum += "|";
                acum += "<c" + i.ToString() + "> " + actual.claves[i].ToString();
                acum += "|<r" + i.ToString() + ">";

                if(actual.ramas[i] != null)
                {
                    enlace += "\"node" + contNodo.ToString() + "\":r" + i.ToString() + " -> ";
                    contAux += 1;
                    enlace += "\"node" + contAux.ToString() + "\"\n";
                }
            }
            acum += "\"];\n";
        }
    }
}
