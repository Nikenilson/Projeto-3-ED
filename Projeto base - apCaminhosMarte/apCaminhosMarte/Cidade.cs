using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Cidade
    {
        //Atributos:
        private int idCidade;
        private string nome;
        private int coordenadaX;
        private int coordenadaY;

        //Constantes dos atributos:
        const int inicioId = 0;
        const int tamanhoId = 3;
        const int inicioNome = inicioId + tamanhoId;
        const int tamanhoNome = 15;
        const int inicioX = inicioNome + tamanhoNome;
        const int tamanhoX = 5;
        const int inicioY = inicioX + tamanhoX;
        const int tamanhoY = 5;

        //Propriedades dos atributos:
        public int IdCidade { get => idCidade; set => idCidade = value; }
        public string Nome { get => nome; set => nome = value; }
        public int CoordenadaX { get => coordenadaX; set => coordenadaX = value; }
        public int CoordenadaY { get => coordenadaY; set => coordenadaY = value; }


        //Construtor da classe. Especificamente, este construtor atribui
        //aos atributos os dados de uma linha do arquivo texto.
        public Cidade(string s)
        {
            idCidade = int.Parse(s.Substring(inicioId, tamanhoId));
            nome = s.Substring(inicioNome, tamanhoNome);
            coordenadaX = int.Parse(s.Substring(inicioX, tamanhoX));
            coordenadaY = int.Parse(s.Substring(inicioY, tamanhoY));
        }
    }
}
