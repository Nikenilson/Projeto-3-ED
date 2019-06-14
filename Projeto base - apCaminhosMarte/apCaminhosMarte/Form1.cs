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
        Caminho[][] matriz;

        Bitmap DrawArea;


        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            Cidade origem, destino;
            Caminho caminhoAFazer;

            MessageBox.Show("Buscar caminhos entre cidades selecionadas");

            origem  = (Cidade)lsbOrigem.SelectedItem;
            destino = (Cidade)lsbDestino.SelectedItem;

            //Criar método Percorrer na classe Caminho
            //Pegar origem e destino como parâmetro.
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

                }
                arq.Close();
            }

            MessageBox.Show("Selecione o arquivo CaminhosEntreCidadesMarte.txt");

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                var arq = new StreamReader(oFileDialog.FileName);
                
                string linha = null;
                while (!arq.EndOfStream)
                {
                    linha = arq.ReadLine();
                    matriz[int.Parse(linha.Substring(0, 3))][int.Parse(linha.Substring(3, 6))] = new Caminho(linha); 
                }
                arq.Close();
            }

        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            DrawArea = new Bitmap(pbMapa.Size.Width, pbMapa.Size.Height);
            Graphics g = Graphics.FromImage(DrawArea);
            SolidBrush meuPincel = new SolidBrush(Color.Black);
            Cidade aux;

            //ta errado, mas eh so temporario pra ter algo aqui
            while (arvore.Atual != null)
            {
                
                aux = arvore.visitar(arvore.Atual);
                g.FillEllipse(meuPincel, aux.CoordenadaX * (pbMapa.Size.Width / 4096), aux.CoordenadaY * (pbMapa.Size.Height / 2048), 25, 25);
            }
                


            g.Dispose();
        }
    }
}

/*
 * 
 * Roteiro de utilização
Quando o programa iniciar sua execução, ler o arquivo CidadesMarte.txt e montar uma árvore
binária de busca armazenando o objeto que representa uma cidade, como todos os seus campos.

No evento Paint do PictureBox - exibir os nomes e locais das cidades no mapa, de acordo com a
proporção entre coordenadas das cidades referentes ao tamanho original (4096x2048) e as
dimensões atuais do picturebox.

No evento Click do btnBuscar – procurar os caminhos entre as cidades selecionadas no
lsbOrigem e lsbDestino, exibindo todos os caminhos no dvgCaminhos (um por linha) e o melhor
caminho no dgvMelhorCaminho. Usar retas para ligar as cidades no mapa referente ao caminho
da linha selecionada no dgvCaminhos.

Na guia [Árvore de Cidades] – exibir a árvore mostrando os números e nomes das cidades.

    */

