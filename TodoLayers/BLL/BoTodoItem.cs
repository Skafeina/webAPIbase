using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLayers.BLL
{
    public class BoTodoItem
    {
        public List<DML.TodoItem> Pesquisa()
        {
            DAL.DaoTodoItem tdi = new DAL.DaoTodoItem();
            return tdi.Pesquisa();
        }
        public long Incluir(DML.TodoItem item)
        {
            DAL.DaoTodoItem tdi = new DAL.DaoTodoItem();
            return tdi.Incluir(item);
        } 
        public void Alterar(DML.TodoItem item)
        {
            DAL.DaoTodoItem tdi = new DAL.DaoTodoItem();
            tdi.Alterar(item);
        }
        public void Deletar(DML.TodoItem item)
        {
            DAL.DaoTodoItem tdi = new DAL.DaoTodoItem();
            tdi.Deletar(item);
        }
        public enum Rotas{
            Pesquisar = 1,
            Incluir = 2,
            Alterar = 3,
            Deletar = 4
        };
    }
}
