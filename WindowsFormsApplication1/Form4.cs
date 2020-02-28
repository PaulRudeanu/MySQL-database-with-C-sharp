
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
    public partial class Form4 : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            AfiseazaContracte();
            IncarcaCNP();
            IncarcaNr();
        }
        private void AfiseazaContracte()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from contract_is";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                        dataGridView1.ColumnCount = 5;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "nr_contract";
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "data";
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "pret_contract";
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "cnp";
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "nr_inmatricul";

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
        private void IncarcaCNP()
        {
             OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select cnp from client_is";
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();

                //preluarea datelor și plasarea lor într-un combobox
                while (dr.Read())
                {
                    // comboBox1.Items.Add(new ComboItem(dr.GetString(1), (Int32)dr.GetDecimal(0)));
                    comboBox1.Items.Add(dr.GetString(0));
                   // MessageBox.Show(dr.GetString(0));
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
        private string IncarcaPret(string x)
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            string a= string.Empty;
            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                //cmd.CommandText = "select nr_inmatricul, pret_inchiriere_zi from masina_is";
                cmd.CommandText = "select pret_inchiriere_zi from masina_is where nr_inmatricul = '" + x + "'";
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();

                //preluarea datelor și plasarea lor într-un combobox

                //comboBox2.Items.Add(new ComboItem(dr.GetString(0), (Int32)dr.GetDecimal(1)));
                //comboBox2.Items.Add(dr.GetString(0));
                dr.Read();
                a = dr.GetInt32(0)+"";
                
                
                //inchiderea conexiunii
                conn.Dispose();
               // return a;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }
            return a;

        }

        private void IncarcaNr()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                //cmd.CommandText = "select nr_inmatricul, pret_inchiriere_zi from masina_is";
                cmd.CommandText = "select nr_inmatricul from masina_is";
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();

                //preluarea datelor și plasarea lor într-un combobox
                while (dr.Read())
                {
                    //comboBox2.Items.Add(new ComboItem(dr.GetString(0), (Int32)dr.GetDecimal(1)));
                    comboBox2.Items.Add(dr.GetString(0));
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

              //  String sqlCommand = "INSERT INTO contract_is (nr_contract,data,perioada_zile,pret_contract,cnp,nr_inmatricul) VALUES";

              //  sqlCommand += "('" + textBox1.Text + "', sysdate " + ",'" + textBox3.Text+"" + "','" + label6.Text+ "','" + comboBox1.SelectedItem.ToString() + "','" + comboBox2.SelectedItem.ToString()+ "')";

              //  String sqlCommand = "INSERT INTO contract_is VALUES (nr_contract.nextval, ";
               // sqlCommand += "'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')";
              

                String sqlCommand = "INSERT INTO contract_is VALUES (nr_contract.nextval, ";
                sqlCommand +="sysdate" + ",'" + textBox3.Text + "','" + label6.Text + "','" + comboBox1.SelectedItem.ToString() + "','" + comboBox2.SelectedItem.ToString() + "')";
              //  sqlCommand += "'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')";



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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
          /*  MessageBox.Show(IncarcaPret(comboBox2.SelectedItem.ToString()));
            int y =Convert.ToInt32(IncarcaPret(comboBox2.SelectedItem.ToString()));
            int x = Convert.ToInt32(textBox3.Text);
            int rez = x * y;
            label6.Text = rez +"";
            */
        }
        private void Refresh1()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from contract_is", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("Pretul/zi este: "+IncarcaPret(comboBox2.SelectedItem.ToString()));
            int y = Convert.ToInt32(IncarcaPret(comboBox2.SelectedItem.ToString()));
            int x = Convert.ToInt32(textBox3.Text);
            int rez = x * y;
            label6.Text = rez + "";
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
