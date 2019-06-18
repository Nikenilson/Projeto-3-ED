using System;

public class PilhaLista<Dado> : IStack<Dado> where Dado : IComparable<Dado>, ICloneable
{
    private NoLista<Dado> topo;
    private int tamanho;
    public PilhaLista()
    { // construtor
        topo = null;
        tamanho = 0;
    }
    public int Tamanho()
    {
        return tamanho;
    }
    public bool EstaVazia()
    {
        return (topo == null);
    }
    public void Empilhar(Dado o)
    {
        NoLista<Dado> novoNo = new NoLista<Dado>(o, topo);
        topo = novoNo; // topo passa a apontar o novo nó
        tamanho++; // atualiza número de elementos na pilha
    }
    public Dado OTopo() 
    {
        Dado o;
        if (EstaVazia())
            throw new PilhaVaziaException("Underflow da pilha");
        o = topo.Info;
        return o;
    }
    public Dado Desempilhar() 
    {
        if (EstaVazia())
        throw new PilhaVaziaException("Underflow da pilha");
        Dado o = topo.Info; // obtém o objeto do topo
        topo = topo.Prox; // avança topo para o nó seguinte
        tamanho--; // atualiza número de elementos na pilha
        return o; // devolve o objeto que estava no topo
    }
    public PilhaLista(PilhaLista<Dado> modelo)
    {
        if(modelo == null)
	    throw new Exception("Modelo ausente");

        this.topo = (NoLista<Dado>) modelo.topo.Clone();
        this.tamanho = modelo.tamanho;
    
    }

    public Object Clone()
    {
        PilhaLista<Dado> obj = null;

        try
        {
             obj = new PilhaLista<Dado>(this);
        }
        catch (Exception erro)
        {
        }

        return obj;
    }
}