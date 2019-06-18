using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace apCaminhosMarte
{
    class ArvoreBinaria<Dado> where Dado : IComparable<Dado>
    {
        NoArvore<Dado> raiz, atual, antecessor;
        int quantosNos = 0;

        public ArvoreBinaria()
        {
            raiz = null;
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
                if (antecessor == null)
                    raiz = novoNo;
                else if (incluido.CompareTo(antecessor.Info) < 0)
                    antecessor.Esq = novoNo;
                else
                    antecessor.Dir = novoNo;
                quantosNos++;
            }
        }

    }
}


