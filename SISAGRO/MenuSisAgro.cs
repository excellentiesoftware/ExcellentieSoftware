using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SISAGRO
{
    public partial class MenuSisAgro : Form
    {
        public MenuSisAgro()
        {
            InitializeComponent();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            SplashForm Formulario = new SplashForm();
            Formulario.ShowDialog();
        }
    }
}
