﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TodoLayers.DAL
{
    internal class AcessoDados
    {
        private string stringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["BancoDeDados"];
                if (conn != null)
                    return conn.ConnectionString;
                else
                    return string.Empty;
            }
        }

        internal void Executar(string NomeProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(stringDeConexao);
            comando.Connection = conexao;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = NomeProcedure;
            comando.CommandTimeout = 400;
            foreach (var item in parametros)
                comando.Parameters.Add(item);

            
            try
            {
                conexao.Open();
                comando.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }

        internal DataSet Consultar(string NomeProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(stringDeConexao);

            comando.Connection = conexao;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = NomeProcedure;
            comando.CommandTimeout = 400;
            foreach (var item in parametros)
                comando.Parameters.Add(item);

            SqlDataAdapter adapter = new SqlDataAdapter(comando);
            DataSet ds = new DataSet();

            try
            {
                conexao.Open();
                adapter.Fill(ds);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conexao.Close();
            }

            return ds;
        }

    }
}
