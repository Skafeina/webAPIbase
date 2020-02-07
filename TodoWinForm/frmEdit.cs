using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TodoLayers.BLL;
using TodoWinForm.Classes;

namespace TodoWinForm
{
    public partial class frmEdit : Form
    {
        string Uri;
        public static string API = ConfigurationManager.AppSettings["BaseAddress"];
        private TodoItem _item;
        public frmEdit()
        {
            InitializeComponent();
        }

        public frmEdit(TodoItem item)
        {
            InitializeComponent();
            _item = item;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            object retorno;

            _item.Name = txtTodo.Text;
            _item.IsComplete = cbbCompleted.SelectedIndex == 0 ? true : false;

            var parametros = JsonConvert.SerializeObject(_item, Formatting.Indented);
            HttpContent Content = new StringContent(parametros, Encoding.UTF8, "application/json");
            Uri = API + @"/api/TodoItems/" + (int)BoTodoItem.Rotas.Alterar;

            retorno = HTTPStringUpdate(Uri, Content);
            Close();
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            txtTodo.Text = _item.Name;
            cbbCompleted.SelectedIndex = _item.IsComplete == true ? 0 : 1; 
        }

        private string HTTPStringUpdate(string uri, HttpContent content)
        {
            object Resultado = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChaveAcesso);

                    // Json de Response //
                    var response = client.PostAsync(new Uri(uri), content).Result;


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
    }
}
