using Npgsql;
using System;
using System.Windows.Forms;

namespace SISAGRO
{
    public class Conexao
    {
        //private static string strConexao = "Server=pgsql.excellentiesoftware.com.br;port=5432;User Id=excellentiesoftware;Password=exc91215709#;Database=excellentiesoftware";
        private static string strConexao = "Server=descloc.c5mjqy33ximb.us-east-1.rds.amazonaws.com;port=5432;User Id=dbmasterdescloc;Password=gWmOwnKU34YnUfVaYRaK;Database=sisagro";
        private static string strEsquema = "public.";

        private NpgsqlConnection conn;
        public Conexao()
        {
            this.conn = new NpgsqlConnection(strConexao);

        }

        public NpgsqlConnection DsCps
        {
            get { return this.conn; }
            set { this.conn = value; }
        }
        public void Conecta()
        {
            conn.Open();

            

            NpgsqlCommand cmdsql = new NpgsqlCommand("SET application_name = 'SISAGROBancoDeDados';", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();

            

        }

        public void StartTransaction()
        {
            NpgsqlCommand cmdsql = new NpgsqlCommand("START TRANSACTION", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public void Commit()
        {
            NpgsqlCommand cmdsql = new NpgsqlCommand("COMMIT", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public void RollBack()
        {
            NpgsqlCommand cmdsql = new NpgsqlCommand("rollback", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public void ExecutarSql(string cComandoSql)
        {
            using (NpgsqlCommand cmdsql = new NpgsqlCommand(cComandoSql, conn))
            {

                cmdsql.ExecuteNonQuery();

            }

        }

        public void ExecutarSql(NpgsqlCommand cmdsql)
        {

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public string CriarTemporaria(string cSql, string cCamposAdicionais = "", string cName = "")
        {
            string cNameTmp = "";
            string cSqlCreate = "";

            System.DateTime DataHoje = DateTime.Now;

            if (cName != string.Empty)
                cNameTmp = cName;
            else
                cNameTmp = "tmp" + DataHoje.ToLongTimeString().Replace(":", "") + DataHoje.Millisecond.ToString();

            cNameTmp = strEsquema + cNameTmp;
            cSqlCreate = "CREATE TEMPORARY TABLE IF NOT EXISTS  " + cNameTmp;

            if (cSql == string.Empty)
            {
                cSqlCreate += "( tmp_id serial primary key " + FuncoesGerais.IIf(cCamposAdicionais != string.Empty, "," + cCamposAdicionais, "") + ");";
            }
            else
            {
                cSqlCreate += " as " + cSql + ";";

                cSqlCreate += " ALTER TABLE " + cNameTmp + " ADD COLUMN tmp_id SERIAL PRIMARY KEY;";

                if (cCamposAdicionais != string.Empty)
                {
                    cSqlCreate += " ALTER TABLE " + cNameTmp + " ADD COLUMN " + cCamposAdicionais + ";";
                }
            }


            try
            {
                ExecutarSql(cSqlCreate);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro na criação da temporaria:" + ex.Message);

            }


            return cNameTmp;
        }
        public void desconecta()
        {
            conn.Close();
        }

    }
}
