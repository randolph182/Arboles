using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolB
{
    public class Generico<T> 
    {
        private T valor;


        public Generico(T val)
        {
            this.valor = val;
        }

        public T getValor()
        {
            return valor;
        }


        public String getId()
        {
            if (typeof(T) == typeof(NodoBD))
                return valor.ToString();
            else if (typeof(T) == typeof(String))
                return valor.ToString();    
            return "";
        }

        
    }
}
