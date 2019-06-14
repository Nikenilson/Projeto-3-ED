using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    public class Caminho
    {
        //Atributos:
        private int idCidadeOrigem;
        private int idCidadeDestino;
        private int distancia;
        private int tempo;
        private int custo;

        //Constantes dos atributos:
        const int inicioIdOrigem = 0;
        const int tamanhoIdOrigem = 3;
        const int inicioIdDestino = inicioIdOrigem + tamanhoIdOrigem;
        const int tamanhoIdDestino = 3;
        const int inicioDistancia = inicioIdDestino + tamanhoIdDestino;
        const int tamanhoDistancia = 5;
        const int inicioTempo = inicioDistancia + tamanhoDistancia;
        const int tamanhoTempo = 4;
        const int inicioCusto = inicioTempo + tamanhoTempo;
        const int tamanhoCusto = 5;

        //Propriedades dos atributos:
        public int IdCidadeOrigem { get => idCidadeOrigem; set => idCidadeOrigem = value; }
        public int IdCidadeDestino { get => idCidadeDestino; set => idCidadeDestino = value; }
        public int Distancia { get => distancia; set => distancia = value; }
        public int Tempo { get => tempo; set => tempo = value; }
        public int Custo { get => custo; set => custo = value; }

        //Construtor da classe. Especificamente, este construtor atribui
        //aos atributos os dados de uma linha do arquivo texto.
        public Caminho(string s)
        {
            this.idCidadeOrigem = int.Parse(s.Substring(inicioIdOrigem, tamanhoIdOrigem)); 
            this.idCidadeDestino = int.Parse(s.Substring(inicioIdDestino, tamanhoIdDestino));
            this.distancia = int.Parse(s.Substring(inicioDistancia, tamanhoDistancia));
            this.tempo = int.Parse(s.Substring(inicioTempo, tamanhoTempo));
            this.custo = int.Parse(s.Substring(inicioCusto, tamanhoCusto));
        }

        
        
    }
}
