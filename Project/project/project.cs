using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class ST111 : Form
    {
        /*
         string conStr =
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\USE\\project363\\DB\\DB.mdb";
        OleDbConnection conn;

        DataSet data = new DataSet();
        */

        object[] cards;

        public ST111()
        {
            InitializeComponent();
        }

        void setting_loadding() {
            fetchDataCards();
        }

        async Task fetchDataCards()
        {
            Cards cards = new Cards();
            List<Cards> cards_data = await cards.fetchDataCards();

            Cards_Games cards_game = new Cards_Games();
            List<List<Cards>> new_cards_data = cards_game.deal_cards(cards_data, 2);

            pictureBox14.Image = Image.FromStream(new MemoryStream(new_cards_data[0][0].picture)); //ผู้เล่นคนแรก , ไพ่ใบที่
            pictureBox15.Image = Image.FromStream(new MemoryStream(new_cards_data[0][1].picture));

            //admin
            pictureBox16.Image = Image.FromStream(new MemoryStream(new_cards_data[1][0].picture));
            pictureBox17.Image = Image.FromStream(new MemoryStream(new_cards_data[1][1].picture));

            /*foreach (Cards card in cards_data) {
                richTextBox1.Text += card.id+" "+ card.name;
            }

            Cards_Games cards_game = new Cards_Games();

            List<Cards> new_cards_data = cards_game.ShuffleCards(cards_data);
            foreach (Cards card in new_cards_data)
            {
                richTextBox2.Text += card.id + " " + card.name;
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
    }
}
