﻿using DataBase;
using System;
using System.Windows.Forms;

namespace Possible_Benefits
{
    public partial class FrmLoading : Form
    {
        public FrmLoading()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (pbLoading.Value < 100)
            {
                pbLoading.Value += 5;
            }
            else
            {
                timer.Stop();
                this.Visible = false;
                try
                {
                    if (!DB.ExistsDataBase())
                    {
                        DB.CreateDatabase();
                        DB.CreateTables();
                    }

                    new FrmBalanceSheet().ShowDialog();
                }
                catch
                {
                    MessageBox.Show("Houve um problema no servidor. Tente novamente. Caso o erro persista contate o suporte.", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }
    }
}
