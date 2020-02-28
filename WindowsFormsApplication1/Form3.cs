
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        Form1 original;
        public Form3(Form1 incoming)
        {
            InitializeComponent();
            original = incoming;
            original.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            AfiseazaClienti();
        }
        private void AfiseazaClienti()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from client_is";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                       // dataGridView1.ColumnCount = 7;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }
                    /*
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "cnp";
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "varsta";
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "nume";
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "prenume";
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "sex";
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[5].HeaderText = "adr_email";
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[6].HeaderText = "telefon";
                    */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }

        }
        private void Refresh1()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from client_is", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

      
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                OracleConnection conn = new OracleConnection(CONNECTION_STRING);

                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
                var x = dataGridView1[0, currentRowIndex].Value;
                string cnp = x.ToString();


                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                String sqlCommand = "DELETE FROM client_is WHERE cnp = '";
                sqlCommand += cnp + "'";

                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Sters!");
                    Refresh1();
                }
                else
                {
                    MessageBox.Show("Eroare");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exceptie" + ex.Message);
            }
        }
     
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OracleConnection conn = new OracleConnection(CONNECTION_STRING);

                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                String sqlCommand = "INSERT INTO client_is (cnp,varsta,nume,prenume,sex,adr_email,telefon) VALUES";

                sqlCommand += "('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + comboBox2.SelectedItem.ToString() + "','" + textBox6.Text + "','" + textBox7.Text + "')";




                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Adaugat");
                    Refresh1();
                }
                else
                {
                    MessageBox.Show("Eroare");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exceptie" + ex.Message);
            }

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            var x = dataGridView1[6, currentRowIndex].Value;
            string cnp = x.ToString();



            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from client_is where cnp =" + cnp;
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();

                //preluarea datelor 
                if (dr.Read())
                {

                    
                    textBox1.Text = dr.GetString(0) + "";
                    textBox2.Text = dr.GetString(1) + "";
                    textBox3.Text = dr.GetString(2);
                    textBox4.Text = dr.GetString(3);
                    comboBox2.SelectedItem = dr.GetString(4);
                    textBox6.Text = dr.GetString(6);
                    textBox7.Text = dr.GetString(7) + "";

                }

                //inchiderea conexiunii
                conn.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }
            
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            this.Close();
            original.Show();
        }
    }
}
