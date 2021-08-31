using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace PagOnline
{
    public class TContraints
    {

        public string cApelido { get; set; }
        public string cCampoTabela { get; set; }
        public object cTabelaVinculo { get; set; }
        public string cCampoVinculo { get; set; }
        public string cRegra { get; set; }
    }
    public class TIndices
    {
        public string cApelido { get; set; }
        public string cCampos { get; set; }
        public bool bUnique { get; set; }

    }
    public class TDbFields
    {

        public int nLen { get; set; }
        public int nDec { get; set; }
        public string cName { get; set; }
        public string cType { get; set; }
        public string cComment { get; set; }
        public string cDefValue { get; set; }

        public bool lNotNull { get; set; }


    }



    public class CheckStruct
    {

        Conexao ds = new Conexao();
        NpgsqlCommand comm = new NpgsqlCommand();
        NpgsqlDataReader dr;


        public Label cLabelTela;
        public ProgressBar prgdlg;

        List<string> aSql = new List<string>();
        List<string> aLegenda = new List<string>();
        List<string> aView = new List<string>();
        List<string> aFuncao = new List<string>();
        List<string> aTrigger = new List<string>();

        private static List<TDbFields> alistacampo = new List<TDbFields>();
        private static List<TIndices> alistaindice = new List<TIndices>();
        private static List<TContraints> alistacontraint = new List<TContraints>();

        public string cTableName = "";
        public string cComentario = "";
        public string cNamePrimary = "";

        public string cSchema = "public";


        public string cSqlViewApagar = "";
        public string cSqlTriggerApagar = "";

        public void open()
        {
            ds.Conecta();
            comm.Connection = ds.DsCps;

        }

        public void AddView(string cSql)
        {
            aView.Add(cSql);
        }

        public void AddFunction(string cSql)
        {
            aFuncao.Add(cSql);
        }

        public void AddTrigger(string cSql)
        {
            aTrigger.Add(cSql);
        }


        public void addcontraint(string apelido, string campotabela, object tabelavinculo, string campovinculo = "", string regra = "")
        {
            var uField = new TContraints();

            uField.cApelido = apelido;
            uField.cCampoTabela = campotabela;
            uField.cTabelaVinculo = tabelavinculo;
            uField.cCampoVinculo = campovinculo;
            uField.cRegra = regra;

            alistacontraint.Add(uField);

        }

        public void addindice(string apelido, string campos, Boolean lunique = false)
        {
            var uField = new TIndices();
            uField.cApelido = apelido;
            uField.cCampos = campos;
            uField.bUnique = lunique;
            alistaindice.Add(uField);
        }

        public void AddCampo(string Name, string type, int Len, int Dec, string Comment, bool NotNull, string DefValue)
        {
            var uField = new TDbFields();

            uField.cName = Name;
            uField.cType = type;
            uField.nLen = Len;
            uField.nDec = Dec;
            uField.cComment = Comment;
            uField.lNotNull = NotNull;

            uField.cDefValue = DefValue;
            alistacampo.Add(uField);
        }

        public void Run()
        {
            int count = 0;
            cLabelTela.Text = "Analisando arquivo " + cComentario;
            cLabelTela.Update();
            comm = new NpgsqlCommand("SELECT count(1) as qtde from pg_tables where schemaname = '"+ cSchema.ToLower().Trim() + "' and tablename ='" + cTableName + "'", ds.DsCps);

            dr = comm.ExecuteReader();
            while (dr.Read())
            {
                count = Convert.ToInt32(dr["qtde"].ToString());
            }
            dr.Close();
            //comm.Dispose();

            if (count == 0)
            {
                CriarArquivo();
            }
            else
            {
                AlterarArquivo();
            }

            AnalisarIndices();
            AnalisarContraint();

            cTableName = "";
            cComentario = "";

            limpacampo();
        }

        private void AnalisarContraint()
        {
            string csql = "";
            string cnomeindice = "";
            string cforekey = "";
            int count = 0;
            int ntotal = 0;
            string cOpcao = "";

            foreach (var element in alistacontraint)
            {
                cnomeindice = cTableName.ToLower().Trim() + "_" + element.cApelido.ToLower().Trim();

                if (element.cTabelaVinculo is string)
                {

                    csql = @"SELECT count(1) as qtde 
                     FROM information_schema.table_constraints AS tc JOIN information_schema.key_column_usage AS kcu 
                     ON tc.constraint_name = kcu.constraint_name    JOIN information_schema.constraint_column_usage AS ccu 
                     ON ccu.constraint_name = tc.constraint_name
                     WHERE constraint_type = 'FOREIGN KEY' AND tc.table_schema = '"+ cSchema.ToLower().Trim() + "' and  tc.table_name='" + cTableName.ToLower().Trim() + @"' and tc.constraint_name = '" + cnomeindice + "'";

                    NpgsqlDataReader rd;
                    NpgsqlCommand cmdsql = new NpgsqlCommand(csql, ds.DsCps);

                    rd = cmdsql.ExecuteReader();
                    while (rd.Read())
                    {
                        count = rd.GetInt32(0);
                    }

                    if (count == 0)
                    {
                        cforekey = " FOREIGN KEY (" + element.cCampoTabela.ToLower().Trim() + ") references " + element.cTabelaVinculo.ToString().ToLower().Trim() +
                            " (" + element.cCampoVinculo.ToLower().Trim() + ") " + element.cRegra.ToLower().Trim();

                        aSql.Add("ALTER TABLE "+ cSchema.ToLower().Trim() + "." + cTableName.ToLower().Trim() + " ADD CONSTRAINT " + cnomeindice + " " + cforekey);
                    }
                    rd.Close();
                    cmdsql.Dispose();
                }
                else if (element.cTabelaVinculo is Array)
                {

                    csql = "SELECT count(1) as qtde " +
                        "FROM  information_schema.table_constraints AS tc JOIN information_schema.check_constraints AS kcu ON tc.constraint_name = kcu.constraint_name " +
                        " JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name  WHERE  tc.table_schema = '" + cSchema.ToLower().Trim() + "' and tc.table_name='" + cTableName.ToLower().Trim() + "' " +
                        " and tc.constraint_name = '" + cnomeindice + "'";

                    NpgsqlDataReader rd;
                    NpgsqlCommand cmdsql = new NpgsqlCommand(csql, ds.DsCps);

                    rd = cmdsql.ExecuteReader();
                    while (rd.Read())
                    {
                        count = rd.GetInt32(0);
                    }

                    if (count != 0)
                    {
                        aSql.Add("alter table " + cSchema.ToLower().Trim() +"."+cTableName.ToLower().Trim() + " drop constraint " + cnomeindice);
                    }
                    cforekey = "check (";

                    // Pegando um objeto e convertendo para array

                    var ateste = ((IEnumerable)element.cTabelaVinculo).Cast<object>()
                                   .Select(x => x == null ? x : x.ToString())
                                   .ToArray();

                    ntotal = ateste.Count() - 1;

                    for (int npos = 0; npos <= ntotal; npos++)
                    {
                        cOpcao = ateste[npos].ToString();

                        cforekey += element.cCampoTabela.ToLower().Trim() + "::" + IIf(cOpcao is string, "text", "integer") + " = " + IIf(cOpcao is string, "'" + cOpcao + "'", cOpcao.ToString()) + "::" + IIf(cOpcao is string, "text", "integer");
                        if (npos != ntotal)
                            cforekey += " or ";

                    }

                    cforekey += ")";
                    cforekey = "ALTER TABLE " + cSchema.ToLower().Trim() +"."+ cTableName.ToLower().Trim() + " ADD CONSTRAINT " + cnomeindice + " " + cforekey;

                    aSql.Add(cforekey);

                    rd.Close();
                    cmdsql.Dispose();
                }


            }
        }

        private void AnalisarIndices()
        {
            string csql = "";
            string cnomeindice = "";
            int count = 0;

            foreach (var element in alistaindice)
            {

                cnomeindice = cTableName.ToLower().Trim() + "_" + element.cApelido.ToLower().Trim();

                csql = @"SELECT count(1) as qtde
                        FROM
                            pg_indexes
                        WHERE
                            schemaname = '"+ cSchema.ToLower().Trim() + @"'
                            and Tablename = '" + cTableName + "' and indexname = '" + cnomeindice + "'";

                /*csql = "SELECT count(1) as qtde " +
                " FROM pg_class t,    pg_class i,    pg_index ix,   pg_attribute a" +
                " WHERE t.oid = ix.indrelid    and i.oid = ix.indexrelid    and a.attrelid = t.oid    and a.attnum = ANY(ix.indkey) " +
                " and t.relkind = 'r' and t.relname like '" + cTableName.ToLower().Trim() + "%' and I.relname = '" +
                cnomeindice + "' group by t.relname, I.relname " +
                " ORDER BY t.relname,    I.relname";
                */
                NpgsqlDataReader rd;
                NpgsqlCommand cmdsql = new NpgsqlCommand(csql, ds.DsCps);
                //Clipboard.SetText(csql);

                rd = cmdsql.ExecuteReader();
                while (rd.Read())
                {
                    count = rd.GetInt32(0);
                }

                if (count == 0)
                {
                    aSql.Add("create  " + IIf(element.bUnique, " unique ", " ") + " index " + cnomeindice + " on " + cSchema.ToLower()+"."+cTableName.ToLower().Trim() + " (" + element.cCampos + ")");

                }
                rd.Close();
                cmdsql.Dispose();
            }
        }

        object IIf(bool expression, object truePart, object falsePart)
        { return expression ? truePart : falsePart; }

        private void AlterarArquivo()
        {
            string cNomeCampo = "";
            string ctipodb = "";
            string csqlalteracao = "";
            bool balterar = false;

            int nTamanho = 0;
            int nDecimal = 0;
            bool btemcampo = false;
            string cSql = "";


            foreach (var element in alistacampo)
            {
                btemcampo = false;
                nTamanho = 0;
                nDecimal = 0;

                //if (element.cName.ToLower() == "tmc_id")
                //  MessageBox.Show("öi");

                if (!element.cType.ToLower().Equals("serial")) // && !element.cType.ToLower().Trim().Equals("integer"))
                {
                    try
                    {
                        cSql = "SELECT column_name::char(40) as coluna ,data_type as Tipo,coalesce(character_maximum_length,0) as Tamanho,coalesce(numeric_precision,0) as TamNumerico,coalesce(numeric_scale,0) TamDecimal FROM information_schema.columns WHERE lower(table_schema) = '"+cSchema.ToLower()+"' and lower(table_name) = '" + cTableName.ToLower() + "' and lower(column_name) = '" + element.cName.ToLower() + "'";

                        NpgsqlDataReader rd;
                        NpgsqlCommand cmdsql = new NpgsqlCommand(cSql, ds.DsCps);

                        rd = cmdsql.ExecuteReader();
                        while (rd.Read())
                        {
                            cNomeCampo = rd["coluna"].ToString().ToLower().Trim();
                            ctipodb = rd["Tipo"].ToString().ToLower().Trim();

                            if (rd.GetInt32(2) > 0)
                                nTamanho = rd.GetInt32(2);
                            else
                                nTamanho = rd.GetInt32(3);
                            nDecimal = rd.GetInt32(4);
                        }


                        cmdsql.Dispose();
                        rd.Close();
                    }

                    catch (NpgsqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    if (element.cName.ToString().ToLower().Equals(cNomeCampo))
                    {

                        btemcampo = true;
                        csqlalteracao = "alter table " + cSchema+"."+cTableName + " alter column " + element.cName + " type ";

                    }
                    else
                    {
                        ctipodb = element.cType;
                        csqlalteracao = "alter table " + cSchema + "." + cTableName + " add column " + element.cName + " ";
                    }

                    if (btemcampo)
                    {
                        if (element.cType.ToString() == "integer")
                        {
                            csqlalteracao = "";
                        }

                        if (nTamanho == element.nLen && nDecimal == element.nDec)
                        {
                            csqlalteracao = "";
                        }

                    }

                    if (!csqlalteracao.Equals(""))
                    {

                        csqlalteracao += ctipodb;
                        if (element.nLen > 0)
                        {
                            csqlalteracao += "(" + element.nLen.ToString();

                            if (!element.nDec.Equals(0))
                                csqlalteracao += "," + element.nDec.ToString() + ") ";
                            else
                                csqlalteracao += ") ";

                        }

                        if (!element.cDefValue.Equals(""))
                        {
                            if (btemcampo)
                                csqlalteracao += " ; alter table " + cSchema + "." + cTableName + " alter column " + element.cName + " set default " + element.cDefValue + ";";
                            else
                                csqlalteracao += " default " + element.cDefValue+" ; ";
                            //FuncoesGerais.Mensagem(csqlalteracao);
                        }



                        aSql.Add(csqlalteracao);
                        csqlalteracao = "";
                        if (!btemcampo)
                        {
                            if (!element.cComment.Equals(""))
                            {

                                aLegenda.Add("COMMENT ON COLUMN " + cSchema + "." + cTableName + '.' + element.cName + " is '" + element.cComment.ToString() + "'");
                            }
                        }
                    }
                }
                else if (element.cType.ToLower() == "serial")
                    cNamePrimary = element.cName.ToString();
            }


        }

        private void CriarArquivo()
        {
            string cSql = "";
            aLegenda.Clear();
            cSql = "create table " + cSchema + "." + cTableName + " (";

            foreach (var element in alistacampo)
            {
                cSql += element.cName + " " + element.cType;
                if (element.cType == "serial")
                    cNamePrimary = element.cName;
                if (element.nLen > 0)
                {
                    cSql += "(" + element.nLen.ToString();

                    if (!element.nDec.Equals(0))
                        cSql += "," + element.nDec.ToString() + ") ";
                    else
                        cSql += ") ";

                }

                if (!element.cDefValue.Equals(""))
                {
                    cSql += " default " + element.cDefValue;
                }
                cSql += ",";
                if (!element.cComment.Equals(""))
                    aLegenda.Add("COMMENT ON COLUMN " + cSchema+"."+cTableName + '.' + element.cName + " is '" + element.cComment.ToString() + "'");

            }
            if (!cNamePrimary.Equals(""))
                cSql += " CONSTRAINT Pry_" + cTableName + " PRIMARY KEY (" + cNamePrimary + "),";

            cSql = cSql.Substring(0, cSql.Length - 1) + ");";

            // MessageBox.Show(cSql);
            aSql.Add(cSql);
            cNamePrimary = "";


            if (!cComentario.Equals(" "))
                aSql.Add("COMMENT ON TABLE " + cSchema + "." + cTableName + " is '" + cComentario + "'");
            if (aLegenda.Count > 0)
            {
                foreach (var element in aLegenda)
                    aSql.Add(element.ToString());

            }
        }


        private void limpacampo()
        {
            alistacampo.Clear();
            alistaindice.Clear();
            alistacontraint.Clear();

        }

        public void HabilitarExtension(string cName)
        {
            DataSet dsExtension = new DataSet();

            dsExtension = FuncoesGerais.oxcQuery("SELECT count(1) as qtde FROM pg_extension where extname = " + FuncoesGerais.QuoteStr(cName.ToLower()), ds);

            if (dsExtension.Tables[0].Rows[0]["qtde"].ToString() == "0")
            {
                using (NpgsqlCommand cmdsql = new NpgsqlCommand("create extension " + cName.ToLower(), ds.DsCps))
                {
                    cmdsql.ExecuteNonQuery();
                }

            }

            dsExtension.Dispose();
        }

        public void executa()
        {
            int count = 0;

            DataSet dsExtension = new DataSet();

            NpgsqlCommand cmdsql = new NpgsqlCommand("", ds.DsCps);

            prgdlg.Visible = true;
            prgdlg.Value = 0;
            prgdlg.Maximum = aSql.Count() + aLegenda.Count() + aView.Count() + aFuncao.Count() + aTrigger.Count();



            cmdsql.CommandText = cSqlViewApagar.ToString();
            cmdsql.ExecuteNonQuery();

            cmdsql.CommandText = cSqlTriggerApagar.ToString();
            cmdsql.ExecuteNonQuery();

            cLabelTela.Text = "executando as modificações no banco de dados...";
            cLabelTela.Update();
            foreach (var element in aSql)
            {
                count++;
                prgdlg.Value = count;

                prgdlg.Update();

                cmdsql.CommandText = element.ToString();
                cmdsql.ExecuteNonQuery();



            }
            cLabelTela.Text = "executando os comentários no banco de dados...";

            foreach (var element in aLegenda)
            {
                count++;
                prgdlg.Value = count;
                prgdlg.Update();
                cmdsql.CommandText = element.ToString();
                cmdsql.ExecuteNonQuery();



            }

            cLabelTela.Text = "atualizando as views no banco de dados...";

            foreach (var element in aView)
            {
                count++;
                prgdlg.Value = count;
                prgdlg.Update();
                cmdsql.CommandText = element.ToString();
                cmdsql.ExecuteNonQuery();



            }

            cLabelTela.Text = "atualizando as funções no banco de dados...";

            foreach (var element in aFuncao)
            {
                count++;
                prgdlg.Value = count;
                prgdlg.Update();
                cmdsql.CommandText = element.ToString();
                cmdsql.ExecuteNonQuery();



            }

            cLabelTela.Text = "atualizando as triggers no banco de dados...";

            foreach (var element in aTrigger)
            {
                count++;
                prgdlg.Value = count;
                prgdlg.Update();
                cmdsql.CommandText = element.ToString();
                cmdsql.ExecuteNonQuery();



            }

            cmdsql.Dispose();



        }
    }

}
