﻿using MyProject.DAL.Models;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace MyProject.DAL
{
    public class UserCmdSelect : SqlMysteryContext
    {
        public DataTable TabelaGenerica(String comando) {


            MySqlConnection com = new MySqlConnection("Server=localhost;Database=sql_mystery;Uid=root;Pwd=1234;");
            MySqlCommand cmd = new MySqlCommand(comando, com);
            try
            {
                com.Open();
                DataTable tab = ObterTabela(cmd.ExecuteReader());
                com.Close();
                return tab; 
            }
            catch(System.Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
      

        }

       

        public DataTable ObterTabela(DbDataReader reader)
        {
            DataTable tbEsquema = reader.GetSchemaTable();
            DataTable tbRetorno = new DataTable();

            foreach (DataRow r in tbEsquema.Rows)
            {
                if (!tbRetorno.Columns.Contains(r["ColumnName"].ToString()))
                {
                    DataColumn col = new DataColumn()
                    {
                        ColumnName = r["ColumnName"].ToString(),
                        Unique = Convert.ToBoolean(r["IsUnique"]),
                        AllowDBNull = Convert.ToBoolean(r["AllowDBNull"]),
                        ReadOnly = Convert.ToBoolean(r["IsReadOnly"])
                    };
                    tbRetorno.Columns.Add(col);
                }
            }

            while (reader.Read())
            {
                DataRow novaLinha = tbRetorno.NewRow();
                for (int i = 0; i < tbRetorno.Columns.Count; i++)
                {
                    novaLinha[i] = reader.GetValue(i);
                }
                tbRetorno.Rows.Add(novaLinha);
            }

            return tbRetorno;
        }
    }
}
