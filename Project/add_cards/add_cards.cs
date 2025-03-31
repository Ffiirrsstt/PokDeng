using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace add_cards
{
    public partial class add_cards : Form
    {
        string conStr =
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\USE\\project363\\DB\\DB.mdb";

        public add_cards()
        {
            InitializeComponent();
        }

        void add_data_cards()
        {
            OleDbConnection conn = new OleDbConnection(conStr);
            conn.Open();

            string sql = "INSERT INTO Cards " +
                "(Card_Name, Card_Picture, Card_Rank, Card_Suit) VALUES (@name, @picture, @rank, @suit)";
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

            foreach (string suit in suits)
            {
                //for (int rank = 0; rank < ranks.Length; rank++)
                foreach (string rank in ranks)
                {
                    string cardName = $"{rank} of {suit}";
                    string imagePath = $"D:\\USE\\project363\\img\\card\\{cardName}.png"; // ไฟล์ภาพของไพ่
                    //byte[] imageBytes = System.IO.File.Exists(imagePath) ? System.IO.File.ReadAllBytes(imagePath) : new byte[0];
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("name", cardName);
                    cmd.Parameters.AddWithValue("picture", imageBytes);
                    cmd.Parameters.AddWithValue("rank", rank);
                    cmd.Parameters.AddWithValue("suit", suit);

                    cmd.ExecuteNonQuery();
                }
            }
            
            conn.Close();
        }

        /*private void add_cards_Load(object sender, EventArgs e)
        {

            string conStr =
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\USE\\project363\\DB\\DB.mdb";
            OleDbConnection conn = new OleDbConnection(conStr);
            conn.Open();
            string sql = "select * from Cards";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataSet data = new DataSet();
            adapter.Fill(data, "Pit");

            byte[] imgByte = (byte[])data.Tables["Pit"].Rows[0]["Card_Picture"];
            MemoryStream imgStream = new MemoryStream(imgByte);
            //pictureBox1.Image = Image.FromStream(imgStream);
        }*/
    }
}
