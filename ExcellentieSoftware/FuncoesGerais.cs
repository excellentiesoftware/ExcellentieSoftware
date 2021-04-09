using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
//using Syncfusion.HtmlConverter;


namespace ExcellentieSoftware
{
    public class FuncoesGerais
    {

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        public static bool PermiteCps(string codigo, bool lMostraErro = true)
        {
            bool bOK = true;

            if (!bOK)
            {
                if (lMostraErro)
                    MessageBox.Show("Você não tem permissão.", "(" + codigo + ")");
            }


            return bOK;
        }

        public static object IIf(bool expression, object truePart, object falsePart)
        { return expression ? truePart : falsePart; }

        public static string QuoteStr(string cConteudo)
        { return "'" + cConteudo + "'"; }


        public static void Mensagem(string cMensagem)
        {
            MessageBox.Show(cMensagem);
        }

        // "https://maps.googleapis.com/maps/api/distancematrix/json?key="+cKey+"&origins=\\"+cOrig+;
        //"&destinations="+cDest+"&mode=driving&language=pt-BR&sensor=false"

        public static string[] DistanciaGeoLocalizacaoGoogle(string Origem, string Destino)
        {
            string[] aPosicao = { "0", "0" };
            string cKeyGoogle = "AIzaSyC3CTwcQaDGua2o4T9KuQ2j866J0-BS9V8";
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/xml?key={2}&origins=\\{0}&destinations={1}&mode=walking&language=pt-BR&sensor=false", Uri.EscapeDataString(Origem), Uri.EscapeDataString(Destino), cKeyGoogle);

            try
            {
                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                XDocument xdoc = XDocument.Load(response.GetResponseStream());

                XElement result = xdoc.Element("DistanceMatrixResponse").Element("row").Element("element");
                XElement locationduracao = result.Element("duration");
                XElement locationdistancia = result.Element("distance");

                aPosicao[0] = locationduracao.Element("text").Value;
                aPosicao[1] = locationdistancia.Element("text").Value;

                response.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na acesso a API Google Maps (" + ex.Message + ")");
            }
            return aPosicao;
        }

        public static string[] PegaGeoLocalizacaoGoogle(string Localizacao)
        {
            string[] aPosicao = { "0", "0" };
            string cKeyGoogle = "AIzaSyC3CTwcQaDGua2o4T9KuQ2j866J0-BS9V8";


            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(Localizacao), cKeyGoogle);

            try
            {
                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                XDocument xdoc = XDocument.Load(response.GetResponseStream());

                XElement result = xdoc.Element("GeocodeResponse").Element("result");
                XElement locationElement = result.Element("geometry").Element("location");
                XElement lat = locationElement.Element("lat");
                XElement lng = locationElement.Element("lng");
                aPosicao[0] = lat.Value;
                aPosicao[1] = lng.Value;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na acesso a API Google Maps (" + ex.Message + ")");
                aPosicao[0] = "0";
                aPosicao[1] = "0";
            }
            return aPosicao;
        }


        public static bool SearchFileBD(string arquivo, Conexao ds = null)
        {
            bool lAchado = false;
            DataSet dsArquivo = new DataSet();

            try
            {
                Conexao ds_conexao;
                if (ds == null)
                {
                    ds_conexao = new Conexao();
                    ds_conexao.Conecta();
                }
                else
                {
                    ds_conexao = ds;
                }

                dsArquivo = FuncoesGerais.oxcQuery("SELECT count(1) as qtde from pg_tables where tablename ='" + arquivo + "'", ds_conexao);

                lAchado = Int32.Parse(dsArquivo.Tables[0].Rows[0]["qtde"].ToString()) > 0;

                if (ds == null)
                    ds_conexao.desconecta();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro SQL:" + ex.Message);
            }
            return lAchado;

        }

        public static bool SqlExecute(string comandosql, Conexao ds = null, bool lAvisoMensagem = true)
        {
            bool lOk = true;
            Conexao ds_conexao;
            if (ds == null)
            {
                ds_conexao = new Conexao();
                ds_conexao.Conecta();
            }
            else
            {
                ds_conexao = ds;
            }
            try
            {

                ds_conexao.StartTransaction();
                ds_conexao.ExecutarSql(comandosql);
                ds_conexao.Commit();

            }
            catch (Exception ex)
            {
                lOk = false;
                ds_conexao.RollBack();
                if (lAvisoMensagem)
                    MessageBox.Show("Erro SQL:" + ex.Message);
            }

            if (ds == null)
                ds_conexao.desconecta();


            return lOk;
        }


        
        public static DataSet oxcQuery(string cSql, Conexao ds_conexao)
        {
            DataSet dsDados = new DataSet();
            NpgsqlDataAdapter QrDados = new NpgsqlDataAdapter(cSql, ds_conexao.DsCps);
            QrDados.Fill(dsDados);

            return dsDados;
        }

        public static bool xIn(object Obj, object[] Arr)
        {
            bool bOK = false;
            int index = Array.BinarySearch(Arr, Obj,
                        StringComparer.CurrentCulture);


            if (index >= 0)
                bOK = true;

            return bOK;
        }

        

        public static byte[] PegaImagemUrl(string curl)
        {
            string imageUrl = curl;
            byte[] imageBytes = null;

            try
            {
                HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                WebResponse imageResponse = imageRequest.GetResponse();

                Stream responseStream = imageResponse.GetResponseStream();

                using (BinaryReader br = new BinaryReader(responseStream))
                {
                    imageBytes = br.ReadBytes(500000);
                    br.Close();
                }
                responseStream.Close();
                imageResponse.Close();

            }
            catch (Exception ex)
            {

            }

            return imageBytes;
        }
        public static DataSet oxcQuery(string cSql)
        {
            Conexao ds_conexao = new Conexao();
            ds_conexao.Conecta();
            DataSet dsDados = new DataSet();
            NpgsqlDataAdapter QrDados = new NpgsqlDataAdapter(cSql, ds_conexao.DsCps);
            QrDados.Fill(dsDados);
            ds_conexao.desconecta();

            return dsDados;
        }
        public static bool ConfirmarAcao(string cMensagem = "")
        {

            if (MessageBox.Show(cMensagem, "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;
            else
                return false;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

    }
}
