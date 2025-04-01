using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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

        /*
         อยากลืมทำยอดติดลบ ถ้าติดลบเชิญออก*/

        async Task fetchDataCards()
        {
            Player player = new Player();
            money_player_label.Text = player.display();
            int bet = 5000;

            player.deduct_bet(bet);

            Cards cards = new Cards();
            List<Cards> cards_data = await cards.fetchDataCards();

            Cards_Games cards_game = new Cards_Games();
            //สับ
            List<Cards> shuffleCards = cards_game.shuffle_cards(cards_data);
            //แจก
            List<List<Cards>> cardCard_hands = cards_game.deal_cards(shuffleCards, 2);
            PokDeng pokdeng_game = new PokDeng();
            List<int> player_draw = new List<int> { 0,1 };
            cardCard_hands = cards_game.draw_additionalCard(cardCard_hands, player_draw, 1, shuffleCards);

            //admin
            pictureBox16.Image = Image.FromStream(new MemoryStream(cardCard_hands[1][0].picture));
            pictureBox17.Image = Image.FromStream(new MemoryStream(cardCard_hands[1][1].picture));
            pictureBox21.Image = Image.FromStream(new MemoryStream(cardCard_hands[1][2].picture));
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox21.SizeMode = PictureBoxSizeMode.StretchImage;

            PokDeng resultDealer = pokdeng_game.much_cards_hand(cardCard_hands[1]);
            richTextBox1.Text += "แต้ม : " + resultDealer.points_cards.ToString() + ", จ่ายกี่เท่า : " + resultDealer.times_pay.ToString() + " | เป็นไพ่ชนิดพิเศษไหม : " + resultDealer.special_hands.ToString() + " คือ " + resultDealer.special_hands_type.ToString() + "*มีใบที่สูงสุด คือ ไพ่ :" + resultDealer.hierarchy.ToString();

            //ผู้เล่น
            pictureBox14.Image = Image.FromStream(new MemoryStream(cardCard_hands[0][0].picture)); //ผู้เล่นคนแรก , ไพ่ใบที่
            pictureBox15.Image = Image.FromStream(new MemoryStream(cardCard_hands[0][1].picture));
            pictureBox22.Image = Image.FromStream(new MemoryStream(cardCard_hands[0][2].picture));
            pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox22.SizeMode = PictureBoxSizeMode.StretchImage;

            PokDeng resultUser = pokdeng_game.much_cards_hand(cardCard_hands[0]);
            richTextBox2.Text += "แต้ม : " + resultUser.points_cards.ToString() + ", จ่ายกี่เท่า : " + resultUser.times_pay.ToString() + " | เป็นไพ่ชนิดพิเศษไหม : " + resultUser.special_hands.ToString() + " คือ " + resultUser.special_hands_type.ToString() + "*มีใบที่สูงสุด คือ ไพ่ :" + resultUser.hierarchy.ToString();

            dictionary_PokDeng dic = new dictionary_PokDeng();
            List<string> listResult = dic.result_game;
            string result = pokdeng_game.win_lose_draw(resultUser, resultDealer);
            if (result == listResult[3]) MessageBox.Show("ตัวคำนวณผลแพ้-ชนะ Error น่ะ");

            string txt="";
            if (result == listResult[0])
            {
                //ชนะ
                int times_pay = resultUser.times_pay;
                txt = "ได้รับ : " + times_pay + "เท่า";
                player.money_in(times_pay, bet);
                MessageBox.Show(player.money.ToString());
            }
            else if (result == listResult[1])
            {
                //แพ้
                int times_pay = resultDealer.times_pay;
                txt = "จ่าย : " + times_pay + "เท่า";
                player.money_out(times_pay, bet);
                MessageBox.Show(player.money.ToString());
            }
            else if (result == listResult[2])
            {
                txt = "จ่าย : " + "ได้รับเงินคืน";
                player.refund_bet(bet);
                MessageBox.Show(player.money.ToString());
            }

            richTextBox3.Text = "ผล : " + result + Environment.NewLine + txt;
            money_player_label.Text = player.display();



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
            Player player = new Player();

            money_player_label1.Text = player.money.ToString(); 
        }
    }
}
