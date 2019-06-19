using System;
/// 
/// Samuel Gomes de Lima Dias - 18169 \\ Victor Botin Avelino - 18172
///
public class NoLista<Dado> where Dado : IComparable<Dado>, ICloneable
{ 
    Dado info;
    NoLista<Dado> prox;

    public NoLista(Dado info, NoLista<Dado> prox)
    {
        Info = info;
        Prox = prox;
    }

    public Dado Info
    {
        get { return info;  }
        set
        {
            if (value != null)
               info = value;
        }
    }

    public NoLista<Dado> Prox
    {
        get => prox;
        set => prox = value;
    }
    public NoLista(NoLista<Dado> modelo)
    {
        if (modelo == null)
            throw new Exception("Modelo ausente");
        

        this.info = (Dado) modelo.info.Clone();
        if (modelo.Prox != null)
            this.prox = (NoLista<Dado>)modelo.prox.Clone();
        else
            this.prox = null;
    }

    public Object Clone()
    {
        NoLista<Dado> obj = null;

        try
        {
            obj = new NoLista<Dado>(this);
        }
        catch (Exception erro)
        {
        }

        return obj;
    }
}

