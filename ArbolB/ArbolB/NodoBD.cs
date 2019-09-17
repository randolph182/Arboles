using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolB
{
    public class NodoBD
    {
        public String id; //nombre de la base de datos 
        Arbol_B tablas = null; //apuntador a un nuevo arbol b pero con tablas
        //aca tambien podria ir un entorno
        public NodoBD(String id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return id.ToString();
        }


    }
}
