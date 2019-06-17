using System;
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
            int idCidadeAtual = lsbOrigem.SelectedIndex;
            int idCidadeDestino = lsbDestino.SelectedIndex;
            int aux = 0;
            Caminho caminhoAtual = null;
            
            PilhaLista<Caminho>[] possibilidades = new PilhaLista<Caminho>[Convert.ToInt32(Math.Pow(qtdCidades, 2))];

            var caminhos = new PilhaLista<Caminho>();
            var possibilidades2 = new PilhaLista<Caminho>();

            for (int linhas = 0; linhas < qtdCidades; linhas++)
                for (int colunas = 0; colunas < qtdCidades; colunas++)
                {
                    if (idCidadeAtual == matriz[linhas, colunas].IdCidadeOrigem)
                        caminhos.Empilhar(matriz[linhas, colunas]);
                }
            possibilidades[aux] = caminhos;
            
            

            while (!possibilidades[aux].EstaVazia())
            {
                caminhoAtual = possibilidades[aux].Desempilhar();
                
                for (int linhas = 0; linhas < qtdCidades; linhas++)
                    for (int colunas = 0; colunas < qtdCidades; colunas++)
                    {
                        if (caminhoAtual.IdCidadeDestino == matriz[linhas, colunas].IdCidadeOrigem)
                            possibilidades2.Empilhar(matriz[linhas, colunas]);
                    }

                aux++;
            }

           /* Cidade origem, destino;
            Caminho caminhoAFazer;

            MessageBox.Show("Buscar caminhos entre cidades selecionadas");

            origem  = (Cidade)lsbOrigem.SelectedItem;
            destino = (Cidade)lsbDestino.SelectedItem;
            */
           //Criar método Percorrer na classe Caminho
           //Pegar origem e destino como parâmetro.

            /*No evento Click do btnBuscar – 
             * procurar os caminhos entre as cidades selecionadas no lsbOrigem e lsbDestino, 
             * exibindo todos os caminhos no dgvCaminhos (um por linha) 
             * e o melhor caminho no dgvMelhorCaminho. 
             * Usar retas para ligar as cidades no mapa referente ao caminho da linha selecionada no dgvCaminhos.*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                g.DrawString(atualRecursivo.Info.Nome, new Font("Comic Sans", 12, FontStyle.Regular), meuPincel, x - 5 , y + 15);

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

