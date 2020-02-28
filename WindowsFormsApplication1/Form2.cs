
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
    public partial class Form2 : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        Form1 original;
        public Form2(Form1 incoming)
        {
            InitializeComponent();
            original = incoming;
            original.Hide();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            AfiseazaMasini();
            IncarcaNume();
        }
        private void AfiseazaMasini()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from masina_is";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                        dataGridView1.ColumnCount = 8;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "nr_inmatricul";
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "marca";
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "tip";
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "pret_inchiriere_zi";
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "combustibil";
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[5].HeaderText = "tip_cutie_viteze";
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[6].HeaderText = "inchiriata";
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[7].HeaderText = "cod_birou";
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

        private void button1_Click(object sender, EventArgs e)
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

                String sqlCommand = "INSERT INTO masina_is  (nr_inmatricul,marca ,tip, pret_inchiriere_zi,combustibil, tip_cutie_viteze, inchiriata, cod_birou) VALUES";

                sqlCommand += "('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "'," + int.Parse(textBox4.Text) + ",'" + comboBox4.SelectedItem.ToString() + "','" + comboBox2.SelectedItem.ToString() + "','" + comboBox3.SelectedItem.ToString() + "'," +((ComboItem)comboBox1.SelectedItem).Value +")";




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
        private void IncarcaNume()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select cod_birou, nume_birou from birou_is";
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();

                //preluarea datelor și plasarea lor într-un combobox
                while (dr.Read())
                {
                    comboBox1.Items.Add(new ComboItem(dr.GetString(1), (int)dr.GetDecimal(0)));
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
        private void Refresh1()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from masina_is", conn);
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
                var x = dataGridView1[8, currentRowIndex].Value;
                string nr_inmatricul = x.ToString();


                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                String sqlCommand = "DELETE FROM masina_is WHERE nr_inmatricul = '";
                sqlCommand += nr_inmatricul + "'";

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select marca, tip, pret_inchiriere_zi,combustibil, tip_cutie_viteze from masina_is where inchiriata like 'nu%'";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                        //dataGridView1.ColumnCount = 14;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }

                    // dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "marca";
                    // dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "tip";
                    //  dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "pret_inchiriere_zi";
                    //  dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "combustibil";
                    // dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "tip_cutie_viteze";
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            Hide();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            this.Close();
            original.Show();
        }
    }
}
