using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace SistemaSolarApp
{
    public partial class MainForm : Form
    {
        private List<Planeta> planetas;
        private FlowLayoutPanel pnlCards; 
        private string filePath = "planetas.json";

        public MainForm()
        {
            ConfigurarInterface();
            CarregarDados();
        }

        private void ConfigurarInterface()
        {
            // Configurações da Janela Principal
            this.Text = "Explorador do Sistema Solar - Modo Galeria";
            this.Size = new Size(1100, 750);
            this.BackColor = Color.FromArgb(10, 10, 25);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Cabeçalho Estilizado
            Label lblTitulo = new Label
            {
                Text = "SISTEMA SOLAR",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.Gold,
                Dock = DockStyle.Top,
                Height = 90,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Container Principal dos Cards (Equivalente a um Flex Container)
            pnlCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(25),
                BackColor = Color.Transparent
            };

            // Barra de Ações Inferior
            FlowLayoutPanel panelAcoes = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 85,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(25, 18, 0, 0),
                BackColor = Color.FromArgb(15, 15, 35)
            };

            panelAcoes.Controls.Add(CriarBotao("Cadastrar Novo", Color.ForestGreen, (s, e) => CadastrarPlaneta()));
            panelAcoes.Controls.Add(CriarBotao("Sair", Color.FromArgb(60, 60, 60), (s, e) => Application.Exit()));

            // Adicionando controles ao Form
            this.Controls.Add(pnlCards);
            this.Controls.Add(panelAcoes);
            this.Controls.Add(lblTitulo);
        }

        private void AtualizarCards()
        {
            // Limpa o container antes de renderizar (Similar ao innerHTML = "" no JS)
            pnlCards.Controls.Clear();
            
            foreach (var p in planetas)
            {
                pnlCards.Controls.Add(CriarCardPlaneta(p));
            }
        }

        private Panel CriarCardPlaneta(Planeta p)
        {
            // Container do Card
            Panel card = new Panel
            {
                Size = new Size(250, 340),
                BackColor = Color.FromArgb(35, 35, 70),
                Margin = new Padding(15)
            };

            // Badge/Nome do Planeta
            Label lblNome = new Label
            {
                Text = p.Nome.ToUpper(),
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.DeepSkyBlue,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Corpo de Texto (Dados Técnicos)
            Label lblDados = new Label
            {
                Text = $"✨ Rotação: {p.Rotacao}h\n\n" +
                       $"🚀 Translação: {p.Translacao}d\n\n" +
                       $"📏 Diâmetro: {p.Diametro}km\n\n" +
                       $"🌡️ Temp: {p.Temperatura}°C\n\n" +
                       $"☀️ Dist. Sol: {p.Distancia}M km",
                Font = new Font("Segoe UI Semibold", 10),
                ForeColor = Color.WhiteSmoke,
                Location = new Point(20, 65),
                Size = new Size(210, 180)
            };

            // Botão de Link (Simulando uma Anchor Tag)
            Button btnLink = new Button
            {
                Text = "🔗 Abrir Imagem",
                Size = new Size(210, 40),
                Location = new Point(20, 260),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gold,
                Cursor = Cursors.Hand
            };
            btnLink.FlatAppearance.BorderColor = Color.Gold;
            btnLink.Click += (s, e) => AbrirLink(p.Imagem);

            // Botão de Deletar (Floating no canto superior direito)
            Button btnDelete = new Button
            {
                Text = "✕",
                Size = new Size(30, 30),
                Location = new Point(215, 5),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(255, 80, 80),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => {
                planetas.Remove(p);
                SalvarDados();
                AtualizarCards();
            };

            card.Controls.Add(btnDelete);
            card.Controls.Add(btnLink);
            card.Controls.Add(lblDados);
            card.Controls.Add(lblNome);

            return card;
        }

        private void CarregarDados()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                planetas = JsonSerializer.Deserialize<List<Planeta>>(json);
            }
            else
            {
                planetas = new List<Planeta>
                {
                    new Planeta { Nome = "Mercúrio", Rotacao = "1407", Translacao = "88", Diametro = "4879", Temperatura = "167", Distancia = "57.9", Imagem = "https://bit.ly/3uN6b8k" },
                    new Planeta { Nome = "Vênus", Rotacao = "-5832", Translacao = "224.7", Diametro = "12104", Temperatura = "464", Distancia = "108.2", Imagem = "https://bit.ly/3uRA7Qe" },
                    new Planeta { Nome = "Terra", Rotacao = "24", Translacao = "365.2", Diametro = "12742", Temperatura = "15", Distancia = "149.6", Imagem = "https://bit.ly/3uO7q5L" },
                    new Planeta { Nome = "Marte", Rotacao = "24.6", Translacao = "687", Diametro = "6779", Temperatura = "-65", Distancia = "227.9", Imagem = "https://bit.ly/3Id8T8w" },
                    new Planeta { Nome = "Júpiter", Rotacao = "9.9", Translacao = "4333", Diametro = "139822", Temperatura = "-110", Distancia = "778.6", Imagem = "https://bit.ly/3uN6P6s" },
                    new Planeta { Nome = "Saturno", Rotacao = "10.7", Translacao = "10759", Diametro = "116460", Temperatura = "-140", Distancia = "1433", Imagem = "https://bit.ly/3T6oYp9" },
                    new Planeta { Nome = "Urano", Rotacao = "-17.2", Translacao = "30687", Diametro = "50724", Temperatura = "-195", Distancia = "2872", Imagem = "https://bit.ly/3uL8Y9r" },
                    new Planeta { Nome = "Netuno", Rotacao = "16.1", Translacao = "60190", Diametro = "49244", Temperatura = "-201", Distancia = "4495", Imagem = "https://bit.ly/3T2Z9R1" }
                };
                SalvarDados();
            }
            // Importante: Agora chamamos o renderizador de cards
            AtualizarCards();
        }

        private void SalvarDados()
        {
            string json = JsonSerializer.Serialize(planetas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private void CadastrarPlaneta()
        {
            string novoNome = Microsoft.VisualBasic.Interaction.InputBox("Digite o nome do planeta:", "Novo Registro");
            if (!string.IsNullOrWhiteSpace(novoNome))
            {
                planetas.Add(new Planeta { 
                    Nome = novoNome, 
                    Rotacao = "0", 
                    Translacao = "0", 
                    Diametro = "0", 
                    Temperatura = "0", 
                    Distancia = "0", 
                    Imagem = "https://google.com" 
                });
                SalvarDados();
                AtualizarCards();
            }
        }

        private void AbrirLink(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
                MessageBox.Show("Não foi possível abrir o link da imagem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Button CriarBotao(string texto, Color cor, EventHandler evento)
        {
            Button btn = new Button
            {
                Text = texto,
                BackColor = cor,
                ForeColor = Color.White,
                Size = new Size(150, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 15, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += evento;
            return btn;
        }
    }
}
