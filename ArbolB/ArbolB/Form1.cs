using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbolB
{
    public partial class Form1 : Form
    {
        int cuenta = 7;
        Arbol_B ab;
        public Form1()
        {
            InitializeComponent();
           ab = new Arbol_B(5);
            ab.insertar(5);
            ab.insertar(9);
            ab.insertar(15);
            ab.insertar(16);
            ab.insertar(18);
            ab.insertar(22);
            ab.insertar(24);
            ab.insertar(26);
            ab.insertar(29);
            ab.insertar(32);
            ab.insertar(45);
            ab.insertar(48);
            ab.insertar(57);
            ab.insertar(79);
            ab.insertar(82);
            ab.insertar(126);
            ab.insertar(172);
            ab.insertar(192);
            ab.insertar(232);
            ab.insertar(19);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ab.insertar(int.Parse(txtInsertar.Text.ToString()));
            txtInsertar.Clear();
            //cuenta++;
            //String s1 = "5";
            //String s2 = null;

            //int val = String.Compare(s1, s2);
            //System.Console.WriteLine(val);


            //NodoBD nuevo = new NodoBD("bd1");

            //Generico<String> gen = new Generico<String>("hola");

            //Console.WriteLine(gen.getId());

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Grafo f = new Grafo();
            f.generar_grafo_arbolB(ab.raiz);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ab.eliminar(int.Parse(textBox1.Text.ToString()));
            textBox1.Clear();
        }
    }
}
