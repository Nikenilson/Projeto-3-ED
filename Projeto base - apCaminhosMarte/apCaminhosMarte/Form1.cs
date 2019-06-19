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
        //Método que, efetivamente, busca caminhos entre as cidades do mapa.
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            //Variáveis para operações do btnBuscar.
            int idCidadeOrigem = lsbOrigem.SelectedIndex;
            int idCidadeDestino = lsbDestino.SelectedIndex;
            int cidadeAtual = idCidadeOrigem;
            int saidaAtual = 0;
            bool[] passou = new bool[qtdCidades];
            bool achou = false;
            bool acabou = false;
            List<PilhaLista<Caminho>> caminhosValidos = new List<PilhaLista<Caminho>>();
            PilhaLista<Caminho> p = new PilhaLista<Caminho>();

            for (int i = 0; i < qtdCidades; i++) //Inicia os valores de “passou”    
                passou[i] = false; //Pois ainda não foi em nenhuma cidade  

            int indice = 0;
            while (!acabou) //Enquanto não acabou
            {
                while ((saidaAtual < qtdCidades) && !achou)
                {   //Se não há saida pela cidade testada, verifica a próxima.    
                    if (matriz[cidadeAtual,saidaAtual] == null)
                        saidaAtual++;
                    else //Se já passou pela cidade testada, verifica se a próxima 
                            //cidade permite saída.
                        if (passou[saidaAtual])
                        saidaAtual++;
                    else     //Se chegou na cidade desejada, empilha o local     
                                //e termina o processo de procura de caminho.
                    if (saidaAtual == idCidadeDestino) //Se a saída é o id do destino,
                    {
                        p.Empilhar(new Caminho(cidadeAtual, saidaAtual)); //Encontramos o destino.
                        achou = true;
                    }
                    else
                    {
                        p.Empilhar(new Caminho(cidadeAtual, saidaAtual)); //Vamos a outra cidade, isto é, à cidade de saída.
                        passou[cidadeAtual] = true;
                        cidadeAtual = saidaAtual;
                        saidaAtual = 0;
                    }
                }
                if (!achou)
                    if (!p.EstaVazia())
                    {
                        //Backtracking --> Retorna o caminho.
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
                {  //Desempilha a configuração atual da pilha      
                   //Para a pilha da lista de parâmetros  

                    caminhosValidos.Add(new PilhaLista<Caminho>());
                    PilhaLista<Caminho> auxiliar = new PilhaLista<Caminho>();
                    while (!p.EstaVazia())
                    {
                        auxiliar.Empilhar(p.Desempilhar()); //Invertemos os caminhos.
                        caminhosValidos[indice].Empilhar(auxiliar.OTopo());
                    }
                    while(!auxiliar.EstaVazia())
                    {
                        p.Empilhar(auxiliar.Desempilhar());
                    }
                    indice++;
                    achou = false;
                    //Backtracking --> Retorna o caminho.
                    Caminho aux = p.Desempilhar();
                    saidaAtual = aux.IdCidadeDestino;
                    passou[saidaAtual] = false;
                    cidadeAtual = aux.IdCidadeOrigem;
                    aux = null;
                    saidaAtual++;
                }
            }
            //Método que mostra todos os caminhos no dgvCaminho.
            TodosOsCaminhos(caminhosValidos, idCidadeOrigem, idCidadeDestino);
        }
        //Exibe todos os caminhos validos no dgv
        public void TodosOsCaminhos(List<PilhaLista<Caminho>> caminhosValidos, int idCidadeOrigem, int idCidadeDestino)
        {
            //Limpa o DataGridView
            dgvCaminho.Rows.Clear();
            dgvCaminho.ColumnCount = 23; //Número máximo de cidades

            //Se não ha caminhos válidos, avisa ao usuário 
            if (caminhosValidos.Count == 0)
                MessageBox.Show("Nenhum caminho encontrado!");
            else
            {
                //Percorre os caminhosValidos
                for (int i = caminhosValidos.Count - 1; i >= 0; i--)
                {
                    //Clonamos a pilha e à armazenamos em uma variavel auxiliar, pois ele será usada no futuro.
                    PilhaLista<Caminho> auxiliar = (PilhaLista<Caminho>)caminhosValidos[i].Clone();
                    Caminho caminhoAux;
                    string[] row = new string[Convert.ToInt32(Math.Pow(qtdCidades, 2) * 2)];
                    int aux = 0;
                    
                    //Utiliza o método Existe para posicionar o atual da arvore
                    arvore.Existe(new Cidade(idCidadeOrigem));
                    row[aux++] = idCidadeOrigem + " - " + arvore.Atual.Info.Nome;
                    while (!auxiliar.EstaVazia())
                    {
                        //Até que a pilha auxiliar se esvazie, adiciona os caminhos no DataGridView
                        caminhoAux = auxiliar.Desempilhar();
                        arvore.Existe(new Cidade(caminhoAux.IdCidadeDestino));
                        row[aux++] = caminhoAux.IdCidadeDestino + " - " + arvore.Atual.Info.Nome;
                    }
                    dgvCaminho.Rows.Add(row);
                }
                //Após a exibição dos caminhos no dgvCaminhos, chama o método MenorCaminho para exibir o Melhor Caminho  
                MenorCaminho(caminhosValidos, idCidadeOrigem, idCidadeDestino);
            }
        }
        //Método que encontra o menor/melhor caminho.
        public void MenorCaminho(List<PilhaLista<Caminho>> caminhosValidos,int idCidadeOrigem, int idCidadeDestino)
        {
            //Limpa o DataGridView
            dgvMelhorCaminho.Rows.Clear();
            dgvMelhorCaminho.ColumnCount = 23;//Numero maximo e cidades
            int menorDistancia = 0;
            int indiceMenor = 0;
            List<PilhaLista<Caminho>> aux = new List<PilhaLista<Caminho>>();

            //Pega a soma das distancias dos caminhos da primeira pilha pra futuramente compará-la com as outras 
            while (!caminhosValidos[0].EstaVazia())
            {
                //Adiciona um clone do Caminho atual em uma variavel auxiliar para recuperar ele depois
                aux.Add((PilhaLista<Caminho>) caminhosValidos[0].Clone());
                menorDistancia =+ caminhosValidos[0].Desempilhar().Distancia;
            }
            
            //Percorre os caminhos válidos comparando as distancias a dim de achar a menor delas
            for (int auxI = 1; auxI < caminhosValidos.Count; auxI++)
            {
                //Adiciona um clone do Caminho atual em uma variavel auxiliar para recuperar ele depois
                aux.Add((PilhaLista<Caminho>) caminhosValidos[auxI].Clone());
                int distancia = 0;
                while (!caminhosValidos[auxI].EstaVazia())
                    distancia =+ caminhosValidos[auxI].Desempilhar().Distancia;

                //Se a distancia desse caminho for menor que a menor das distancias, essa se torna a menor distancias
                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    indiceMenor = auxI;
                }
            }

            //Repreenche a lista de caminhos, para que possamos indexar o indice de menor distancia
            for (int auxI = 0; auxI < aux.Count; auxI++)
            {
                caminhosValidos.Clear();
                caminhosValidos.Add(aux[auxI]);
            }

            PilhaLista<Caminho> melhorCaminho = caminhosValidos[indiceMenor];

            string[] rowMelhorCaminho = new string[Convert.ToInt32(Math.Pow(qtdCidades, 2))];
            int i = 0;
            Caminho caminhoAux;

            //Utiliza o método Existe para posicionar o atual da arvore
            arvore.Existe(new Cidade(idCidadeOrigem));
            rowMelhorCaminho[i++] = idCidadeOrigem + " - " + arvore.Atual.Info.Nome;

            //Até que o caminho se esvazie, adiciona os caminhos no DataGridView
            while (!melhorCaminho.EstaVazia())
            {
                caminhoAux = melhorCaminho.Desempilhar();
                
                arvore.Existe(new Cidade(caminhoAux.IdCidadeDestino));
                rowMelhorCaminho[i++] = caminhoAux.IdCidadeDestino + " - " + arvore.Atual.Info.Nome;
            }
            //Adiciona os caminhos no DataGriedView
            dgvMelhorCaminho.Rows.Add(rowMelhorCaminho);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Abertura do primeiro arquivo --> CidadesMarte.txt
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

            //Abertura do segundo arquivo --> CaminhosEntreCidadesMarte.txt
            var arq2 = new StreamReader("CaminhosEntreCidadesMarte.txt");
            matriz = new Caminho[qtdCidades, qtdCidades];
            string linha2 = null;
            while (!arq2.EndOfStream)
            {
                linha2 = arq2.ReadLine();
                matriz[int.Parse(linha2.Substring(0, 3)), int.Parse(linha2.Substring(3, 3))] = new Caminho(linha2);
            }
            arq2.Close();

            //Abertura do terceiro arquivo --> CidadesMarteOrdenado.txt
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

           
        }

        //Método para escrever os caminhos do mapa deduzidos em uma notação de DataGridView.
        private void dgvMelhorCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Limpa a lista de cidades antes selecionadas
            listaCidades.Clear();
            for (int c = 0; c < (sender as DataGridView).ColumnCount; c++)
            {
                //Verifica se o click foi em uma celula válida
                if (e.RowIndex < 0 || (sender as DataGridView).Rows[e.RowIndex].Cells[c].Value == null)
                    break;

                //Pega o código da cidade na celula em que o usuário clickou 
                string linha = (sender as DataGridView).Rows[e.RowIndex].Cells[c].Value.ToString();
                string[] linhaVetor = linha.Split('-');

                //Adiciona a cidade a ser desenhada na lista de cidades, para que o método DesenhaLinhasCaminho possa 
                //desenhar o caminho quando o usuario clicar em um caminho
                listaCidades.Add(new Cidade(Convert.ToInt32(linhaVetor[0])));
            }
            pbMapa.Invalidate(); //Invalida um desenho para que possa ser feito outro.
        }
        //Método que pinta o mapa na tela do formulário.
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            DesenhaCidades(g, arvore.Raiz);
            DesenhaLinhas(g);
            DesenhaLinhasCaminho(g);
        }
        //Desenha a árvore binária na segunda tabpage do formulário.
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
        //Desenhando as linhas no mapa do formulário.
        private void DesenhaLinhas(Graphics g)
        {   
            Caminho aux = null;
            Cidade c1 = null;
            Cidade c2 = null;
            Pen minhaCaneta = new Pen(Color.DarkGreen, 4);
            //Faz com que as linhas tenha setas nas pontas
            minhaCaneta.CustomEndCap = new AdjustableArrowCap(4, 6);

            //Percorre a matriz de caminhos desenhando todas as cidades e caminhos 
            for (int linhas = 0; linhas < qtdCidades; linhas++)
                for (int colunas = 0; colunas < qtdCidades; colunas++)
                {
                    if (matriz[linhas, colunas] != null)
                    {
                        aux = matriz[linhas, colunas];

                        //Busca as cidades com o id na arvore
                        c1 = arvore.BuscarDado(new Cidade(linhas));
                        c2 = arvore.BuscarDado(new Cidade(colunas));

                        //Pega as cordenadas de cada cidade para desenha-las no picture box
                        float xI = pbMapa.Size.Width * c1.CoordenadaX / 4096;
                        float yI = pbMapa.Size.Height * c1.CoordenadaY / 2048;
                        float xF = pbMapa.Size.Width * c2.CoordenadaX / 4096;
                        float yF = pbMapa.Size.Height * c2.CoordenadaY / 2048;

                        //Onde I => Inicial, F => Final

                        //Duas exceções, o caminho de Senzeni Na até Gondor e de Arrakeen até Gondor
                        //As duas são caminhos que devem passar por fora do mapa, pois o planeta é uma esfera
                        if ((c1.IdCidade == 2 || c1.IdCidade == 18) && (c2.IdCidade == 10))
                        {
                            g.DrawLine(minhaCaneta, xI, yI, -xF, yF);
                            g.DrawLine(minhaCaneta, 2048 + xI, yI, xF, yF);
                        }
                        else //O resto é tratado normalmente e desenhado com as coordenadas passadas 
                            g.DrawLine(minhaCaneta, xI, yI, xF, yF);
                    }
                }
        }
        //Desenhando as linhas específicas do caminho selecionado.
        private void DesenhaLinhasCaminho(Graphics g)
        {
            //Se a lista de cidades não está vazia, existem caminhos á serem desenhados
            if (listaCidades.Count > 0)
            {
                Pen minhaCaneta = new Pen(Color.Purple, 4);
                //Faz com que as linhas tenha setas nas pontas
                minhaCaneta.CustomEndCap = new AdjustableArrowCap(5, 8);
                SolidBrush meuPincel = new SolidBrush(Color.Black);
                //Onde I => Inicial, F => Final
                float xF = -1;
                float yF = -1;
                float xI = -1;
                float yI = -1;
                Cidade aux = null;

                //Percorre a lista de cidades
                for (int i = 0; i < listaCidades.Count; i++)
                {
                    //Pega a cidade com o idCidade passado 
                    Cidade c1 = arvore.BuscarDado(new Cidade(listaCidades[i].IdCidade));

                    //Pega as coordenadas 
                    xF = pbMapa.Size.Width * c1.CoordenadaX / 4096;
                    yF = pbMapa.Size.Height * c1.CoordenadaY / 2048;

                    //Duas exceções, o caminho de Senzeni Na até Gondor e de Arrakeen até Gondor
                    //As duas são caminhos que devem passar por fora do mapa, pois o planeta é uma esfera
                    if ((aux != null &&( aux.IdCidade == 2 || aux.IdCidade == 18)) && ( c1.IdCidade == 10))
                    {
                        g.DrawLine(minhaCaneta, xI, yI, -xF, yF);
                        g.DrawLine(minhaCaneta, 2048 + xI, yI, xF, yF);
                    }
                    else //O resto e tratado normalmente
                    {
                        //O caminho so pode ser desenhado entre duas cidades, como o for pega uma por vez
                        //Na primeira vez não será possivel desenhar o caminho
                        if (xI > -1 && yI > -1)
                            g.DrawLine(minhaCaneta, xI, yI, xF, yF);
                    }

                    //"Incrementa" as variaveis
                    xI = xF;
                    yI = yF;
                    aux = c1;
                }
            }
        }  
        //Desenhando as cidades que compõem o Planeta Marte.
        private void DesenhaCidades(Graphics g, NoArvore<Cidade> atualRecursivo)
        {
            if (atualRecursivo != null) //A primeira coisa a se fazer em uma recursão é sair dela
            {
                SolidBrush meuPincel = new SolidBrush(Color.Black);
                //Pega as coordenadas da cidade a tual e desenha ela no picturebox
                float x = pbMapa.Size.Width * atualRecursivo.Info.CoordenadaX / 4096 - 8;
                float y = pbMapa.Size.Height * atualRecursivo.Info.CoordenadaY / 2048 - 8;
                g.FillEllipse(meuPincel, x, y, 16, 16);
                g.DrawString(atualRecursivo.Info.Nome, new Font("Comic Sans", 12, FontStyle.Regular), meuPincel, x - 5, y + 15);

                //Continua o processo recursivamente
                DesenhaCidades(g, atualRecursivo.Esq);
                DesenhaCidades(g, atualRecursivo.Dir);
            }
        }
        private void DrawFrame(object sender, EventArgs e)
        {
            pbMapa.Invalidate();
        }
        //Evento que chama o método de desenho da árvore.
        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            //Desenha a Arvore
            Graphics g = e.Graphics;
            DesenhaArvore(true, arvore.Raiz, (int)pnlArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, g);
        }
    }
}