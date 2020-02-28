
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
using Oracle.ManagedDataAccess.EntityFramework;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AfiseazaBirou();
            IncarcaNume();
        }
        private void AfiseazaBirou()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from birou_is";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                        dataGridView1.ColumnCount = 4;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "cod_birou";
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "nume_birou";
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "adresa_birou";
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "tel_birou";
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

                String sqlCommand = "INSERT INTO birou_is VALUES (cod_birou.nextval, ";
                sqlCommand += "'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')";

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
                MessageBox.Show("Exceptie:"+ " " + ex.Message);
            }

          

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.Visible = true;
            groupBox3.Visible = true;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            var x = dataGridView1[4, currentRowIndex].Value;
            string cod_birou = x.ToString();



            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from birou_is where cod_birou =" + cod_birou;
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();

                //preluarea datelor 
                if (dr.Read())
                {

                    comboBox1.SelectedItem = new ComboItem(dr.GetString(1), dr.GetInt32(0));
                    textBox4.Text = dr.GetString(1);
                    txtAdresa.Text = dr.GetString(2);
                    txtTelefon.Text = dr.GetString(3) + "";

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
                    comboBox1.Items.Add(new ComboItem(dr.GetString(1), (Int32)dr.GetDecimal(0)));
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(this);
            f.Show();
            
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3(this);
            f.Show();
          
        }
        private void Refresh1()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from birou_is", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
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


                String sqlCommand = "UPDATE birou_is set nume_birou = '";
                sqlCommand += textBox4.Text + "'";
                sqlCommand += ", adresa_birou = '" + txtAdresa.Text + "'";
                sqlCommand += ", tel_birou = '" + txtTelefon.Text + "'";
                sqlCommand += " where cod_birou=" + ((ComboItem)comboBox1.SelectedItem).Value; ;



                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Updated");
                    Refresh1();
                }
                else
                {
                    MessageBox.Show("Error");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                OracleConnection conn = new OracleConnection(CONNECTION_STRING);

                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
                var x = dataGridView1[4, currentRowIndex].Value;
                string cod_birou = x.ToString();


                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                String sqlCommand = "DELETE FROM birou_is WHERE cod_birou = '";
                sqlCommand += cod_birou + "'";

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

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Show();
            Hide();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Enter(object sender, EventArgs e)
        {
            button4.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button4_Leave(object sender, EventArgs e)
        {
            button4.ForeColor = Color.LightGray;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "SELECT A.nr_inmatricul, A.marca, A.tip, A.pret_inchiriere_zi, B.nume_birou, B.tel_birou FROM masina_is A, birou_is B WHERE A.cod_birou = B.cod_birou";

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
                    /*

                   // dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "nr_inmatricul";
                   // dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "marca";
                  //  dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "tip";
                  //  dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "pret_inchiriere_zi";
                   // dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "nume_birou";
                  //  dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[5].HeaderText = "tel_birou";
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

        private void button7_Click(object sender, EventArgs e)
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "SELECT Q.nr_contract, Q.cnp, P.nr_inmatricul, P.marca, P.tip FROM masina_is P,contract_is Q WHERE P.nr_inmatricul = Q.nr_inmatricul";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                       // dataGridView1.ColumnCount = 5;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }
                    /*

                    //dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "nr_contract";
                   // dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "cnp";
                   // dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "nr_inmatricul";
                  //  dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "marca";
                   // dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "tip";
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
   

        private void button8_Click(object sender, EventArgs e)
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "SELECT R.nr_contract, R.pret_contract, S.cnp, S.nume, S.prenume,T.nr_inmatricul, T.marca, T.tip, U.nume_birou FROM contract_is R,client_is S, masina_is T,birou_is U WHERE(R.cnp = S.cnp AND R.nr_inmatricul = T.nr_inmatricul)  AND T.cod_birou = U.cod_birou";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);
                     //   dataGridView1.ColumnCount = 5;

                        // setare dataSource pentru controld grid
                        dataGridView1.DataSource = dt;
                    }
                    /*

                   // dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "nr_contract";
                   // dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "pret_contract";
                   // dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[2].HeaderText = "cnp";
                  //  dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "nume";
                   // dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[4].HeaderText = "prenume";
                  //  dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[5].HeaderText = "nr_inmatricul";
                   // dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[6].HeaderText = "marca";
                    //dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[7].HeaderText = "tip";
                    //dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[8].HeaderText = "nume_birou";
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

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.ForeColor = Color.LightGray;
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.ForeColor = Color.LightGray;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.ForeColor = Color.LightGray;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.LightGray;
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.ForeColor = Color.LightGray;
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            button7.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            button7.ForeColor = Color.LightGray;
        }

        private void button8_MouseEnter(object sender, EventArgs e)
        {
            button8.ForeColor = Color.FromArgb(229, 126, 45);
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            button8.ForeColor = Color.LightGray;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Refresh1();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
 }

