using System;

/// 
/// Samuel Gomes de Lima Dias - 18169 \\ Victor Botin Avelino - 18172
///

public interface IStack<Dado> where Dado : IComparable<Dado>
{
  void Empilhar(Dado elemento);
  Dado Desempilhar();
  Dado OTopo();
  bool EstaVazia();
  int Tamanho();
}
