using MySql.Data.MySqlClient;
using System;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace AjandekListaApp
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection("Server = localhost; Database = ajandek; Uid = root; Pwd = ");
            conn.Open();
            this.FormClosed += (sender, args) =>
            {
                if (conn != null)
                {
                    conn.Close();
                }

            };
            AdatBetoltes();
        }

        void AdatBetoltes()
        {
            string sql = @"
SELECT id, nev, uzlet 
FROM ajandek
ORDER BY id
";
            var comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string nev = reader.GetString("nev");

                    string uzlet;

                    try
                    {
                        uzlet = reader.GetString("uzlet");
                    }
                    catch (SqlNullValueException ex)
                    {
                        uzlet = null;
                    }

                    var ajandek = new Ajandek(id, nev, uzlet);
                    lbox_termekek.Items.Add(ajandek);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ajandek p = (Ajandek)lbox_termekek.SelectedItem;
            if (lbox_termekek.SelectedItem == null)
            {
                txt_nev.Text = " ";
                txt_uzlet.Text = " ";
            }
            else
            {
                txt_nev.Text = p.Nev;
                txt_uzlet.Text = p.Uzlet;
            }


        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            int id = 0;
            for (int i = 0; i < lbox_termekek.Items.Count; i++)
            {
                Ajandek p = (Ajandek)lbox_termekek.Items[i];
                id = p.Id;
                id++;
            }
            var nev = txt_nev.Text;

            var uzlet = txt_uzlet.Text;

            if (txt_uzlet.Text == "")
            {
                uzlet = "Saját készítésű";
            }


            var ajandek = new Ajandek(id, nev, uzlet);

            if (nev.Length > 1000 || uzlet.Length > 1000)
            {
                MessageBox.Show("Túl hosszú adatot adtál meg!");
            }
            else if (nev == "")
            {
                MessageBox.Show("nem lehet üres a név mező!");
            }
            else
            {
                var insertComm = conn.CreateCommand();
                insertComm.CommandText = @"
INSERT INTO ajandek (id, nev, uzlet)
VALUES (@id, @nev, @uzlet);
";
                insertComm.Parameters.AddWithValue("@id", null);
                insertComm.Parameters.AddWithValue("@nev", nev);
                insertComm.Parameters.AddWithValue("@uzlet", uzlet);
                int erintettSorok = insertComm.ExecuteNonQuery();
                lbox_termekek.Items.Add(ajandek);
            }

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {

            var deleteComm = conn.CreateCommand();
            deleteComm.CommandText = @"
Delete from ajandek where ajandek.id = @id;
";
            Ajandek p = (Ajandek)lbox_termekek.SelectedItem;
            int id = p.Id;
            deleteComm.Parameters.AddWithValue("@id", id);
            int erintettSorok = deleteComm.ExecuteNonQuery();
            lbox_termekek.Items.RemoveAt(lbox_termekek.SelectedIndex);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            Ajandek p = (Ajandek)lbox_termekek.SelectedItem;
            int id = p.Id;
            if (txt_uzlet.Text == "")
            {
                txt_uzlet.Text = "Saját készítésű";
            }

            Ajandek p2 = new Ajandek(id, txt_nev.Text, txt_uzlet.Text);
            lbox_termekek.Items[lbox_termekek.SelectedIndex] = p2;

            var editComm = conn.CreateCommand();
            editComm.CommandText = @"
UPDATE ajandek SET nev = @nev, uzlet = @uzlet WHERE ajandek.id = @id;
";

            editComm.Parameters.AddWithValue("@id", id);
            editComm.Parameters.AddWithValue("@nev", txt_nev.Text);
            editComm.Parameters.AddWithValue("@uzlet", txt_uzlet.Text);
            int erintettSorok = editComm.ExecuteNonQuery();
        }
    }
}
