using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class ArvoreBinaria<Tipo> where Tipo : IComparable<Tipo>
    {
        NoArvore<Tipo> raiz;

        Tipo tipo;

        public ArvoreBinaria()
        {
            raiz = new NoArvore<Tipo>();
        }
        public void AdicionarDado(Tipo T)
        {
            
        }
        /*public void RemoverDado(Tipo T)
        {

        }*/
        public Tipo BuscarDado(Tipo T)
        {
            Tipo ret = default(Tipo);

            return ret;
        }
        private NoArvore<Tipo> BuscarNo()
        {
            NoArvore<Tipo> ret = null;

            return ret;
        }

    }
}
