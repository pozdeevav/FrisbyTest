using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FrisbyTest
{
    public partial class EmployeesList : Form
    {
        AddNewEmployee newEmployee;

        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;

        string connectionString = @"Data Source=DESKTOP-RU9N0S3\SQLSERVERDEVELOP;Initial Catalog=FrisbyTest;Integrated Security=true;"; //строка подключения к БД

        public EmployeesList()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void EmployeesList_Load(object sender, EventArgs e) //метод при загрузке формы
        {
            GetEmployeesTable();
        }

        private void добавитьНовогоСотрудникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewEmployee addNewEmployee = new AddNewEmployee(this);
            addNewEmployee.Show();
            this.Hide();
        }

        public void GetEmployeesTable()
        {
            string query = "SELECT * FROM Employees"; //SQL-запрос на вывод всех записей из таблицы Employees

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(query, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }

            dataGridView1.Columns[0].HeaderText = "ID сотрудника";
            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Отчество";
            dataGridView1.Columns[4].HeaderText = "Должность";
            dataGridView1.Columns[5].HeaderText = "Оклад";
            dataGridView1.Columns[6].HeaderText = "Дата приема на работу";
            dataGridView1.Columns[7].HeaderText = "Дата увольнения";

            dataGridView1.Columns[0].Visible = false;
        }

        private void удалениеЗаписиОСотрудникеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DeletedEmployee = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value);

            string query = "DELETE FROM Employees WHERE EmployeeId = " + DeletedEmployee;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows.RemoveAt(i);

            GetEmployeesTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> arrayRemove = new List<int>();
            int j = 0;

            if (textBox1.Text != "" && textBox1.Text != null)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Contains(textBox1.Text))
                    { }
                    else
                    {
                        arrayRemove.Add(i);
                    }
                }
                for (int i = arrayRemove.Count-1; i >= 0; i--)
                {                    
                    dataGridView1.Rows.RemoveAt(arrayRemove[i]);
                }
            }
            else
            {
                MessageBox.Show("Заполните поле ввода", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int averageSalary = 0;
            int countEmployees = 0;
            
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                averageSalary += Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
            }

            MessageBox.Show("Количество работников: " + dataGridView1.RowCount + "\n" +
                "Средняя зарпалата: " + averageSalary / Convert.ToInt32(dataGridView1.RowCount), "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows.RemoveAt(i);

            GetEmployeesTable();
        }

        private void получитьСписокСотрудниковВXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XDocument xdocument = new XDocument();
            XElement employees = new XElement("employees");

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                XElement employee = new XElement("employee");
                XElement employeeId = new XElement("employeeId", dataGridView1.Rows[i].Cells[0].Value);
                XElement secondName = new XElement("secondName", dataGridView1.Rows[i].Cells[1].Value);
                XElement firstName = new XElement("firstName", dataGridView1.Rows[i].Cells[2].Value);
                XElement patronymic = new XElement("patronymic", dataGridView1.Rows[i].Cells[3].Value);
                XElement position = new XElement("position", dataGridView1.Rows[i].Cells[4].Value);
                XElement salary = new XElement("salary", dataGridView1.Rows[i].Cells[5].Value);
                XElement employmentDate = new XElement("employmentDate", dataGridView1.Rows[i].Cells[6].Value);
                XElement dismissalDate = new XElement("dismissalDate", dataGridView1.Rows[i].Cells[7].Value);

                employee.Add(employeeId);
                employee.Add(secondName);
                employee.Add(firstName);
                employee.Add(patronymic);
                employee.Add(position);
                employee.Add(salary);
                employee.Add(employmentDate);
                employee.Add(dismissalDate);

                employees.Add(employee);
            }

            xdocument.Add(employees);

            xdocument.Save("employees.xml");

            if (MessageBox.Show("Файл успешно создан\nОткрыть файл?", "Успешно!", MessageBoxButtons.YesNo, MessageBoxIcon.Information)==DialogResult.Yes)
            {
                string path = "X:/FrisbyTest/bin/Debug/employees.xml";
                Process.Start(path);
            }

        }
    }
}
