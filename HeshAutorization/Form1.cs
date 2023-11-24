using System;
using SD = System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Runtime.Remoting.Contexts;
using System.Reflection.Emit;

namespace HeshAutorization
{
    public partial class Form1 : Form
    {
        Form2 f = new Form2();
        Form3 f2 = new Form3();
        Form4 f3 = new Form4();
        string roll;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-N9HFFH1\MSQLSERVER; Initial Catalog=WS; Integrated Security=True");
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

        public void openConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        private void button_Aut_Click(object sender, EventArgs e)
        {
            string Login = textBox1.Text.ToString();
            string Password = textBox2.Text.ToString();
            DataTable tata = new DataTable();
            openConnection();

            SqlCommand sc = new SqlCommand($"select Роль from avtoe where Логин='{Login}' and Пароль='{Password}'", sqlConnection);
            sqlDataAdapter.SelectCommand = sc;
            sqlDataAdapter.Fill(tata);
            string kk;

            foreach (DataRow row in tata.Rows)
            {
                kk = row["Роль"].ToString();
            }

            switch (kk)
            {
                case "Администратор":
                    f.Show();
                    break;

                case "Оператор":
                    f2.Show();
                    break;

                case "Абонент":
                    f3.Show();
                    break;
            }

            if (Proverka(Login, Password))
            {
                MessageBox.Show("Вы вошли в систему", "ура");
            }
            else
            {
                MessageBox.Show("Перепроверьте данные", "упс");
            }

            
       
            closeConnection();
            
        }

        private void button_Reg_Click(object sender, EventArgs e)
        {
            string LoginReg = textBox3.Text.ToString();
            string PasswordReg1 = textBox4.Text.ToString();
            string PasswordReg2 = textBox5.Text.ToString();
            string Name = textBox8.Text.ToString();
            string lastName = textBox7.Text.ToString();
            string post = textBox6.Text.ToString();

            if (radioButton1.Checked)
            {
                roll = "Администратор";
            }
            if (radioButton2.Checked)
            {
                roll = "Оператор";
            }
            if (radioButton2.Checked)
            {
                roll = "Абонент";
            }

            if (PasswordReg1 == PasswordReg2)
            {
                openConnection();

                if (Proverka(LoginReg, PasswordReg1) == true)
                {
                    MessageBox.Show("Аккаунт не создан", "упс");
                }
                else if (Proverka(LoginReg, PasswordReg1) == false)
                {
                    PasswordReg1 = CreateMD5(PasswordReg1);
                    string commandString = $"insert into avtoe(Логин, Пароль, Имя, Фамилия, Должность, Роль) values('{LoginReg}', '{PasswordReg1}', '{Name}', '{lastName}', '{post}', '{roll}')";
                    SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Аккаунт был создан", "ура");

                    }
                    else
                    {
                        MessageBox.Show("Аккаунт не создан", "упс");
                    }
                }
            }

            else
            {
                MessageBox.Show("Пароли разные", "упс");
            }

            closeConnection();
        }

        

        private Boolean Proverka(string log, string pass)
        {
            DataTable table = new DataTable();
            string Login = log;
            string Password = pass;

            Password = CreateMD5(Password);

            string commandString = $"select Логин, Пароль from avtoe where Логин='{Login}' and Пароль='{Password}'";
            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);
            

            sqlDataAdapter.SelectCommand = sqlCommand;
            sqlDataAdapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
