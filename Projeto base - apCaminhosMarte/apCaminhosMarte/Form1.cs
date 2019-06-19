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
using System.Drawing.Drawing2D;

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
        List<Cidade> listaCidades = new List<Cidade>();
        public Form1()
        {
            InitializeComponent();
        }
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            int idCidadeOrigem = lsbOrigem.SelectedIndex;
            int idCidadeDestino = lsbDestino.SelectedIndex;
            int cidadeAtual = idCidadeOrigem;
            int saidaAtual = 0;
            bool[] passou = new bool[qtdCidades];
            bool achou = false;
            bool acabou = false;

            List<PilhaLista<Caminho>> caminhosValidos = new List<PilhaLista<Caminho>>();
            PilhaLista<Caminho> p = new PilhaLista<Caminho>();

            for (int i = 0; i < qtdCidades; i++) // inicia os valores de “passou”    
                passou[i] = false; // pois ainda não foi em nenhuma cidade  

            int indice = 0;
            while (!acabou) //Mudar condicao
            {
                while ((saidaAtual < qtdCidades) && !achou)
                {   // se não há saida pela cidade testada, verifica a próxima      
                    if (matriz[cidadeAtual,saidaAtual] == null)
                        saidaAtual++;
                    else // se já passou pela cidade testada, verifica se a próxima 
                            // cidade permite saida  
                        if (passou[saidaAtual])
                        saidaAtual++;
                    else     // se chegou na cidade desejada, empilha o local     
                                // e termina o processo de procura de caminho    
                    if (saidaAtual == idCidadeDestino)
                    {
                        p.Empilhar(new Caminho(cidadeAtual, saidaAtual));
                        achou = true;
                    }
                    else
                    {
                        p.Empilhar(new Caminho(cidadeAtual, saidaAtual));
                        passou[cidadeAtual] = true;
                        cidadeAtual = saidaAtual;
                        saidaAtual = 0;
                    }
                }
                if (!achou)
                    if (!p.EstaVazia())
                    {
                        //Backtracking
                        Caminho aux = p.Desempilhar();
                        saidaAtual = aux.IdCidadeDestino;
                        passou[saidaAtual] = false;
                        cidadeAtual = aux.IdCidadeOrigem;
                        aux = null;
                        saidaAtual++;
                    }
                    else
                        acabou = true;
                if (achou)
                {  // desempilha a configuração atual da pilha      
                   // para a pilha da lista de parâmetros  

                    caminhosValidos.Add(new PilhaLista<Caminho>());
                    PilhaLista<Caminho> auxiliar = new PilhaLista<Caminho>();
                    while (!p.EstaVazia())
                    {
                        auxiliar.Empilhar(p.Desempilhar());
                        caminhosValidos[indice].Empilhar(auxiliar.OTopo());
                    }
                    while(!auxiliar.EstaVazia())
                    {
                        p.Empilhar(auxiliar.Desempilhar());
                    }
                    indice++;
                    achou = false;
                    //Backtracking
                    Caminho aux = p.Desempilhar();
                    saidaAtual = aux.IdCidadeDestino;
                    passou[saidaAtual] = false;
                    cidadeAtual = aux.IdCidadeOrigem;
                    aux = null;
                    saidaAtual++;
                }
            }
            //Mostra todos os caminhos no dgvCaminhos
            TodosOsCaminhos(caminhosValidos, idCidadeOrigem, idCidadeDestino);
        }
        public void TodosOsCaminhos(List<PilhaLista<Caminho>> caminhosValidos, int idCidadeOrigem, int idCidadeDestino)
        {
            dgvCaminho.Rows.Clear();
            dgvCaminho.ColumnCount = 23; //Numero maximo e cidades
            if (caminhosValidos.Count == 0)
                MessageBox.Show("Nenhum caminho encontrado!");
            else
            {
                for (int i = caminhosValidos.Count - 1; i >= 0; i--)
                {
                    PilhaLista<Caminho> auxiliar = (PilhaLista<Caminho>)caminhosValidos[i].Clone();
                    string[] row = new string[Convert.ToInt32(Math.Pow(qtdCidades, 2) * 2)];
                    int aux = 0;
                    Caminho caminhoAux;
                    arvore.Existe(new Cidade(idCidadeOrigem));
                    row[aux++] = idCidadeOrigem + " - " + arvore.Atual.Info.Nome;
                    while (!auxiliar.EstaVazia())
                    {
                        caminhoAux = auxiliar.Desempilhar();
                        arvore.Existe(new Cidade(caminhoAux.IdCidadeDestino));
                        row[aux++] = caminhoAux.IdCidadeDestino + " - " + arvore.Atual.Info.Nome;
                    }
                    dgvCaminho.Rows.Add(row);
                }
                MenorCaminho(caminhosValidos, idCidadeOrigem, idCidadeDestino);
            }
        }
        public void MenorCaminho(List<PilhaLista<Caminho>> caminhosValidos,int idCidadeOrigem, int idCidadeDestino)
        {
            dgvMelhorCaminho.Rows.Clear();
            dgvMelhorCaminho.ColumnCount = 23;//Numero maximo e cidades
            int menorDistancia = 0;
            int indiceMenor = 0;
            List<PilhaLista<Caminho>> aux = new List<PilhaLista<Caminho>>();
            while (!caminhosValidos[0].EstaVazia())
            {
                aux.Add((PilhaLista<Caminho>) caminhosValidos[0].Clone());
                menorDistancia =+ caminhosValidos[0].Desempilhar().Distancia;
            }
          
            for (int auxI = 1; auxI < caminhosValidos.Count; auxI++)
            {
                aux.Add((PilhaLista<Caminho>) caminhosValidos[auxI].Clone());
                int distancia = 0;
                while (!caminhosValidos[auxI].EstaVazia())
                    distancia =+ caminhosValidos[auxI].Desempilhar().Distancia;

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    indiceMenor = auxI;
                }
            }

            for (int auxI = 0; auxI < aux.Count; auxI++)
            {
                caminhosValidos.Clear();
                caminhosValidos.Add(aux[auxI]);
            }

            PilhaLista<Caminho> melhorCaminho = caminhosValidos[indiceMenor];

            string[] rowMelhorCaminho = new string[Convert.ToInt32(Math.Pow(qtdCidades, 2))];
            int i = 0;
            Caminho caminhoAux;

            arvore.Existe(new Cidade(idCidadeOrigem));
            rowMelhorCaminho[i++] = idCidadeOrigem + " - " + arvore.Atual.Info.Nome;

            while (!melhorCaminho.EstaVazia())
            {
                caminhoAux = melhorCaminho.Desempilhar();
                
                arvore.Existe(new Cidade(caminhoAux.IdCidadeDestino));
                rowMelhorCaminho[i++] = caminhoAux.IdCidadeDestino + " - " + arvore.Atual.Info.Nome;
            }
            
            dgvMelhorCaminho.Rows.Add(rowMelhorCaminho);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //arq 1
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

            //arq2
            var arq2 = new StreamReader("CaminhosEntreCidadesMarte.txt");
            matriz = new Caminho[qtdCidades, qtdCidades];
            string linha2 = null;
            while (!arq2.EndOfStream)
            {
                linha2 = arq2.ReadLine();
                matriz[int.Parse(linha2.Substring(0, 3)), int.Parse(linha2.Substring(3, 3))] = new Caminho(linha2);
            }
            arq2.Close();

            //arq3
            var arq3 = new StreamReader("CidadesMarteOrdenado.txt");
            lsbOrigem.Items.Clear();
            lsbDestino.Items.Clear();

            string linha3 = null;
            while (!arq3.EndOfStream)
            {
                linha3 = arq3.ReadLine();
                lsbOrigem.Items.Add(linha3.Substring(0, 3) + " - " + linha3.Substring(3, 15));
                lsbDestino.Items.Add(linha3.Substring(0, 3) + " - " + linha3.Substring(3, 15));
            }
            arq3.Close();

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
        private void dgvMelhorCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //listaCaminhos.Clear();
            listaCidades.Clear();
            for (int c = 0; c < (sender as DataGridView).ColumnCount; c++)
            {
                if (e.RowIndex < 0 || (sender as DataGridView).Rows[e.RowIndex].Cells[c].Value == null)
                    break;
                string linha = (sender as DataGridView).Rows[e.RowIndex].Cells[c].Value.ToString();
                string[] linhaVetor = linha.Split('-');

                listaCidades.Add(new Cidade(Convert.ToInt32(linhaVetor[0])));
            }
            pbMapa.Invalidate();
        }
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            DesenhaCidades(g, arvore.Raiz);
            DesenhaLinhas(g);
            DesenhaLinhasCaminho(g);

        }
        private void DesenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,int x, int y, double angulo, double incremento, double comprimento, Graphics g)
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
                DesenhaArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                                    incremento * 0.60, comprimento * 0.8, g);
                DesenhaArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
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
            Pen minhaCaneta = new Pen(Color.DarkGreen, 4);
            minhaCaneta.CustomEndCap = new AdjustableArrowCap(4, 6);
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

                        if ((c1.IdCidade == 2 || c1.IdCidade == 18) && (c2.IdCidade == 10))
                        {
                            g.DrawLine(minhaCaneta, xI, yI, -xF, yF);
                            g.DrawLine(minhaCaneta, 2048 + xI, yI, xF, yF);
                        }
                        else
                            g.DrawLine(minhaCaneta, xI, yI, xF, yF);
                    }
                }
        }
        private void DesenhaLinhasCaminho(Graphics g)
        {
            Pen minhaCaneta = new Pen(Color.Purple, 4);
            minhaCaneta.CustomEndCap = new AdjustableArrowCap(5, 8);
            SolidBrush meuPincel = new SolidBrush(Color.Black);
            if (listaCidades.Count > 0)
            {
                float xF = -1;
                float yF = -1;
                float xI = -1;
                float yI = -1;
                Cidade aux = null;

                for (int i = 0; i < listaCidades.Count; i++)
                {
                    Cidade c1 = arvore.BuscarDado(new Cidade(listaCidades[i].IdCidade));

                    xF = pbMapa.Size.Width * c1.CoordenadaX / 4096;
                    yF = pbMapa.Size.Height * c1.CoordenadaY / 2048;

                    if ((aux != null &&( aux.IdCidade == 2 || aux.IdCidade == 18)) && ( c1.IdCidade == 10))
                    {
                        g.DrawLine(minhaCaneta, xI, yI, -xF, yF);
                        g.DrawLine(minhaCaneta, 2048 + xI, yI, xF, yF);
                    }
                    else
                    {
                        if (xI > -1 && yI > -1)
                            g.DrawLine(minhaCaneta, xI, yI, xF, yF);
                    }
                    xI = xF;
                    yI = yF;
                    aux = c1;
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
            DesenhaArvore(true, arvore.Raiz, (int)pnlArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, g);
        }
        void dgvCaminho_OnRowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }
    }
}