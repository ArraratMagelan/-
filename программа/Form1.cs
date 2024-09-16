using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace программа
{
    public partial class Form1 : Form
    {
        private UserActivityMonitor _activityMonitor;
        private Timer _timer;
        private DateTime _startTime;
        private int flag = 0;
        private string timestop = "";
        private string timestart = "";
        private string sqlcon= "Server=localhost;Port=5432;Database=postgres;User Id=maks_user; Password=qwer1234QWER;";
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeTimer();
        }
        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Add("ActivityTime", "Last Activity Time");
        }

        private void InitializeTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000; 
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_activityMonitor.LastActivityTime.AddSeconds(30) < DateTime.Now && _activityMonitor.LastActivityTime != _startTime) // Если 30 секунд простоя
            {
                dataGridView1.Rows.Add("Последняя активность: " + _activityMonitor.LastActivityTime);
                _startTime = _activityMonitor.LastActivityTime;
                flag = 1;
                timestop= _activityMonitor.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (flag == 1)
            {
                if (_activityMonitor.LastActivityTime.AddSeconds(2) >= DateTime.Now)
                {
                    dataGridView1.Rows.Add("Пользователь снова активен: " + _activityMonitor.LastActivityTime);
                    flag = 0;
                    timestart = _activityMonitor.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");
                    dataGridView2.DataSource = null;
                    NpgsqlConnection sqlConnection = new NpgsqlConnection(sqlcon);
                    sqlConnection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = sqlConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("insert into soxr_vremia values(default, '{0}', '{1}')", timestop,timestart);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    sqlConnection.Close();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _activityMonitor = new UserActivityMonitor();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _activityMonitor.Dispose();
            _timer.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            NpgsqlConnection sqlConnection = new NpgsqlConnection(sqlcon);
            sqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = String.Format("select * from soxr_vremia;");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable data = new DataTable();
                data.Load(reader);
                dataGridView2.DataSource = data;
            }
            reader.Close();
            command.Dispose();
            sqlConnection.Close();
        }
    }
}
