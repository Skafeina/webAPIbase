using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLayers.DML;

namespace TodoLayers.DAL
{
    internal class DaoTodoItem : AcessoDados
    {
        internal List<TodoItem> Pesquisa()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();


            DataSet ds = base.Consultar("TGS_SP_PesTodoItem", parametros);
            List<TodoItem> tdi = Converter(ds);

            return tdi;
        }
        internal long Incluir(TodoItem item)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Name", item.Name));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IsCompleted", item.IsCompleted));

            DataSet ds = base.Consultar("TGS_SP_IncTodoItem", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }
        internal void Alterar(DML.TodoItem item)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Name", item.Name));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IsCompleted", item.IsCompleted));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", item.Id));

            base.Executar("TGS_SP_AltTodoItem", parametros);
        }
        internal void Deletar(DML.TodoItem item)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", item.Id));

            base.Executar("TGS_SP_DelTodoItem", parametros);
        }
        private List<TodoItem> Converter(DataSet ds)
        {
            List<TodoItem> lista = new List<TodoItem>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    TodoItem tdi = new TodoItem();
                    tdi.Id = row.Field<int>("Id");
                    tdi.Name = row.Field<string>("Name");
                    tdi.IsCompleted = row.Field<int>("IsCompleted");
                    lista.Add(tdi);
                }
            }

            return lista;
        }
    }
}
