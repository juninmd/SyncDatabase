using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CadastroClientes
{
    public partial class formCadastro : Form
    {
        public formCadastro()
        {
            InitializeComponent();
            MostraResultados();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var query = $"INSERT INTO {ConfigurationManager.AppSettings["schema"]}.cliente VALUES('{textBox2.Text}','{textBox1.Text}')";
                var guid = Guid.NewGuid();

                Directory.CreateDirectory(ConfigurationManager.AppSettings["caminhoArquivo"]);

                var filename = $"{ConfigurationManager.AppSettings["caminhoArquivo"]}\\{guid}.txt";
                var bdConn = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                bdConn.Open();
                if (bdConn.State == ConnectionState.Open)
                {
                    //Se estiver aberta insere os dados na BD
                    var commS = new MySqlCommand(query, bdConn);
                    commS.ExecuteNonQuery();
                    bdConn.Close();
                    File.WriteAllText(filename, query.Replace(ConfigurationManager.AppSettings["schema"], "#changeme#"));
                    MessageBox.Show("Gravado com Sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MostraResultados();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Deu Erro!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void MostraResultados()
        {
            var mDataSet = new DataSet();
            var mConn = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            mConn.Open();

            //cria um adapter utilizando a instrução SQL para aceder à tabela
            var mAdapter = new MySqlDataAdapter($"SELECT * FROM {ConfigurationManager.AppSettings["schema"]}.cliente", mConn);

            //preenche o dataset através do adapter
            mAdapter.Fill(mDataSet, "tabela_dados");

            //atribui o resultado à propriedade DataSource da dataGridView
            dataGridView1.DataSource = mDataSet;
            dataGridView1.DataMember = "tabela_dados";
        }
    }
}
