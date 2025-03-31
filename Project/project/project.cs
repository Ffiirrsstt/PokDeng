using System; 
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class ST111 : Form
    {
        string conStr =
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\USE\\project363\\DB\\DB.mdb";
        OleDbConnection conn;

        DataSet data = new DataSet();

        object[] cards;

        public ST111()
        {
            InitializeComponent();
        }

        //void setting_loadding() {
        async Task setting_loadding() {
            await fetchDataCards();
        }

        //void fetchDataCards()
        async Task fetchDataCards()
        {
            OleDbConnection conn = new OleDbConnection(conStr);
            conn.Open();
            string sql = "select * from Cards";
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            adapter.Fill(data, "Cards");

            //cards = data.Tables["Cards"].Rows[0].ItemArray;
            DataTable cards_table = data.Tables["Cards"];

            /*foreach (DataRow card in cards_table.Rows) {
                //richTextBox1.Text += card["Card_Name"]+" "+card["Card_Suit"];
            }*/
        }

        void game_intro()
        {
            //await Task.Delay(5000);
            //this.BackgroundImage = Image.FromFile("D:\\USE\\img\\project363\\Intro.png");

        }

        private void ST111_Load(object sender, EventArgs e)
        {
            setting_loadding();
        }

        private void pokdeng_game_Click(object sender, EventArgs e)
        {

        }



        /*public MyForm()
        {
            // สร้าง PictureBox
            pic = new PictureBox
            {
                Image = Image.FromFile("your_image.png"),
                SizeMode = PictureBoxSizeMode.AutoSize,
                Location = new Point(100, 100) // ตำแหน่งเริ่มต้น
            };
            Controls.Add(pic);

            // ตั้งค่า Timer
            timer = new Timer { Interval = 10 }; // หน่วงเวลา 10ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // คำนวณตำแหน่งใหม่แบบ Smooth
            int newX = pic.Left + Math.Sign(targetX - pic.Left) * Math.Min(step, Math.Abs(targetX - pic.Left));
            int newY = pic.Top + Math.Sign(targetY - pic.Top) * Math.Min(step, Math.Abs(targetY - pic.Top));

            pic.Location = new Point(newX, newY);

            // หยุดเมื่อถึงเป้าหมาย
            if (pic.Left == targetX && pic.Top == targetY)
            {
                timer.Stop();
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new MyForm());
        }*/
    }
}
