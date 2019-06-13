using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class ArvoreBinaria<Dado> where Dado : IComparable<Dado>
    {
        NoArvore<Dado> raiz, atual, antecessor;
        int quantosNos = 0;
        Dado dado;

        public ArvoreBinaria()
        {
            raiz = new NoArvore<Dado>();
        }
        public NoArvore<Dado> Raiz
        {
            get { return raiz; }
            set { raiz = value; }
        }
        public NoArvore<Dado> Atual
        {
            get { return atual; }
            set { atual = value; }
        }
        public NoArvore<Dado> Antecessor
        {
            get { return antecessor; }
            set { antecessor = value; }
        }
        public bool Existe(Dado procurado)  // pesquisa binária não recursiva
        {
            antecessor = null;
            atual = Raiz;
            while (atual != null)
            {
                if (atual.Info.CompareTo(procurado) == 0)
                    return true;
                else
                {
                    antecessor = atual;
                    if (procurado.CompareTo(atual.Info) < 0)
                        atual = atual.Esq; // Desloca à esquerda
                    else
                        atual = atual.Dir; // Desloca à direita
                }
            }
            return false; // Se atual == null, a chave não existe mas antecessor aponta o pai 
        }

        public void Incluir(Dado incluido)    // inclusão usando o método de pesquisa binária
        {
            if (Existe(incluido))
                throw new Exception("Informação repetida");
            else
            {
                var novoNo = new NoArvore<Dado>(incluido);
                if (incluido.CompareTo(antecessor.Info) < 0)
                    antecessor.Esq = novoNo;
                else
                    antecessor.Dir = novoNo;
                quantosNos++;
            }
        }

        /*
         
            MÉTODO COMENTADO TEMPORÁRIO PARA USO INTERNO, SENHOR SAMUEL.
         
        public boolean excluiNo(T excluido)
        {
            if (!existe(excluido))
            return false;
            else
            {
            // antecessor e atual foram definidos em existe()
            if (atual.getDir() == null) // nó a excluir tem 0 ou 1 filho?
            liga(antecessor, excluido, atual.getEsq());
            else
            if (atual.getEsq() == null)
            liga(antecessor, excluido, atual.getDir());
            else
            {
            // nó a excluir tem 2 filhos
            antecessor = atual;
            NoArvore2<T> aux = atual.getEsq();
            while (aux.getDir() != null) // procura maior dos menores filhos
            {
            antecessor = aux;
            aux = aux.getDir();
            }
            atual.setElemento(aux.getElemento()); // troca conteúdo
            antecessor.setDir(aux.getEsq());
            aux = null;
            }
            this.setTamanho(this.getTamanho()-1);
            return true;
            }
            }
         */

        public void RemoverDado(Dado T)
        {
            if (!Existe(T))
                throw new Exception("Dado inexistente");

        }
        public Dado BuscarDado(Dado T)
        {
            Dado ret = default(Dado);

            return ret;
        }
        private NoArvore<Dado> BuscarNo()
        {
            NoArvore<Dado> ret = null;

            return ret;
        }

    }
}
