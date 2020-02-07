using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using TodoWinForm.Classes;
using TodoLayers.BLL;

namespace TodoWinForm
{
    public partial class frmMain : Form
    {
        //Lista que receberá o conteúdo do método GetTodoItems no Controller
        List<TodoItem> todoItems = new List<TodoItem>();
        ConvertListToDataTable<TodoItem> ConvertItemData = new ConvertListToDataTable<TodoItem>();
        HttpContent Content;
        string Uri;
        public static string API = ConfigurationManager.AppSettings["BaseAddress"];
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string mensagem = string.Empty;

            TodoItem item = new TodoItem() { Name = txtTodo.Text, IsComplete = cbbCompleted.SelectedIndex == 0 ? true : false };
            var parametros = JsonConvert.SerializeObject(item, Formatting.Indented);
            Content = new StringContent(parametros, Encoding.UTF8, "application/json");
            Uri = API + @"/api/TodoItems/" + (int)BoTodoItem.Rotas.Incluir;

            mensagem = HTTPString(Content, Uri);

            CarregaDgvTodoItem();
            MessageBox.Show(mensagem, "Teste", MessageBoxButtons.OK);
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            object retorno;

            string parametros = JsonConvert.SerializeObject(todoItems[dgvTodoItems.CurrentRow.Index], Formatting.Indented);
            Content = new StringContent(parametros, Encoding.UTF8, "application/json");
            Uri = API + @"/api/TodoItems/" + (int)BoTodoItem.Rotas.Deletar;

            retorno = HTTPString(Content, Uri);

            CarregaDgvTodoItem();
            //MessageBox.Show(retorno.ToString(), "Atenção", MessageBoxButtons.OK);
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            CarregaDgvTodoItem();
            //Se houver algum item selecionado no dataGridView, o botão para deleção é habilitado.
            btnDel.Enabled = VerificaItemSelDgv();
        }
        private void dgvTodoItems_DoubleClick(object sender, EventArgs e)
        {
            if (VerificaItemSelDgv())
            {
                TodoItem itemSelected = new TodoItem()
                {
                    Id = long.Parse(dgvTodoItems.CurrentRow.Cells[0].Value.ToString()),
                    Name = dgvTodoItems.CurrentRow.Cells[1].Value.ToString(),
                    IsComplete = bool.Parse(dgvTodoItems.CurrentRow.Cells[2].Value.ToString())
                };

                frmEdit update = new frmEdit(itemSelected);
                update.ShowDialog();
                CarregaDgvTodoItem();
            }
        }


        //APIWEB 3- método para deletar um TodoItem via GET (DeleteSync) através de um id no URL
        //Sem uso
        private string HTTPStringDel(string uri)
        {
            object Resultado = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChaveAcesso);

                    // Json de Response //
                    var response = client.DeleteAsync(new Uri(uri)).Result;


                    Resultado = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                    //Resultado = response.Content.ReadAsStringAsync().Result;



                    if (response.IsSuccessStatusCode)
                    {
                        if (Resultado != null)
                        {
                            return Resultado.ToString();
                        }
                        else
                        {
                            return "Resultado é == a null: " + Resultado;
                        }

                    }
                    else
                    {
                        MessageBox.Show(Resultado.ToString(), "Erro : IsSuccessStautosCode deu false", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Ocorreu um erro inesperado na chamada http: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Resultado.ToString();

        }

        //APIWEB 2- método para inserir um TodoItem via POST
        private string HTTPString(HttpContent conteudo, string uri)
        {
            object Resultado = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChaveAcesso);

                    // Json de Response //
                    var response = client.PostAsync(new Uri(uri), conteudo).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        Resultado = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                        //Resultado = response.Content.ReadAsStringAsync().Result;

                        if (Resultado != null)
                        {
                            return "Resultado é != de null: " + Resultado;
                        }
                        else
                        {
                            return "Resultado é == a null: " + Resultado;
                        }

                    }
                    else
                    {
                        MessageBox.Show(Resultado.ToString(), "Erro : IsSuccessStautosCode deu false", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Ocorreu um erro inesperado na chamada http: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Resultado.ToString();

        }

        //APIWEB 1- método para buscar um List<T> via POST sem parâmetro
        public static List<T> HTTPListar<T>(string Uri)
        {
            //Lista genérica
            List<T> Lista = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    //Como no Controller é HttpGet sem envio de parâmetro, basta enviar a URL do caminho para 
                    //retornar o resultado esperado.
                    var response = client.PostAsync(new Uri(Uri), null).Result;
                    
                    //Se deu certo a busca acima
                    if (response.IsSuccessStatusCode)
                    {
                        //Pegar o resultado do método GetTodoItems no Controller.
                        //Este método retorna uma IEnumerable<T> que neste caso é um List<TodoItem>
                        //Porém, armazenando num List<T> para reaproveitamento de código
                        Lista = JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        //var erro = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                        var erro = response.Content.ReadAsStringAsync().Result;
                        MessageBox.Show(erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Ocorreu um erro inesperado na chamada http: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Lista;

        }


        //Métodos para carregamento, formatação e verificação de linha selecionada.
        private void FormataDgvTodoItem()
        {
            dgvTodoItems.Columns[0].Visible = false;
        }
        private void CarregaDgvTodoItem()
        {
            //Montando uma string com o endereço do servidor (API) e o caminho URL que deverá "executar"
            Uri = API + @"/api/TodoItems/" + (int)BoTodoItem.Rotas.Pesquisar;

            //O método HTTPListar é genérico, retorna uma lista do tipo que você quiser.
            //Como é via Get (Controller) e não tem um filtro para busca, só é enviado a URL.
            todoItems = HTTPListar<TodoItem>(Uri);


            //ConvertItemData é um objeto do tipo ConvertListToDataTable (Classe abaixo)
            dgvTodoItems.DataSource = ConvertItemData.ConvertToDataTable(todoItems);
            FormataDgvTodoItem();
        }
        private bool VerificaItemSelDgv()
        {
            if (dgvTodoItems.Rows.Count > 0)
            {
                Int32 selectedRowCount = dgvTodoItems.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private void dgvTodoItems_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            btnDel.Enabled = VerificaItemSelDgv();
        }

    }


    //Classe para converter um List<T> para DataTable, deste modo, um jeito simples de incluir num DataSource
    public class ConvertListToDataTable<T>
    {
        public DataTable ConvertToDataTable(List<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
