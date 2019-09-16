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
            ab.insertar(1);
            ab.insertar(2);
            ab.insertar(3);
            ab.insertar(4);
            ab.insertar(5);
            ab.insertar(6);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ab.insertar(cuenta);
            cuenta++;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Grafo f = new Grafo();
            f.generar_grafo_arbolB(ab.raiz);
        }
    }
}
