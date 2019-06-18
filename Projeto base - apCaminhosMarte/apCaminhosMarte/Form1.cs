﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/// 
/// Samuel Gomes de Lima Dias - 18169 \\ Victor Botin Avelino - 18172
///

namespace apCaminhosMarte
{
    public partial class Form1 : Form
    {
        ArvoreBinaria<Cidade> arvore;
        Caminho[,] matriz;
        int qtdCidades = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {

        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            int idCidadeOrigem = lsbOrigem.SelectedIndex;
            int idCidadeAtual = idCidadeOrigem;
            int idCidadeDestino = lsbDestino.SelectedIndex;
                         
            PilhaLista<Caminho>[] caminhosValidos = new PilhaLista<Caminho>[Convert.ToInt32(Math.Pow(qtdCidades, 2))]; //muda depois
            PilhaLista<Caminho> p = new PilhaLista<Caminho>();

            
            
            //ha caminho comecou aqui
            bool[] passou = new bool[qtdCidades];
            int cidade_atual, saida_atual;
            bool achou = false;
            cidade_atual = idCidadeOrigem;
            saida_atual = 0;

            for (int i = 0; i < qtdCidades; i++) // inicia os valores de “passou”    
                passou[i] = false; // pois ainda não foi em nenhuma cidade  

            int indice = 0;
            while (!achou && !(cidade_atual == idCidadeOrigem && saida_atual == qtdCidades && p.EstaVazia())) //Mudar condicao
            {
                while ((saida_atual < qtdCidades) && !achou)
                {   // se não há saida pela cidade testada, verifica a próxima      
                    if (matriz[cidade_atual,saida_atual] == null)
                        saida_atual++;
                    else // se já passou pela cidade testada, verifica se a próxima 
                            // cidade permite saida  
                        if (passou[saida_atual])
                        saida_atual++;
                    else     // se chegou na cidade desejada, empilha o local     
                                // e termina o processo de procura de caminho    
                    if (saida_atual == idCidadeDestino)
                    {
                        p.Empilhar(new Caminho(cidade_atual, saida_atual));
                        achou = true;
                    }
                    else
                    {
                        p.Empilhar(new Caminho(cidade_atual, saida_atual));
                        passou[cidade_atual] = true;
                        //System.out.println("Saiu de "+ cidade_atual +           " para "+saida_atual);
                        cidade_atual = saida_atual;
                        saida_atual = 0;
                    }
                }
                if (!achou)
                    if (!p.EstaVazia())
                    {
                        Caminho aux = p.Desempilhar();
                        saida_atual = aux.IdCidadeDestino;
                        cidade_atual = aux.IdCidadeOrigem;
                        aux = null;
                        //System.out.println("voltando de "+saida_atual+    " para "+cidade_atual);
                        saida_atual++;
                    }
            }
            if (achou)
            {  // desempilha a configuração atual da pilha      
                // para a pilha da lista de parâmetros   
                while (!p.EstaVazia())
                {
                    caminhosValidos[indice].Empilhar(p.Desempilhar());
                }
            }
        
            //ha caminho acabou aqui

            //Mostra todos os caminhos no dgvCaminhos
            TodosOsCaminhos(caminhosValidos, idCidadeOrigem, idCidadeDestino);

            //Melhor caminho
            MenorCaminho(caminhosValidos, idCidadeOrigem, idCidadeDestino);

            /*No evento Click do btnBuscar – 
             * procurar os caminhos entre as cidades selecionadas no lsbOrigem e lsbDestino, 
             * exibindo todos os caminhos no dgvCaminhos (um por linha) 
             * e o melhor caminho no dgvMelhorCaminho. \\Ta la 
             * Usar retas para ligar as cidades no mapa referente ao caminho da linha selecionada no dgvCaminhos.\\Ta la
             */
        }
        public void TodosOsCaminhos(PilhaLista<Caminho>[] caminhosValidos, int idCidadeOrigem, int idCidadeDestino)
        {
            for (int i = 0; i < caminhosValidos.Length; i++)
            {
                PilhaLista<Caminho> auxiliar = caminhosValidos[i];

                string[] row = new string[Convert.ToInt32(Math.Pow(qtdCidades, 2) * 2)];
                int aux = 0;
                Caminho caminhoAux;
                while (!auxiliar.EstaVazia())
                {
                    caminhoAux = auxiliar.Desempilhar();
                    arvore.Existe(new Cidade(idCidadeOrigem));
                    row[aux++] = caminhoAux.IdCidadeOrigem + " - " + arvore.Atual.Info.Nome;
                    arvore.Existe(new Cidade(idCidadeDestino));
                    row[aux++] = caminhoAux.IdCidadeDestino + " - " + arvore.Atual.Info.Nome;
                }
                dgvCaminho.Rows.Add(row);
            }
        }
        public void MenorCaminho(PilhaLista<Caminho>[] caminhosValidos,int idCidadeOrigem, int idCidadeDestino)
        {
            int menorDistancia = 0;
            int indiceMenor = 0;
            PilhaLista<Caminho>[] aux = new PilhaLista<Caminho>[caminhosValidos.Length];
            while (!caminhosValidos[0].EstaVazia())
            {
                aux[0] = caminhosValidos[0];//.Clone();
                menorDistancia =+ caminhosValidos[0].Desempilhar().Distancia;
            }
          
            for (int auxI = 1; auxI < caminhosValidos.Length; auxI++)
            {
                aux[auxI] = caminhosValidos[0];//.Clone();
                int distancia = 0;
                while (!caminhosValidos[auxI].EstaVazia())
                    distancia =+ caminhosValidos[auxI].Desempilhar().Distancia;

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    indiceMenor = auxI;
                }
            }

            for (int auxI = 0; auxI < caminhosValidos.Length; auxI++)
            {
                caminhosValidos[auxI] = aux[auxI];
            }

            PilhaLista<Caminho> melhorCaminho = caminhosValidos[indiceMenor];

            string[] row = new string[Convert.ToInt32(Math.Pow(qtdCidades, 2))];
            int i = 0;
            Caminho caminhoAux;
            while (!melhorCaminho.EstaVazia())
            {
                caminhoAux = melhorCaminho.Desempilhar();
                arvore.Existe(new Cidade(idCidadeOrigem));
                row[i++] = caminhoAux.IdCidadeOrigem + " - " + arvore.Atual.Info.Nome;
                arvore.Existe(new Cidade(idCidadeDestino));
                row[i++] = caminhoAux.IdCidadeDestino + " - " + arvore.Atual.Info.Nome;
            }
            dgvMelhorCaminho.Rows.Add(row);
        }

        public static bool haCaminho(int[][] cam, int origem, int destino, int lin, int col, PilhaLista<Caminho> saida)
        {
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            var arq = new StreamReader("CidadesMarte.txt");
            arvore = new ArvoreBinaria<Cidade>();

            string linha = null;
            while (!arq.EndOfStream)
            {
                linha = arq.ReadLine();
                arvore.Incluir(new Cidade(linha));
                qtdCidades++;
            }
            arq.Close();

            var arq2 = new StreamReader("CaminhosEntreCidadesMarte.txt");
            matriz = new Caminho[qtdCidades, qtdCidades];
            string linha2 = null;
            while (!arq2.EndOfStream)
            {
                linha2 = arq2.ReadLine();
                matriz[int.Parse(linha2.Substring(0, 3)), int.Parse(linha2.Substring(3, 3))] = new Caminho(linha2);
            }
            arq2.Close();

            /*
            MessageBox.Show("Selecione o arquivo CidadesMarte.txt");

            if(oFileDialog.ShowDialog() == DialogResult.OK)
            {
                var arq = new StreamReader(oFileDialog.FileName);
                arvore = new ArvoreBinaria<Cidade>();

                string linha = null;
                while(!arq.EndOfStream)
                {
                    linha = arq.ReadLine();
                    arvore.Incluir(new Cidade(linha));
                    qtdCidades++;
                }
                arq.Close();
            }

            MessageBox.Show("Selecione o arquivo CaminhosEntreCidadesMarte.txt");

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                var arq = new StreamReader(oFileDialog.FileName);
                matriz = new Caminho[qtdCidades,qtdCidades];
                string linha = null;
                while (!arq.EndOfStream)
                {
                    linha = arq.ReadLine();
                    matriz[int.Parse(linha.Substring(0, 3)),int.Parse(linha.Substring(3, 3))] = new Caminho(linha); 
                }
                arq.Close();
            }
            */

        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DesenhaCidades(g, arvore.Raiz);
            DesenhaLinhas(g);
        }

        private void desenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
                            int x, int y, double angulo, double incremento,
                            double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                // sleep(100);
                desenhaArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                                    incremento * 0.60, comprimento * 0.8, g);
                desenhaArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                                                    incremento * 0.60, comprimento * 0.8, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Yellow);
                g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
                g.DrawString(Convert.ToString(raiz.Info.Nome), new Font("Comic Sans", 12),
                                new SolidBrush(Color.Black), xf - 15, yf - 10);
            }
        }

        private void DesenhaLinhas(Graphics g)
        {
            Caminho aux = null;
            Cidade c1 = null;
            Cidade c2 = null;
            Pen minhaCaneta = new Pen(Color.DimGray, 2);
            for (int linhas = 0; linhas < qtdCidades; linhas++)
                for (int colunas = 0; colunas < qtdCidades; colunas++)
                {
                    if (matriz[linhas, colunas] != null)
                    {
                        aux = matriz[linhas, colunas];
                        c1 = arvore.BuscarDado(new Cidade(linhas));
                        c2 = arvore.BuscarDado(new Cidade(colunas));

                        float xI = pbMapa.Size.Width * c1.CoordenadaX / 4096;
                        float yI = pbMapa.Size.Height * c1.CoordenadaY / 2048;
                        float xF = pbMapa.Size.Width * c2.CoordenadaX / 4096;
                        float yF = pbMapa.Size.Height * c2.CoordenadaY / 2048;

                        //Onde I => Inicial, F => Final

                        g.DrawLine(minhaCaneta, xI, yI, xF, yF);
                    }
                }
        }
        private void DesenhaCidades(Graphics g, NoArvore<Cidade> atualRecursivo)
        {
            if (atualRecursivo != null)
            {
                SolidBrush meuPincel = new SolidBrush(Color.Black);
                float x = pbMapa.Size.Width * atualRecursivo.Info.CoordenadaX / 4096 - 8;
                float y = pbMapa.Size.Height * atualRecursivo.Info.CoordenadaY / 2048 - 8;
                g.FillEllipse(meuPincel, x, y, 16, 16);
                g.DrawString(atualRecursivo.Info.Nome, new Font("Comic Sans", 12, FontStyle.Regular), meuPincel, x - 5, y + 15);

                DesenhaCidades(g, atualRecursivo.Esq);
                DesenhaCidades(g, atualRecursivo.Dir);
            }
        }
        private void DrawFrame(object sender, EventArgs e)
        {
            pbMapa.Invalidate();
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            desenhaArvore(true, arvore.Raiz, (int)pnlArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, g);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvMelhorCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Graphics g = pbMapa.CreateGraphics();
            SolidBrush meuPincel = new SolidBrush(Color.Black);
            int coluna = 0;

            for (int c = 0; c < (sender as DataGridView).ColumnCount; c++)
            {
                string linha = (sender as DataGridView).Rows[e.RowIndex].Cells[coluna].Value.ToString();
                if (linha == null || linha == "")
                    break;
                string[] linhaVetor = linha.Split('-');
                arvore.Existe(new Cidade(Convert.ToInt32(linhaVetor[0])));
                float x = pbMapa.Size.Width * arvore.Atual.Info.CoordenadaX / 4096 - 8;
                float y = pbMapa.Size.Height * arvore.Atual.Info.CoordenadaY / 2048 - 8;
                g.FillEllipse(meuPincel, x, y, 16, 16);
                g.DrawString(arvore.Atual.Info.Nome, new Font("Comic Sans", 12, FontStyle.Regular), meuPincel, x - 5, y + 15);

            }
        }
    }
}


/*

Roteiro de utilização

-Quando o programa iniciar sua execução, ler o arquivo CidadesMarte.txt e montar uma árvore
binária de busca armazenando o objeto que representa uma cidade, como todos os seus campos.
// tA lA

-No evento Paint do PictureBox - exibir os nomes e locais das cidades no mapa, de acordo com a
proporção entre coordenadas das cidades referentes ao tamanho original (4096x2048) e as
dimensões atuais do picturebox.// tA lA

-No evento Click do btnBuscar – procurar os caminhos entre as cidades selecionadas no
lsbOrigem e lsbDestino, exibindo todos os caminhos no dvgCaminhos (um por linha) 

-O melhor caminho no dgvMelhorCaminho. 

-Usar retas para ligar as cidades no mapa referente ao caminho
da linha selecionada no dgvCaminhos.

-Na guia [Árvore de Cidades] – exibir a árvore mostrando os números e nomes das cidades.// tA lA

*/

/*
    int aux = 0;
    bool acabou = false;
    bool regressivo = false;

    Caminho caminhoAtual = null;
    PilhaLista<Caminho>[] possibilidades = new PilhaLista<Caminho>[Convert.ToInt32(Math.Pow(qtdCidades, 2))];

    var caminhos = new PilhaLista<Caminho>();
    var possibilidades2 = new PilhaLista<Caminho>();

    do
    {
        for (int colunas = 0; colunas < qtdCidades; colunas++)
        {
            if (matriz[idCidadeAtual, colunas] != null && idCidadeAtual == matriz[idCidadeAtual, colunas].IdCidadeOrigem)
                caminhos.Empilhar(matriz[idCidadeAtual, colunas]);
        }
        possibilidades[aux] = caminhos;

        if (possibilidades[aux].EstaVazia())
            regressivo = true;
        else
        {
            caminhoAtual = possibilidades[aux].Desempilhar();
            idCidadeAtual = caminhoAtual.IdCidadeDestino;
        }
            aux++;

    } while (!acabou);
        */

