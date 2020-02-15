using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using TodoApiFW.Models;
using TodoLayers.BLL;
using TodoLayers.DML;

namespace TodoApiFW.Controllers
{
    [RoutePrefix("api/TodoItems")]
    public class TodoItemsController : ApiController
    {

        /// <summary>
        /// Método usado para consultar todos os itens do banco de ToDoItem
        /// </summary>
        /// <returns>Lista de TodoItem</returns>
        [HttpPost]
        [Route("1")]
        public IHttpActionResult PostClienteList() //HttpResponseMessage //IHttpActionResult
        {
            try
            {
                List<TodoLayers.DML.TodoItem> clientes = new BoTodoItem().Pesquisa();

                //Return result to jTable
                //return Request.CreateResponse(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Usado para Incluir uma linha de registro de ToDoItem
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns>Retorna um Json string com mensagem de sucesso ou erro</returns>
        [HttpPost]
        [Route("2")]
        public JsonResult<string> PostTodoItem([FromBody] TodoApiFW.Models.TodoItem todoItem) //Task<IActionResult>
        {
            BoTodoItem boTdi = new BoTodoItem();

            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                //Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                todoItem.Id = boTdi.Incluir(new TodoLayers.DML.TodoItem()
                {
                    Name = todoItem.Name,
                    IsCompleted = todoItem.IsComplete == true ? 1 : 0
                });
                return Json("\nTarefa: " + todoItem.Name + "\nAdicionado com sucesso!");
            }

            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);

        }

        /// <summary>
        /// Método usado para atualizar um item do banco através de Post com roteamento
        /// Ex: Url: localhost/api/TodoItems/up -> [Route("up")] e assinatura da classe -> [RoutePrefix("api/TodoItems")]
        /// </summary>
        /// <param name="titem"></param>
        /// <returns>Uma string dizendo que foi atualizada com sucesso</returns>
        [HttpPost]
        [Route("3")]
        public JsonResult<string> PostUpdateTodoItem([FromBody] Models.TodoItem titem) //Task<IActionResult>
        {
            BoTodoItem boTdi = new BoTodoItem();
            TodoLayers.DML.TodoItem tdi = new TodoLayers.DML.TodoItem();
            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                //Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                tdi.Id = titem.Id;
                tdi.Name = titem.Name;
                tdi.IsCompleted = titem.IsComplete == true ? 1 : 0;
                boTdi.Alterar(tdi);
                return Json("\nTarefa: " + titem.Name + "\nAtualizada com sucesso!");
            }

            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);

        }

        /// <summary>
        /// Método usado para deletar um item do banco através de Post com roteamento
        /// </summary>
        /// <param name="titem"></param>
        /// <returns>Uma string dizendo que foi deletado com sucesso</returns>
        [HttpPost]
        [Route("4")]
        public JsonResult<string> PostDeleteTodoItem([FromBody] Models.TodoItem titem) //Task<IActionResult> | async Task<JsonResult<string>> 
        {
            BoTodoItem boTdi = new BoTodoItem();
            TodoLayers.DML.TodoItem tdi = new TodoLayers.DML.TodoItem();
            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                //Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                tdi.Id = titem.Id;
                tdi.Name = titem.Name;
                tdi.IsCompleted = titem.IsComplete == true ? 1 : 0;
                boTdi.Deletar(tdi);
                return Json("\nTarefa: " + titem.Name + "\nDeletada com sucesso!");
            }

            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);

        }

    }

    //Usado somente se o retorno de uma requisição for HttpResponseMessage
    public class ResponseResultExtractor
    {
        public T Extract<T>(HttpResponseMessage response)
        {
            return response.Content.ReadAsAsync<T>().Result;
        }
    }

}
