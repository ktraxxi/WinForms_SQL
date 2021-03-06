using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace WindowsFormsSqlLearning
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        private SqlConnection NorthWindConnection;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);
            sqlConnection.Open();
            NorthWindConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthWindDB"].ConnectionString);
            NorthWindConnection.Open();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand
                ($"INSERT INTO Students (name, surname, birthday, birthplace, phone, email) VALUES (@name, @surname, @birthday, @birthplace, @phone, @email)",
                sqlConnection);

            DateTime date = DateTime.Parse(textBox3.Text);

            command.Parameters.AddWithValue("name", textBox1.Text); command.Parameters.AddWithValue("surname", textBox2.Text);
            command.Parameters.AddWithValue("birthday", $"{date.Month}/{date.Day}/{date.Year}"); command.Parameters.AddWithValue("birthplace", textBox4.Text);
            command.Parameters.AddWithValue("phone", textBox5.Text); command.Parameters.AddWithValue("email", textBox6.Text);

            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(textBox7.Text, NorthWindConnection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader dataReader = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products", NorthWindConnection);
                dataReader = sqlCommand.ExecuteReader();

                ListViewItem item = null;
                while (dataReader.Read())
                {
                    item = new ListViewItem(new string[] 
                    { 
                        Convert.ToString(dataReader["ProductName"]), 
                        Convert.ToString(dataReader["QuantityPerUnit"]), 
                        Convert.ToString(dataReader["UnitPrice"]) 
                    });
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }
    }
}
