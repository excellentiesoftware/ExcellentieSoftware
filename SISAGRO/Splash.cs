using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace SISAGRO
{
    public partial class SplashForm : Form
    {
        private bool lIniciarAtualizacao = false;
        public SplashForm()
        {
            InitializeComponent();
        }

        private void EstruturaCadastro(CheckStruct oCheck)
        {


            oCheck.cTableName = "cad_grupo_propriedades";
            oCheck.cComentario = "Grupo dos Espações da Propriedas";
            oCheck.cSchema = "public";

            oCheck.AddCampo("grppro_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("grppro_nome", "varchar", 100, 0, "Razao Social", false, "");
            oCheck.AddCampo("grppro_ativo", "boolean", 0, 0, "Ativo", false, "true");
            
            oCheck.addindice("ind_grppro_01", "upper(grppro_nome)", true);

            oCheck.Run();



            oCheck.cTableName = "cad_usuario";
            oCheck.cComentario = "Cadastro de Usuario do Grupo Propriedades";
            oCheck.cSchema = "public";

            oCheck.AddCampo("usu_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("grppro_id", "integer", 0, 0, "Id Grupo Propriedade", false, "");
            oCheck.AddCampo("grpace_id", "integer", 0, 0, "Id Grupo Acesso", false, "");
            oCheck.AddCampo("usu_nome", "varchar", 100, 0, "Nome do Usuario", false, "");
            oCheck.AddCampo("usu_login", "varchar", 100, 0, "Login do Usuario", false, "");
            oCheck.AddCampo("usu_email", "varchar", 100, 0, "Email", false, "");
            oCheck.AddCampo("usu_senha", "varchar", 100, 0, "Senha", false, "");
            oCheck.AddCampo("usu_ativo", "boolean", 0, 0, "Ativo:", false, "true");
            oCheck.AddCampo("usu_foto", "bytea", 0, 0, "Foto", false, "");
            oCheck.AddCampo("usu_celular", "varchar", 30, 0, "celular", false, "");
            oCheck.AddCampo("usu_telefone_fixo", "varchar", 30, 0, "Telefone", false, "");
            oCheck.AddCampo("usu_numero_cpf", "varchar", 18, 0, "Cpf", false, "");
            oCheck.AddCampo("usu_tipo_genero", "char", 1, 0, "Sexo", false, "");

            oCheck.addindice("ind_usu_01", "grppro_id,upper(usu_login)", true);
            oCheck.addindice("ind_usu_02", "grppro_id");

            oCheck.Run();


            oCheck.cTableName = "cad_cidade";
            oCheck.cComentario = "Cadastro das Cidades";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cid_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cid_nome", "varchar", 100, 0, "Nome da Cidade", false, "");
            oCheck.AddCampo("cid_uf", "char", 2, 0, "Sigla do Estado", false, "");
            oCheck.AddCampo("cid_pais", "varchar", 5, 0, "Sigla do Pais", false, "");
            oCheck.AddCampo("cid_coordenada", "point", 0, 0, "Coordenada geolocalizacao", false, "");
            
            oCheck.addindice("ind_cid_01", "upper(cid_nome),upper(cid_uf)", true);
            
            oCheck.Run();


            /*oCheck.cTableName = "cad_propriedade";
            oCheck.cComentario = "Cadastr dos Clientes";

            oCheck.AddCampo("cad_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cad_razao_social", "varchar", 100, 0, "Razao Social", false, "");
            oCheck.AddCampo("cad_nome_fantasia", "varchar", 100, 0, "Nome Fantasia", false, "");
            oCheck.AddCampo("cad_cnpj_cpf", "varchar", 18, 0, "CNPJ/CPF", false, "");
            oCheck.AddCampo("cad_email", "varchar", 100, 0, "Email", false, "");
            oCheck.AddCampo("cad_telefone", "varchar", 30, 0, "Telefone", false, "");
            oCheck.AddCampo("cad_ativo", "boolean", 0, 0, "Ativo", false, "true");

            oCheck.AddCampo("lgr_nome", "varchar", 100, 0, "Rua do Logradouro", false, "");
            oCheck.AddCampo("end_numero", "varchar", 10, 0, "Numero", false, "");
            oCheck.AddCampo("end_complemento", "varchar", 30, 0, "Complemento", false, "");
            oCheck.AddCampo("cid_nome", "varchar", 40, 0, "Cidade", false, "");
            oCheck.AddCampo("bai_nome", "varchar", 40, 0, "Bairro", false, "");
            oCheck.AddCampo("uf_sigla", "varchar", 2, 0, "UF", false, "");


            //oCheck.AddCampo("log_registro", "text", 0, 0, "Log Registro", false, "");

            oCheck.addindice("ind_cad_01", "upper(cad_cnpj_cpf)", true);

            oCheck.Run();

            */


        }


        private void VerificarBancoDados()
        {


            CheckStruct oCheck = new CheckStruct();

            progressBarControl1.Visible = true;

            oCheck.cLabelTela = lblAtualizacao;
            oCheck.prgdlg = progressBarControl1;
            oCheck.open();


            lblAtualizacao.Text = "Verificando extensões do banco de dados..";
            lblAtualizacao.Update();

            //oCheck.HabilitarExtension("unaccent");
            //oCheck.HabilitarExtension("pg_trgm");
            //oCheck.HabilitarExtension("tablefunc");
            //oCheck.HabilitarExtension("postgis");
            //oCheck.HabilitarExtension("plpgsql");


            lblAtualizacao.Text = "Apagando as views..";
            lblAtualizacao.Update();

            oCheck.cSqlTriggerApagar = @"
                 DROP TRIGGER IF EXISTS  log_cad_centro_custo ON cad_centro_custo cascade;
    		";

            // Apagar as view antes da criação e atualização dos dados

            ApagarView(oCheck);
            lblAtualizacao.Text = "Verificando a estrutura das tabelas...";
            lblAtualizacao.Update();

            EstruturaCadastro(oCheck);



            AtualizaFuncoes(oCheck);
            AtualizaTrigger(oCheck);

            lblAtualizacao.Text = "Executando todas as modificações estruturais...";
            lblAtualizacao.Update();

            oCheck.executa();


        }
        private void AtualizaTrigger(CheckStruct oCheck)
        {

            string cSql = "";

            /*cSql = @"CREATE TRIGGER log_cad_organiacao
                     AFTER INSERT OR UPDATE  OR DELETE ON cad_organizacao
                     FOR EACH ROW
                     EXECUTE PROCEDURE sis_log_tabelas();";

            oCheck.AddTrigger(cSql);
            */

        }


        private void AtualizaFuncoes(CheckStruct oCheck)
        {

            string cSql = "";

            cSql = @"CREATE OR REPLACE FUNCTION iif_integer(BOOLEAN, integer, Integer) RETURNS integer AS
                      $body$
                      DECLARE
                         rtVal integer;
                      BEGIN
                         rtVal := (SELECT CASE $1 WHEN TRUE THEN $2 ELSE $3 END);
                         RETURN rtVal;
                      END;
                      $body$

                      LANGUAGE 'plpgsql' STABLE CALLED ON NULL INPUT SECURITY INVOKER;";

            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION iif_boolean(BOOLEAN, boolean, boolean) RETURNS boolean AS
                      $body$
                      DECLARE
                         rtVal boolean;
                      BEGIN
                         rtVal := (SELECT CASE $1 WHEN TRUE THEN $2 ELSE $3 END);
                         RETURN rtVal;
                      END;
                      $body$

                      LANGUAGE 'plpgsql' STABLE CALLED ON NULL INPUT SECURITY INVOKER;";

            oCheck.AddFunction(cSql);

            cSql = @" CREATE OR REPLACE FUNCTION iif_numerico(BOOLEAN, double PRECISION, double PRECISION) RETURNS double PRECISION AS
                      $body$
                      DECLARE
                         rtVal double PRECISION;
                      BEGIN
                         rtVal := (SELECT CASE $1 WHEN TRUE THEN $2 ELSE $3 END);
                         RETURN rtVal;
                      END;
                      $body$

                      LANGUAGE 'plpgsql' STABLE CALLED ON NULL INPUT SECURITY INVOKER;";
            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION iif_date(BOOLEAN, date, Date) RETURNS Date AS
                      $body$
                      DECLARE
                         rtVal Date;
                      BEGIN
                         rtVal := (SELECT CASE $1 WHEN TRUE THEN $2 ELSE $3 END);
                         RETURN rtVal;
                      END;
                      $body$

                      LANGUAGE 'plpgsql' STABLE CALLED ON NULL INPUT SECURITY INVOKER;";

            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION iif_string(BOOLEAN, text, text) RETURNS text AS
                      $body$
                      DECLARE
                         rtVal text;
                      BEGIN
                         rtVal := (SELECT CASE $1 WHEN TRUE THEN $2 ELSE $3 END);
                         RETURN rtVal;
                      END;
                      $body$

                      LANGUAGE 'plpgsql' STABLE CALLED ON NULL INPUT SECURITY INVOKER;";
            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION exc_remove_tracos (p_texto text)
                        RETURNS text AS
                         $$
                         DECLARE
                           stextonovo TEXT;
                         BEGIN

                         stextonovo := replace(p_texto, '-','') ;
                         stextonovo := replace(stextonovo, '.','') ;
                         stextonovo := replace(stextonovo, '/','') ;
                         stextonovo := replace(stextonovo, '\','') ;
                         stextonovo := replace(stextonovo, ')','') ;
                         stextonovo := replace(stextonovo, '(','') ;
                         stextonovo := replace(stextonovo, ' ','') ;
                         stextonovo := replace(stextonovo, '_','') ;
                        RETURN stextonovo;
                         END;

                      $$
                      LANGUAGE 'plpgsql' STABLE CALLED ON NULL INPUT SECURITY INVOKER;";

            oCheck.AddFunction(cSql);


            cSql = @"CREATE OR REPLACE FUNCTION exc_remove_acento (p_texto text)
                        RETURNS text AS
                       $BODY$
                       Select translate($1,
                       'áàâãäåaaaÁÂÃÄÅAAAÀéèêëeeeeeEEEÉEEÈìíîïìiiiÌÍÎÏÌIIIóôõöoooòÒÓÔÕÖOOOùúûüuuuuÙÚÛÜUUUUçÇñÑýÝ',
                       'aaaaaaaaaAAAAAAAAAeeeeeeeeeEEEEEEEiiiiiiiiIIIIIIIIooooooooOOOOOOOOuuuuuuuuUUUUUUUUcCnNyY'
                        );
                       $BODY$
                       LANGUAGE sql VOLATILE
                       COST 100;";
            oCheck.AddFunction(cSql);

            cSql = @" CREATE OR REPLACE FUNCTION exc_locate (p_texto text)
                        RETURNS text AS
                       $BODY$
                       Select cast($1 as text) ;
                       --Select unaccent(cast($1 as text)) ;

                       $BODY$
                       LANGUAGE sql VOLATILE
                       COST 100;";
            oCheck.AddFunction(cSql);


            cSql = @"CREATE OR REPLACE FUNCTION exc_locate (p_texto numeric)
                        RETURNS text AS
                       $BODY$
                       Select cast($1 as text) ;
                        --Select unaccent(cast($1 as text)) ;

                       $BODY$
                       LANGUAGE sql VOLATILE
                       COST 100;";
            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION exc_locate (p_texto smallint)
                        RETURNS text AS
                       $BODY$
                       Select cast($1 as text) ;
                       --Select unaccent(cast($1 as text)) ;

                       $BODY$
                       LANGUAGE sql VOLATILE
                       COST 100;";

            oCheck.AddFunction(cSql);


            cSql = @"CREATE OR REPLACE FUNCTION exc_locate (p_texto text)
                        RETURNS text AS
                       $BODY$
                       Select cast($1 as text) ;
                       --Select unaccent(cast($1 as text)) ;

                       $BODY$
                       LANGUAGE sql VOLATILE
                       COST 100;";
            oCheck.AddFunction(cSql);





            cSql = @"CREATE OR REPLACE FUNCTION year (p_data date)
                      RETURNS double precision AS
                     $BODY$
                     Select extract(Year from p_data) ;
                     $BODY$
                     LANGUAGE sql VOLATILE
                     COST 100;";

            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION month (p_data date)
                      RETURNS double precision AS
                     $BODY$
                     Select extract(Month from p_data) ;
                     $BODY$
                     LANGUAGE sql VOLATILE
                     COST 100;";

            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION day (p_data date)
                    RETURNS double precision AS
                   $BODY$
                   Select extract(day from p_data) ;
                   $BODY$
                   LANGUAGE sql VOLATILE
                   COST 100;";

            oCheck.AddFunction(cSql);


            cSql = @"CREATE OR REPLACE FUNCTION exc_ultimo_dia(Mes INTEGER, Ano INTEGER)
                      RETURNS date AS $$
                      DECLARE
                        UltimoDiaMes date;
                      BEGIN
                         If Mes <> 12 then
                            Mes := Mes + 1 ;
                         else
                            Mes := 0 ;
                            Ano := Ano + 1 ;
                         End if ;
                        SELECT  ((Ano||'/'||(Mes + 1)||'/01'):: DATE - 1)
                        INTO UltimoDiaMes;

                        RETURN UltimoDiaMes;
                      END; $$
                      LANGUAGE 'plpgsql';";

            oCheck.AddFunction(cSql);


            cSql = @"CREATE OR REPLACE FUNCTION exc_proximodiautil(dData date)
                      RETURNS Date AS
                      $BODY$
                      BEGIN
                         IF date_part('dow', dData) = 0 Then
                            dData := dData + 1 ;
                         Else
                            IF date_part('dow', dData) = 6 Then
                               dData := dData + 2 ;
                            End if ;
                         End if ;
                      RETURN dData;
                      END;
                      $BODY$
                      LANGUAGE plpgsql STABLE";

            oCheck.AddFunction(cSql);


            cSql = @"CREATE OR REPLACE FUNCTION exc_vencimentodiautil(pData date, pDias integer)
                     RETURNS date AS
                  $BODY$
                  declare
                    Dia integer;
                    SomaDias integer;
                    NovaData date;
                  Begin
                    SomaDias = 0;
                    NovaData = pData;
                    while (SomaDias < pDias) loop
                      NovaData = NovaData + 1;
                      Dia = extract(dow from NovaData);
                      while (Dia = 0 or Dia = 6) loop
                        NovaData = NovaData + 1;
                        Dia = extract(dow from NovaData);
                      end loop;
                      SomaDias = SomaDias + 1;
                    end loop;
                    return NovaData;
                  end;
                  $BODY$
                    LANGUAGE plpgsql VOLATILE
                    COST 100;";

            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION quotestr(Argumento text)
                  RETURNS text AS
                  $BODY$
                  DECLARE
                  BEGIN

                     RETURN ''''||Argumento||'''' ;

                  END;
                  $BODY$
                  LANGUAGE plpgsql VOLATILE ;";
            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION exc_retornadiavencimento(Mes text,Dia_Tipo Integer,Dia_Numero Integer,Dia_data date)
                      RETURNS Date AS
                      $BODY$
                      DECLARE
                         dDataNova Date ;
                      BEGIN


                         IF dia_tipo = 1 Then -- Fixo
                            Begin
                               dDataNova := cast(substring(Mes,4,7)||'-'||substring(Mes,1,2)||'-'||dia_numero::text as date) ;

                            EXCEPTION when datetime_field_overflow Then

                               dDataNova := cps_ultimo_dia(substring(mes,1,2)::integer,substring(mes,4,7)::integer) ;

                            End ;
                         Elsif dia_Tipo = 2 Then -- dia corrido
                            dDataNova := cps_proximodiautil(cast(substring(Mes,4,7)||'-'||substring(Mes,1,2)||'-'||dia_numero::text as date)) ;
                         ElsIf dia_tipo = 3 Then -- numero de dias uteis no mes
                            dDataNova := cps_vencimentodiautil(cast(substring(Mes,4,7)||'-'||substring(Mes,1,2)||'-01' as date),dia_numero) ;
                         ElsIF dia_tipo = 4 Then -- data fixa
                            dDataNova := dia_data ;

                         End if ;

                         if ddataNova < Now()::date then
                            dDataNova := cps_proximodiautil(Now()::date) ;
                         End if ;

                      RETURN dDataNova;
                      END;
                      $BODY$
                      LANGUAGE plpgsql STABLE";
            oCheck.AddFunction(cSql);

            cSql = @"CREATE OR REPLACE FUNCTION sis_log_tabelas()
                RETURNS trigger
                LANGUAGE 'plpgsql'
                COST 100
                VOLATILE NOT LEAKPROOF
                AS $BODY$
                DECLARE
	                TextoLog text = '' ;
	                Antes varchar array;
	                Depois varchar array;
	                nomecampo varchar;
	                nomescampos varchar array;
	                nomescomentario varchar array ;
	                cUsuario text ='';
	                i integer;
	                texto text;		
	                cChave text = '' ;
	                cIdCampo text = '' ;
	                cSql text = '';
	                nIdRegistro text ;
	                nIdConexao integer ;
	                nColIni integer = 0 ;
	                nIdUsuario integer = 0 ;
                BEGIN
	
	                TextoLog := '' ;
	
	                select pg_backend_pid() into nIdConexao ;
	
	                select application_name into cUsuario from pg_Stat_activity where pid = nIdConexao;
	
	                select POSITION('IdUser:' in cUsuario) into nColIni ;
	
	                if nColIni > 0 then
		                nIdUsuario := substring(cUsuario,nColIni+7)::integer ;
		                cUsuario := substring(cUsuario,0,nColIni) ;
	                end if ;
	
	                IF TG_OP = 'UPDATE' THEN
	                  texto = cast(OLD as text);
	                  texto = substr(texto,2 ,length(texto)-2);
	                  Antes = string_to_array(texto,',');

	                  texto = cast(NEW as text);
	                  texto = substr(texto,2 ,length(texto)-2);
	                  Depois = string_to_array(texto,',');

	                  nomescampos = ARRAY(select attname FROM pg_attribute WHERE attrelid = TG_RELID AND attstattarget = -1 order by attnum);
	                  for i in 1..array_length(nomescampos, 1)
	                  LOOP
	  		                if i = 1 then
				                nIdRegistro := Depois[i] ;
				                cIdCampo := nomescampos[i] ;
			                end if ;
	  	
		                  if lower(nomescampos[i]) not in('log_registro') then
			                  if Antes[i] <> Depois[i] THEN
				                  textolog = textolog||nomescampos[i]||': '||Antes[i]||' >> '||Depois[i]||E'\r\n';
			                  end if;
		                  End if;
	                  END LOOP;   
	                  if textolog <> '' then
                        cSql := 'UPDATE '||TG_TABLE_NAME||' SET log_registro = concat( '||Quotestr(now()::timestamp(0)::text||' '||cUsuario||'-'||textolog)||E'\r\n,'||Quotestr(coalesce(OLD.log_registro,''))||') where '||cIdCampo||'=<codigo>';
	                  end if ; 
	                ELSEIF TG_OP = 'INSERT' THEN
		                texto = cast(NEW as text);
	                    texto = substr(texto,2 ,length(texto)-2);
	                    Depois = string_to_array(texto,',');
	                    nomescampos = ARRAY(select attname FROM pg_attribute WHERE attrelid = TG_RELID AND attstattarget = -1 order by attnum);
	                  textolog := '' ;
	                  for i in 1..array_length(nomescampos, 1)
	                  LOOP
	  		                if i = 1 then
				                nIdRegistro := Depois[i] ;
				                cIdCampo := nomescampos[i] ;
			                end if ;
		                    textolog = textolog||nomescampos[i]||':'||Depois[i]||E'\r\n';
			                if i >= 2 then
				                exit ;
			                end if ;
	                  END LOOP;   
	  
		                cSql := 'UPDATE '||TG_TABLE_NAME||' SET log_registro = '||Quotestr(now()::timestamp(0)::text||' '||cUsuario||'-Criação do Registro.'||E'\n\n'||textolog)||' where '||cIdCampo||'= <codigo>';
	                ELSEIF TG_OP = 'DELETE' THEN		
		                texto = cast(OLD as text);
	                    texto = substr(texto,2 ,length(texto)-2);
	                    Antes = string_to_array(texto,',');
		                nIdRegistro := Antes[1];
		                cSql := 'insert into sis_arquivo_morto(usu_id,mor_data_exclusao,mor_tabela,mor_script_criacao,mor_id_registro) values('||nIdUsuario::text||',now()::timestamp(0),'||quotestr(TG_TABLE_NAME)||','||quotestr(OLD.log_registro)||',<codigo>)';
	                END if ;
	                if cSql <> '' then
		                cSql := REPLACE(cSql, '<codigo>', nIdRegistro) ;
	  	                EXECUTE cSql ;
	                end if ;
	

                RETURN NULL;
                END;
                $BODY$;";

            oCheck.AddFunction(cSql);


            

        }


        private void ApagarView(CheckStruct oCheck)
        {
            string cSqlDrop = "";

            string cSqlCreate = "";

            cSqlDrop = @"
                        DROP VIEW IF EXISTS vw_dados_usuario cascade ;
            ";


            oCheck.cSqlViewApagar = cSqlDrop;


            // Inclusao das Views

            /*            cSqlCreate = @"

                           CREATE OR REPLACE VIEW vw_dados_usuario AS
                             select usu.*,iif_string(usu_ativo=false,'Desativado','') as ativo,cad_razao_social,cad_celular,cad_email,cad_telefone,stu_nome,dpu_nome,cad_foto
                             from cad_usuario usu 
                            left join cad_cadastro cad on(usu.cad_id = cad.cad_id)
                            left join cad_setor_usuario stu on(usu.stu_id = stu.stu_id)
                            left join cad_departamento_usuario dpu on(usu.dpu_id = dpu.dpu_id)

                        ";

                        oCheck.AddView(cSqlCreate);
            */


        }



        private void timer1_Tick(object sender, EventArgs e)
        {

            timer1.Enabled = false;
            lIniciarAtualizacao = FuncoesGerais.ConfirmarAcao("Atualizar Bando de Dados ?");
            if (lIniciarAtualizacao)
            {
                lblAtualizacao.Text = "Aguarde... conectando ao banco de dados..";
                lblAtualizacao.Update();
                VerificarBancoDados();

            }

            this.Close();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            string cSql = "";
            Version VersaoSistema = Assembly.GetExecutingAssembly().GetName().Version;
            int NumeroBancoDados = VersaoSistema.Build;

            DataSet dsAtualizacao = new DataSet();

            try
            {

                if (!FuncoesGerais.SearchFileBD("sis_atualizacaobanco"))
                {
                    cSql = "create table sis_atualizacaobanco (atu_id serial,atu_numeroatual integer)";

                    FuncoesGerais.SqlExecute(cSql);

                }

                dsAtualizacao = FuncoesGerais.oxcQuery("select count(1) as qtde from sis_atualizacaobanco");

                if (Int32.Parse(dsAtualizacao.Tables[0].Rows[0]["qtde"].ToString()) == 0)
                {
                    FuncoesGerais.SqlExecute("insert into sis_atualizacaobanco (atu_numeroatual) values(" + NumeroBancoDados.ToString() + ")");
                    lIniciarAtualizacao = true;
                }
                else
                {
                    dsAtualizacao = FuncoesGerais.oxcQuery("select atu_numeroatual from sis_atualizacaobanco");

                    if (Int32.Parse(dsAtualizacao.Tables[0].Rows[0]["atu_numeroatual"].ToString()) < NumeroBancoDados)
                    {
                        lIniciarAtualizacao = true;

                        FuncoesGerais.SqlExecute("update sis_atualizacaobanco set atu_numeroatual=" + NumeroBancoDados.ToString());

                    }



                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro na criação do arquivo de atualização." + Environment.NewLine + ex.Message);
            }

            dsAtualizacao.Dispose();

            timer1.Enabled = true;


        }
    }
}
