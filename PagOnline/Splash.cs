using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace PagOnline
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


            oCheck.cTableName = "cad_paises";
            oCheck.cComentario = "Cadastro dos Paises";
            oCheck.cSchema = "public";

            oCheck.AddCampo("pai_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("pai_nome", "varchar", 100, 0, "Nome do País", false, "");
            oCheck.AddCampo("pai_sigla", "varchar", 5, 0, "Sigla do País", false, "");
            
            oCheck.addindice("ind_paises_01", "upper(pai_sigla)", true);

            oCheck.Run();


            oCheck.cTableName = "cad_estado";
            oCheck.cComentario = "Cadastro dos Estados";
            oCheck.cSchema = "public";

            oCheck.AddCampo("uf_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("uf_nome", "varchar", 100, 0, "Nome do Estado", false, "");
            oCheck.AddCampo("uf_sigla", "varchar", 5, 0, "Sigla do Estado", false, "");
            oCheck.AddCampo("pai_id", "integer", 0, 0, "Id do Páis", false, "");

            oCheck.addindice("ind_estado_01", "upper(uf_sigla)", true);
            oCheck.addcontraint("a", "pai_id", "cad_paises", "pai_id", "on update cascade on delete restrict");

            oCheck.Run();

            oCheck.cTableName = "cad_cidade";
            oCheck.cComentario = "Cadastro das Cidades";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cid_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cid_nome", "varchar", 100, 0, "Nome da Cidade", false, "");
            oCheck.AddCampo("uf_id", "integer", 0, 0, "Id Estado", false, "");
            oCheck.addcontraint("a", "uf_id", "cad_estado", "uf_id", "on update cascade on delete restrict");

            oCheck.addindice("ind_cidade_01", "uf_id,upper(cid_nome)", true);

            oCheck.Run();

            oCheck.cTableName = "cad_bairro";
            oCheck.cComentario = "Cadastro dos Bairros";
            oCheck.cSchema = "public";

            oCheck.AddCampo("bai_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("bai_nome", "varchar", 100, 0, "Nome do País", false, "");
            oCheck.AddCampo("cid_id", "integer", 0, 0, "Id Cidade", false, "");
            oCheck.addcontraint("a", "cid_id", "cad_cidade", "cid_id", "on update cascade on delete restrict");

            oCheck.addindice("ind_bairro_01", "cid_id,upper(bai_nome)", true);

            oCheck.Run();


            oCheck.cTableName = "cad_logradouro";
            oCheck.cComentario = "Cadastro dos Logradouro";
            oCheck.cSchema = "public";

            oCheck.AddCampo("lgr_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("lgr_nome", "varchar", 100, 0, "Nome do País", false, "");
            oCheck.AddCampo("lgr_cep", "varchar", 9, 0, "Nome do País", false, "");
            oCheck.AddCampo("bai_id", "integer", 0, 0, "Id Cidade", false, "");
            oCheck.addcontraint("a", "bai_id", "cad_bairro", "bai_id", "on update cascade on delete restrict");

            oCheck.addindice("ind_logradouro_01", "lgr_cep,upper(lgr_nome)", true);

            oCheck.Run();

            oCheck.cTableName = "cad_plataforma";
            oCheck.cComentario = "Cadastro das Plataformas";
            oCheck.cSchema = "public";

            oCheck.AddCampo("pla_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("pla_nome", "varchar", 100, 0, "Nome da Plataforma", false, "");
            oCheck.AddCampo("pla_site", "varchar", 100, 0, "Site", false, "");
            oCheck.AddCampo("pla_app_id", "varchar", 100, 0, "Id App na Plataforma", false, "");
            
            oCheck.addindice("ind_plataforma_01", "upper(pla_nome)", true);

            oCheck.Run();

            oCheck.cTableName = "sis_configuracao";
            oCheck.cComentario = "Configuração Interna";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cfg_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cfg_client_id", "varchar", 100, 0, "Client Id Generet", false, "");
            oCheck.AddCampo("cfg_secret_id", "varchar", 100, 0, "Secret Id", false, "");
            oCheck.AddCampo("cfg_percentual_taxa", "numeric", 12, 3, "Percentual Taxa", false, "0");
            
            oCheck.Run();


            oCheck.cTableName = "cad_cadastro";
            oCheck.cComentario = "Cadastro das Pessoas e Empresas";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cad_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cad_razao_social", "varchar", 100, 0, "Razão Social", false, "");
            oCheck.AddCampo("cad_nome_fantasia", "varchar", 100, 0, "Nome Fantasia", false, "");
            oCheck.AddCampo("cad_email", "varchar", 100, 0, "Email", false, "");
            oCheck.AddCampo("cad_telefone_fixo", "varchar", 30, 0, "Telefone Fixo", false, "");
            oCheck.AddCampo("cad_telefone_celular", "varchar", 30, 0, "Telefone Celular", false, "");
            oCheck.AddCampo("cad_telefone_fax", "varchar", 30, 0, "Telefone Fax", false, "");
            oCheck.AddCampo("cad_sexo", "varchar", 10, 0, "Sexo", false, "'Masculino'");
            oCheck.AddCampo("cad_cnpj_cpf", "varchar", 18, 0, "Número CNPJ/CPF", false, "");
            oCheck.AddCampo("cad_inscricao_estadual", "varchar", 20, 0, "Inscrição Estadual", false, "");
            oCheck.AddCampo("cad_site", "varchar", 100, 0, "Site", false, "");
            oCheck.AddCampo("cad_ativo", "boolean", 0, 0, "Ativo?", false, "true");
            oCheck.AddCampo("cad_inscricao_municipal", "varchar", 100, 0, "Inscricao Municipal", false, "");
            oCheck.AddCampo("cad_endereco_facebook", "text", 0, 0, "Link Facebook", false, "");
            oCheck.AddCampo("cad_endereco_youtube", "text", 0, 0, "Link Youtube", false, "");
            oCheck.AddCampo("cad_endereco_instagram", "text", 0, 0, "Link Instagram", false, "");
            oCheck.AddCampo("cad_data_inclusao", "timestamp", 0, 0, "Data de Inclusao", false, "");
            oCheck.AddCampo("cad_data_nascimento", "date", 0, 0, "Data de Nascimento", false, "");
            
            oCheck.addindice("ind_cadastro_01", "cad_cnpj_cpf", true);

            oCheck.Run();


            oCheck.cTableName = "cad_cadastro_cliente";
            oCheck.cComentario = "Acesso dos Clientes";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cadcli_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cad_id", "integer", 0, 0, "Id Cadastro", false, "");
            oCheck.AddCampo("lgr_id", "integer", 0, 0, "Id Logradouro", false, "");
            oCheck.AddCampo("cadcli_senha_acesso", "varchar", 100, 0, "Senha de Acesso", false, "");
            oCheck.AddCampo("cadcli_numero_endereco", "varchar", 10, 0, "Numero Endereço", false, "");
            oCheck.AddCampo("cadcli_complemento_endereco", "varchar", 50, 0, "Complemento", false, "");
            oCheck.AddCampo("cadcli_status", "varchar", 20, 0, "Situação", false, "'Pendente'");
            

            oCheck.addcontraint("a", "cad_id", "cad_cadastro", "cad_id", "on update cascade on delete cascade");
            oCheck.addcontraint("b", "cadcli_status", new string[] { "Pendente", "Ativo","Suspenso" });

            oCheck.Run();

            oCheck.cTableName = "cad_cadastro_lojista";
            oCheck.cComentario = "Acesso dos Lojistas";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cadloj_id", "serial", 0, 0, "Id Primario", false, "");
            
            oCheck.AddCampo("cad_id", "integer", 0, 0, "Id Cadastro", false, "");
            oCheck.AddCampo("lgr_id", "integer", 0, 0, "Id Logradouro", false, "");
            oCheck.AddCampo("pla_id", "integer", 0, 0, "Id Plataforma", false, "");

            oCheck.AddCampo("cadloj_senha_acesso", "varchar", 100, 0, "Senha de Acesso", false, "");
            oCheck.AddCampo("cadloj_numero_endereco", "varchar", 10, 0, "Numero Endereço", false, "");
            oCheck.AddCampo("cadloj_complemento_endereco", "varchar", 50, 0, "Complemento", false, "");

            oCheck.AddCampo("cadloj_codigo_banco", "varchar", 50, 0, "Código Banco", false, "");
            oCheck.AddCampo("cadloj_numero_conta", "varchar", 50, 0, "Número Conta", false, "");
            oCheck.AddCampo("cadloj_digito_conta", "varchar", 50, 0, "Digito Conta", false, "");
            oCheck.AddCampo("cadloj_agencia", "varchar", 50, 0, "Número Agencia", false, "");
            oCheck.AddCampo("cadloj_digito_agencia", "varchar", 50, 0, "Digito Agencia", false, "");
            
            oCheck.AddCampo("cadloj_chave_api", "text", 0, 0, "Chave Api", false, "");
            oCheck.AddCampo("cadloj_senha_api", "text", 0, 0, "Senha Api", false, "");
            oCheck.AddCampo("cadloj_url_plataforma", "text", 0, 0, "URL", false, "");
            oCheck.AddCampo("cadloj_segredo_compartilhado", "text", 0, 0, "Segredo Compartilhado", false, "");
            
            oCheck.AddCampo("cadloj_percentual_taxa", "numeric", 12, 3, "Percentual Taxa", false, "0");
            
            oCheck.AddCampo("cadloj_status", "varchar", 20, 0, "Situação", false, "'Em análise'");


            oCheck.addcontraint("a", "cad_id", "cad_cadastro", "cad_id", "on update cascade on delete cascade");
            oCheck.addcontraint("b", "cadloj_status", new string[] { "Em análise", "Reprovado", "Aprovado","Suspenso","Bloqueada" });

            oCheck.Run();


            oCheck.cTableName = "cad_grupo_acesso_lojista";
            oCheck.cComentario = "Cadastro de Acesso";
            oCheck.cSchema = "public";

            oCheck.AddCampo("grpaceloj_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cadloj_id", "integer", 0, 0, "Id Lojista", false, "");
            oCheck.AddCampo("grpaceloj_nome", "varchar", 100, 0, "Nome Grupo", false, "");
            oCheck.AddCampo("grpaceloj_opcoes", "json", 0, 0, "Json das Opcoes", false, "");

            oCheck.addindice("ind_grupoacessolojista_01", "cadloj_id,upper(grpaceloj_nome)", true);
            oCheck.addcontraint("a", "cadloj_id", "cad_cadastro_lojista", "cadloj_id", "on update cascade on delete cascade");

            oCheck.Run();


            oCheck.cTableName = "cad_lojista_colaborador";
            oCheck.cComentario = "Cadastro dos Colaboradores do Lojista";
            oCheck.cSchema = "public";

            oCheck.AddCampo("cob_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cadloj_id", "integer", 0, 0, "Id Lojista", false, "");
            oCheck.AddCampo("cad_id", "integer", 0, 0, "Id Cadastro", false, "");
            oCheck.AddCampo("cob_senha_acesso", "varchar", 100, 0, "Senha Acesso", false, "");
            oCheck.AddCampo("grpaceloj_id", "integer", 0, 0, "Id Grupo Acesso Lojista", false, "");

            oCheck.addindice("ind_colaborador_01", "cadloj_id,cad_id", true);
            oCheck.addcontraint("a", "cad_id", "cad_cadastro", "cad_id", "on update cascade on delete restrict");
            oCheck.addcontraint("b", "cadloj_id", "cad_cadastro_lojista", "cadloj_id", "on update cascade on delete cascade");

            oCheck.Run();


            oCheck.cTableName = "cad_produto";
            oCheck.cComentario = "Cadastro dos Produtos Lojistas";
            oCheck.cSchema = "public";

            oCheck.AddCampo("prd_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cadloj_id", "integer", 0, 0, "Id Lojista", false, "");
            oCheck.AddCampo("prd_nome", "varchar", 100, 0, "Descrição Produto", false, "");
            oCheck.AddCampo("prd_marca", "varchar", 100, 0, "Marca", false, "");
            oCheck.AddCampo("prd_codigo_lojista", "varchar", 100, 0, "Código Lojista", false, "");
            oCheck.AddCampo("prd_numero_ean", "varchar", 100, 0, "Número Ean", false, "");
            oCheck.AddCampo("prd_complemento", "text", 0, 0, "Complemento", false, "");
            oCheck.AddCampo("prd_id_plataforma", "varchar", 100, 0, "Id Produto na PLataforma", false, "");
            oCheck.AddCampo("prd_peso_bruto", "numeric", 12, 4, "Peso Bruto", false, "0");
            oCheck.AddCampo("prd_peso_liquido", "numeric", 12, 4, "Peso Liquido", false, "0");
            oCheck.AddCampo("prd_valor_custo", "numeric", 12, 2, "Valor Custo", false, "0");
            oCheck.AddCampo("prd_valor_venda", "numeric", 12, 2, "Valor Venda", false, "0");
            
            oCheck.addindice("ind_produto_01", "cadloj_id,upper(prd_codigo_lojista)", true);
            oCheck.addcontraint("a", "cadloj_id", "cad_cadastro_lojista", "cadloj_id", "on update cascade on delete cascade");

            oCheck.Run();



            oCheck.cTableName = "cad_grupo_acesso";
            oCheck.cComentario = "Cadastro de Acesso";
            oCheck.cSchema = "public";

            oCheck.AddCampo("grpace_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("grpace_nome", "varchar", 100, 0, "Login", false, "");
            oCheck.AddCampo("grpace_opcoes", "json", 0, 0, "Json das Opcoes", false, "");

            oCheck.addindice("ind_grupoacesso_01", "upper(grpace_nome)", true);

            oCheck.Run();


            oCheck.cTableName = "cad_usuario";
            oCheck.cComentario = "Cadastro dos Usuarios";
            oCheck.cSchema = "public";

            oCheck.AddCampo("usu_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cad_id", "integer", 0, 0, "Id Cadastro", false, "");
            oCheck.AddCampo("usu_login", "varchar", 100, 0, "Login", false, "");
            oCheck.AddCampo("usu_senha_acesso", "varchar", 100, 0, "Senha Acesso", false, "");
            oCheck.AddCampo("usu_ativo", "boolean", 0, 0, "Ativo ?", false, "true");
            oCheck.AddCampo("grpace_id", "integer", 0, 0, "Id GrupoAcesso", false, "");

            oCheck.addindice("ind_usuario_01", "upper(usu_login)", true);
            oCheck.addcontraint("a", "grpace_id", "cad_grupo_acesso", "grpace_id", "on update cascade on delete restrict");

            oCheck.Run();


            oCheck.cTableName = "mov_pedido";
            oCheck.cComentario = "Pedidos Processados";
            oCheck.cSchema = "public";

            oCheck.AddCampo("ped_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cadloj_id", "integer", 0, 0, "Id Lojista", false, "");
            oCheck.AddCampo("cad_id", "integer", 0, 0, "Id Cadastro", false, "");
            oCheck.AddCampo("pla_id", "integer", 0, 0, "Id Plataforma", false, "");
            oCheck.AddCampo("lgr_id", "integer", 0, 0, "Id Plataforma", false, "");
            oCheck.AddCampo("ped_numero_endereco", "varchar", 10, 0, "Número Endereco", false, "");
            oCheck.AddCampo("ped_complemento_endereco", "varchar", 50, 0, "Complemento Endereco", false, "");
            oCheck.AddCampo("ped_data_inclusao", "timestamp", 0, 0, "Data Inclusao", false, "");
            oCheck.AddCampo("ped_data_pedido", "timestamp", 0, 0, "Data Pedido", false, "");
            oCheck.AddCampo("ped_total_liquido", "numeric", 12, 2, "Total Liquido", false, "0");
            oCheck.AddCampo("ped_total_bruto", "numeric", 12, 2, "Total Bruto", false, "0");
            oCheck.AddCampo("ped_valor_frete", "numeric", 12, 2, "Valor Frete", false, "0");
            oCheck.AddCampo("ped_valor_desconto", "numeric", 12, 2, "Total Desconto", false, "0");
            oCheck.AddCampo("ped_valor_taxa", "numeric", 12, 2, "Valor Taxa", false, "0");
            oCheck.AddCampo("ped_percentual_taxa", "numeric", 12, 3, "Percentual Taxa", false, "0");
            oCheck.AddCampo("ped_valor_saque", "numeric", 12, 2, "Total Saque", false, "0");
            oCheck.AddCampo("ped_valor_sacado", "numeric", 12, 2, "Total Sacado", false, "0");
            oCheck.AddCampo("ped_situacao_financeira", "varchar", 20, 0, "Situação Financeira", false, "");
            oCheck.AddCampo("ped_id_transacao_pagamento", "varchar", 100, 0, "ID Pagamento GerenciaNet", false, "");
            oCheck.AddCampo("ped_id_transacao_plataforma", "varchar", 100, 0, "Id Transacao PLataforma", false, "");

            oCheck.AddCampo("ped_json_plataforma", "json", 0, 0, "Json PLataforma", false, "");
            oCheck.AddCampo("ped_json_pagamento", "json", 0, 0, "Json Pagamento", false, "");

            oCheck.addindice("ind_pedido_01", "cadloj_id,cad_id");
            oCheck.addindice("ind_pedido_02", "pla_id,cadloj_id");
            oCheck.addindice("ind_pedido_03", "cadloj_id,ped_data_pedido");

            oCheck.addcontraint("a", "cad_id", "cad_cadastro", "cad_id", "on update cascade on delete restrict");
            oCheck.addcontraint("b", "cadloj_id", "cad_cadastro_lojista", "cadloj_id", "on update cascade on delete restrict");
            oCheck.addcontraint("c", "pla_id", "cad_plataforma", "pla_id", "on update cascade on delete restrict");
            oCheck.addcontraint("d", "lgr_id", "cad_logradouro", "lgr_id", "on update cascade on delete restrict");

            oCheck.Run();

            oCheck.cTableName = "mov_pedido_itens";
            oCheck.cComentario = "items do Pedido";
            oCheck.cSchema = "public";

            oCheck.AddCampo("pedite_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("ped_id", "integer", 0, 0, "Id Pedido", false, "");
            oCheck.AddCampo("prd_id", "integer", 0, 0, "Id Produto", false, "");
            oCheck.AddCampo("pedite_unidade", "varchar", 10, 0, "Unidade", false, "");
            oCheck.AddCampo("pedite_quantidade", "numeric", 12, 3, "Quantidade", false, "0");
            oCheck.AddCampo("pedite_valor_unitario", "numeric", 12, 2, "Valor Unitario", false, "0");
            oCheck.AddCampo("pedite_total_bruto", "numeric", 12, 2, "Total Bruto", false, "0");
            oCheck.AddCampo("pedite_valo_desconto", "numeric", 12, 2, "Valor Desconto", false, "0");
            oCheck.AddCampo("pedite_total_liquido", "numeric", 12, 2, "Total Liquido", false, "0");


            oCheck.addindice("ind_peditem_01", "ped_id,prd_id");
            oCheck.addcontraint("a", "ped_id", "mov_pedido", "ped_id", "on update cascade on delete cascade");
            oCheck.addcontraint("b", "prd_id", "cad_produto", "prd_id", "on update cascade on delete restrict");

            oCheck.Run();



            oCheck.cTableName = "mov_solicitacao_saque";
            oCheck.cComentario = "Solicitacao do Saque";
            oCheck.cSchema = "public";

            oCheck.AddCampo("sol_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("cadloj_id", "integer", 0, 0, "Id Lojista", false, "");
            oCheck.AddCampo("sol_situacao", "varchar", 10, 0, "Situacao do Saque", false, "'Pendente'");
            oCheck.AddCampo("sol_valor_saque", "numeric", 12, 2, "Valor Saque", false, "0");
            oCheck.AddCampo("sol_data_solicitacao", "timestamp", 0,0 , "Data Solicitacao", false, "");
            oCheck.AddCampo("sol_id_transacao", "varchar", 50, 0, "Id Transacao Pagamento", false, "");
            oCheck.AddCampo("usu_id_acao", "integer", 0, 0, "Usuario que tomou acao", false, "");
            oCheck.AddCampo("sol_data_acao", "timestamp", 0, 0, "Data Acao", false, "");
            oCheck.AddCampo("sol_motivo_revogacao", "text", 0, 0, "Motivo Revogação", false, "");

            oCheck.addindice("ind_saque_01", "cadloj_id");
            oCheck.addcontraint("a", "cadloj_id", "cad_cadastro_lojista", "cadloj_id", "on update cascade on delete restrict");
            oCheck.addcontraint("b", "sol_situacao", new string[] { "Pendente", "Aprovado", "Pago", "Revogado" });

            oCheck.Run();



            oCheck.cTableName = "mov_pedido_saque";
            oCheck.cComentario = "Saque dos Pedidos";
            oCheck.cSchema = "public";

            oCheck.AddCampo("pedsaq_id", "serial", 0, 0, "Id Primario", false, "");
            oCheck.AddCampo("sol_id", "integer", 0, 0, "Id Saque", false, "");
            oCheck.AddCampo("ped_id", "integer", 0, 0, "Id Pedido", false, "");
            oCheck.AddCampo("peqsaq_valor_saque", "numeric", 12, 2, "Valor Saque", false, "0");
            oCheck.AddCampo("peqsaq_situacao", "varchar", 20, 0, "Situacao", false, "'Pendente'");


            oCheck.addindice("ind_pedsaque_01", "ped_id,sol_id");
            oCheck.addcontraint("a", "ped_id", "mov_pedido", "ped_id", "on update cascade on delete cascade");
            oCheck.addcontraint("b", "sol_id", "mov_solicitacao_saque", "sol_id", "on update cascade on delete cascade");
            oCheck.addcontraint("c", "peqsaq_situacao", new string[] { "Pendente", "Aprovado","Revogado" });

            oCheck.Run();



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
