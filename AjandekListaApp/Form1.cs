using MySql.Data.MySqlClient;
using System;
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
ORDER BY nev
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
                    catch (sqlNullValueException ex)
                    {
                        uzlet = null;
                    }

                    var ajandek = new Ajandek(id, nev, uzlet);
                    listBox1.Items.Add(ajandek);
                    }
                }
            }

            private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

        private void btn_add_Click(object sender, EventArgs e)
        {
            int id = 0;
            var nev = txt_nev.Text;
            var uzlet = txt_uzlet.Text;
            var ajandek = new Ajandek(id, nev, uzlet);

            var insertComm = conn.CreateCommand();
            insertComm.CommandText = @"
INSERT INTO ajandek (id, nev, uzlet)
VALUES (@id, @nev, @uzlet);
";
            insertComm.Parameters.AddWithValue("@id", null);
            insertComm.Parameters.AddWithValue("@nev", nev);
            insertComm.Parameters.AddWithValue("@uzlet", uzlet);
            int erintettSorok = insertComm.ExecuteNonQuery();


        }
    }
    }
