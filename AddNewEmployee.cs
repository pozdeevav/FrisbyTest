using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrisbyTest
{
    public partial class AddNewEmployee : Form
    {
        EmployeesList employees;

        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string connectionString = @"Data Source=DESKTOP-RU9N0S3\SQLSERVERDEVELOP;Initial Catalog=FrisbyTest;Integrated Security=true;";

        public AddNewEmployee(EmployeesList employeesForm)
        {
            InitializeComponent();
            employees = employeesForm;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            employees.Show();
        }

        private void AddNewEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            employees.Show();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(textBox5.Text);
            }
            catch
            {
                textBox5.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string EmploymentDate = monthCalendar1.SelectionRange.Start.ToString().Substring(0, 10);

            string query = "INSERT INTO Employees (SecondName, FirstName, Patronymic, Position, Salary, EmploymentDate) VALUES ('"
                + textBox1.Text + "', '"
                + textBox2.Text + "', '"
                + textBox3.Text + "', '"
                + textBox4.Text + "', "
                + textBox5.Text + ", CAST('"
                + EmploymentDate + "' as date))";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }

            this.Close();

            for (int i = 0; i < employees.dataGridView1.Rows.Count; i++)
                employees.dataGridView1.Rows.RemoveAt(i);

            employees.GetEmployeesTable();

            employees.Show();
        }
    }
}
