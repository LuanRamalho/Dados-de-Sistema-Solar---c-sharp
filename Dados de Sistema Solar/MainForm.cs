using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using System.Diagnostics;

namespace SistemaSolarApp
{
    public partial class MainForm : Form
    {
        private List<Planeta> planetas;
        private DataGridView dgvPlanetas;
        private string filePath = "planetas.json";

        public MainForm()
        {
            ConfigurarInterface();
            CarregarDados();
        }

        private void ConfigurarInterface()
        {
            // Configurações da Janela
            this.Text = "Explorador do Sistema Solar";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.FromArgb(15, 15, 35); // Azul espacial profundo
            this.StartPosition = FormStartPosition.CenterScreen;

            // Painel de Título
            Label lblTitulo = new Label
            {
                Text = "SISTEMA SOLAR",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.Gold,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Configuração do DataGridView
            dgvPlanetas = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 350,
                BackgroundColor = Color.FromArgb(25, 25, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                ReadOnly = true
            };

            dgvPlanetas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 80);
            dgvPlanetas.ColumnHeadersDefaultCellStyle.ForeColor = Color.Yellow;
            dgvPlanetas.EnableHeadersVisualStyles = false;
            dgvPlanetas.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 60);

            // Adicionando Colunas
            dgvPlanetas.Columns.Add("Nome", "Nome");
            dgvPlanetas.Columns.Add("Rotacao", "Rotação (h)");
            dgvPlanetas.Columns.Add("Translacao", "Translação (d)");
            dgvPlanetas.Columns.Add("Diametro", "Diâmetro (km)");
            dgvPlanetas.Columns.Add("Temp", "Temp (°C)");
            dgvPlanetas.Columns.Add("Distancia", "Dist. Sol (M km)");
            
            // Coluna de Link clicável
            DataGridViewLinkColumn linkCol = new DataGridViewLinkColumn
            {
                Name = "Imagem",
                HeaderText = "Link da Imagem",
                DataPropertyName = "Imagem",
                LinkColor = Color.Cyan,
                ActiveLinkColor = Color.White
            };
            dgvPlanetas.Columns.Add(linkCol);
            dgvPlanetas.CellContentClick += DgvPlanetas_CellContentClick;

            // Painel de Botões
            FlowLayoutPanel panelBotoes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(20),
                BackColor = Color.Transparent
            };

            panelBotoes.Controls.Add(CriarBotao("Cadastrar", Color.ForestGreen, (s, e) => CadastrarPlaneta()));
            panelBotoes.Controls.Add(CriarBotao("Excluir", Color.Crimson, (s, e) => DeletarPlaneta()));
            panelBotoes.Controls.Add(CriarBotao("Sair", Color.Gray, (s, e) => Application.Exit()));

            this.Controls.Add(panelBotoes);
            this.Controls.Add(dgvPlanetas);
            this.Controls.Add(lblTitulo);
        }

        private Button CriarBotao(string texto, Color cor, EventHandler evento)
        {
            Button btn = new Button
            {
                Text = texto,
                BackColor = cor,
                ForeColor = Color.White,
                Size = new Size(120, 45),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += evento;
            return btn;
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
                // Dados Fixos dos 8 Planetas
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
            AtualizarGrid();
        }

        private void SalvarDados()
        {
            string json = JsonSerializer.Serialize(planetas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private void AtualizarGrid()
        {
            dgvPlanetas.Rows.Clear();
            foreach (var p in planetas)
            {
                dgvPlanetas.Rows.Add(p.Nome, p.Rotacao, p.Translacao, p.Diametro, p.Temperatura, p.Distancia, p.Imagem);
            }
        }

        private void DgvPlanetas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPlanetas.Columns["Imagem"].Index && e.RowIndex >= 0)
            {
                string url = dgvPlanetas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); }
                catch { MessageBox.Show("Não foi possível abrir o link."); }
            }
        }

        private void CadastrarPlaneta()
        {
            // Para simplificar, vamos simular uma entrada rápida. 
            // Em um projeto real, você abriria um novo Form com TextBoxes.
            string novoNome = Microsoft.VisualBasic.Interaction.InputBox("Nome do Planeta:", "Novo Cadastro");
            if (!string.IsNullOrEmpty(novoNome))
            {
                planetas.Add(new Planeta { Nome = novoNome, Rotacao = "0", Translacao = "0", Diametro = "0", Temperatura = "0", Distancia = "0", Imagem = "http://" });
                SalvarDados();
                AtualizarGrid();
            }
        }

        private void DeletarPlaneta()
        {
            if (dgvPlanetas.SelectedRows.Count > 0)
            {
                string nome = dgvPlanetas.SelectedRows[0].Cells[0].Value.ToString();
                planetas.RemoveAll(p => p.Nome == nome);
                SalvarDados();
                AtualizarGrid();
            }
        }
    }
}