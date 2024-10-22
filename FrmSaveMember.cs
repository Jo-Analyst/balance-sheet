﻿using DataBase;
using Balance_Sheet.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace Balance_Sheet
{
    public partial class FrmSaveMember : Form
    {

        public bool wasDataSaved { get; set; }
        Member member = new Member();

        int memberId, benefits_id, personId;
        bool isEdition;

        public FrmSaveMember(int personId, string responsible)
        {
            InitializeComponent();

            this.personId = personId;
            lblResponsible.Text = responsible;
            LoadMembers();
        }

        private void LoadMembers()
        {
           DataTable dtMembers = Member.FindByPersonId(personId);

            foreach (DataRow row in dtMembers.Rows)
            {
                addDgvMember(row["name"].ToString(), row["cpf"].ToString(), row["birth"].ToString(), row["phone"].ToString(), row["address"].ToString(), row["number_address"].ToString(), int.Parse(row["id"].ToString()));
            }
        }

        public bool ValidatedFields()
        {
            bool validated = false;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Preencha o nome!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
            }
            else if (mkCPF.MaskCompleted && !ValidateCPF.validate(mkCPF.Text))
                MessageBox.Show("CPF inválido!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (btnSave.Text == "Salvar" && mkCPF.MaskCompleted && member.FindByCPF(mkCPF.Text).Rows.Count == 1 && !isEdition)
                MessageBox.Show("Não foi possível cadastrar. O CPF já está cadastrado!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (mkCPF.MaskCompleted && member.FindByCpfForMember(mkCPF.Text, memberId).Rows.Count == 1 && isEdition)
                MessageBox.Show("Não foi possível editar. O CPF que está tentando editar já está cadastrado no sistema", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                validated = true;

            return validated;
        }

        private void FrmSaveMember_KeyDown(object sender, KeyEventArgs e)
        {
            if (btnSave.Text == "Novo" && e.Control && e.KeyCode == Keys.N || btnSave.Text == "Salvar" && e.Control && e.KeyCode == Keys.S)
            {
                btnSave_Click(sender, e);
            }
        }

        private void ndNumberOfMembers_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (char.IsDigit(e.KeyChar) && (e.KeyChar != (char)8))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (char.IsDigit(e.KeyChar) && (e.KeyChar != (char)8))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Caixa Fácil", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int indexRowPress;

        private void dgvMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)
            {
                dgvMembers.ClearSelection();
                return;
            }

            if (dgvMembers.CurrentCell.ColumnIndex == 0)
            {
                EditBenefits();
                indexRowPress = e.RowIndex;
            }
            else if (dgvMembers.CurrentCell.ColumnIndex == 1)
                DeleteMembers();

            clearSelection(e);
}

        private void DeleteMembers()
        {
            try
            {
                DialogResult dr = MessageBox.Show($"Deseja mesmo excluir {dgvMembers.CurrentRow.Cells["ColName"]} da base de dados?", "Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    Member.Delete(int.Parse(dgvMembers.CurrentRow.Cells["ColId"].Value.ToString()));
                    dgvMembers.Rows.Remove(dgvMembers.CurrentRow);
                }

                dgvMembers.ClearSelection();

            }
            catch
            {
                MessageBox.Show("Houve um erro ao excluir. Feche o aplicativo e tente novamente. Caso o erro persista entre em contato com o suporte", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditBenefits()
        {
            benefits_id = int.Parse(dgvMembers.CurrentRow.Cells[2].Value.ToString());
        }

        private void clearSelection(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dgvMembers.ClearSelection();
            }
        }

        private void dgvMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            clearSelection(e);
        }

        public bool IsFieldDecimal(TextBox value)
        {
            bool isDecimal = false;
            if (!string.IsNullOrEmpty(value.Text) && !decimal.TryParse(value.Text, out decimal result))
            {
                value.Clear();
                value.Focus();
            }
            else
                isDecimal = true;

            return isDecimal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidatedFields())
                return;

            if (btnSave.Text.ToLower() == "salvar")
            {
                SalveMember();

            }
            else
            {
                ClearFieldsMember();
                EnabledFieldsMember();
                txtName.Focus();
            }

            toolTip.SetToolTip(btnSave, "Salvar - [CTRL + S]");
            btnSave.Text = "Salvar";

        }

        private void ClearFieldsMember()
        {
            txtName.Clear();
            mkCPF.Clear();
            mkPhone.Clear();
            txtAddress.Clear();
            txtNumberAddress.Clear();
            memberId = 0;
        }

        private void EnabledFieldsMember()
        {
            txtName.Enabled = true;
            mkCPF.Enabled = true;
            mkPhone.Enabled = true;
            txtAddress.Enabled = true;
            txtNumberAddress.Enabled = true;
        }

        private void DisabledFieldsMember()
        {
            txtName.Enabled = false;
            mkCPF.Enabled = false;
            mkPhone.Enabled = false;
            txtAddress.Enabled = false;
            txtNumberAddress.Enabled = false;
        }

        private void SalveMember()
        {
            try
            {
                member.id = memberId;
                member.name = txtName.Text.Trim();
                member.CPF = mkCPF.MaskCompleted ? mkCPF.Text.Trim() : string.Empty;
                member.address = txtAddress.Text.Trim();
                member.birth = dtBirth.Text;
                member.phone = mkPhone.MaskCompleted ? mkPhone.Text.Trim() : string.Empty;
                member.numberAddress = txtNumberAddress.Text.Trim();
                member.personId = personId;

                member.Save();
                addDgvMember(txtName.Text, mkCPF.Text, dtBirth.Value.ToShortDateString(), mkPhone.Text, txtAddress.Text, txtNumberAddress.Text, personId);
                mkCPF.Text = member.CPF;
                memberId = member.id;
                wasDataSaved = true;
                ClearFieldsMember();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addDgvMember(string name, string cpf, string birth, string phone, string address, string number, int memberId)
        {
            int index = dgvMembers.Rows.Add();
            dgvMembers.Rows[index].Cells["ColEdit"].Value = Resources.Custom_Icon_Design_Flatastic_1_Edit_24;
            dgvMembers.Rows[index].Cells["ColDelete"].Value = Resources.trash_24_icon;

            dgvMembers.Rows[index].Cells["ColId"].Value = memberId.ToString();
            dgvMembers.Rows[index].Cells["ColName"].Value = name.Trim();
            dgvMembers.Rows[index].Cells["ColCPF"].Value = cpf.Trim();
            dgvMembers.Rows[index].Cells["ColBirth"].Value = birth;
            dgvMembers.Rows[index].Cells["ColPhone"].Value = phone;
            dgvMembers.Rows[index].Cells["ColAddress"].Value = address.Trim();
            dgvMembers.Rows[index].Cells["ColNumber"].Value = number.Trim();
            dgvMembers.Rows[index].Height = 35;
        }

        ToolTip toolTip = new ToolTip();

        private void FrmSavePerson_Load(object sender, EventArgs e)
        {
            toolTip.SetToolTip(btnSave, "Salvar - [CTRL + S]");
        }


        private void dgvMembers_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvMembers.Cursor = e.ColumnIndex == 0 || e.ColumnIndex == 1 ? Cursors.Hand : Cursors.Arrow;
        }
    }
}