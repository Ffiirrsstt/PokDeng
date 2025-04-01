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

        //object[] cards;

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
            //สับ
            List<Cards> new_cards_data1 = cards_game.shuffle_cards(cards_data);
            //แจก
            List<List<Cards>> new_cards_data = cards_game.deal_cards(new_cards_data1, 2);
            PokDeng pokdeng_game = new PokDeng();

            //admin
            pictureBox16.Image = Image.FromStream(new MemoryStream(new_cards_data[1][0].picture));
            pictureBox17.Image = Image.FromStream(new MemoryStream(new_cards_data[1][1].picture));
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;

            var result = pokdeng_game.much_cards_hand(new_cards_data[1]);
            richTextBox1.Text += "แต้ม : " + result.points_cards.ToString() + ", จ่ายกี่เท่า : " + result.times_pay.ToString() + " | เป็นไพ่ชนิดพิเศษไหม : " + result.special_hands.ToString() + " คือ " + result.special_hands_type.ToString() + "*มีใบที่สูงสุด คือ ไพ่ :" + result.hierarchy.ToString();

            //ผู้เล่น
            pictureBox14.Image = Image.FromStream(new MemoryStream(new_cards_data[0][0].picture)); //ผู้เล่นคนแรก , ไพ่ใบที่
            pictureBox15.Image = Image.FromStream(new MemoryStream(new_cards_data[0][1].picture));
            pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;

            result = pokdeng_game.much_cards_hand(new_cards_data[0]);
            richTextBox2.Text += "แต้ม : " + result.points_cards.ToString() + ", จ่ายกี่เท่า : " + result.times_pay.ToString() + " | เป็นไพ่ชนิดพิเศษไหม : " + result.special_hands.ToString() + " คือ " + result.special_hands_type.ToString() + "*มีใบที่สูงสุด คือ ไพ่ :" + result.hierarchy.ToString();
            //(points_cards, times_pay, special_hands, special_hands_type, hierarchy)

            /*foreach (Cards card in cards_data) {
                richTextBox1.Text += card.id+" "+ card.name;
            }

            Cards_Games cards_game = new Cards_Games();

            List<Cards> new_cards_data = cards_game.shuffle_cards(cards_data);
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
