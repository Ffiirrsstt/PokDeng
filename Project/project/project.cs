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
         ถ้ายอดเงินต่ำกว่า 2000 ไล่เลย ถือว่าไม่พอเล่นละ
         */
        List<TabPage> tab_list;
        Tab tab;
        Picture pic;
        Player player;
        Calculate cal = new Calculate();
        Cards_Games cards_game;
        Cards cards;
        List<Cards> cards_data;
        PokDeng pokdeng;
        List<List<Cards>> card_hands;
        Cards_Games_UI_deal cards_ui;
        int speed = 5; //ความไวไพ่ (ความไวเคลื่อนที่ของไพ่อะแหละจ้ะ)

        int bet_default = 100000; //วงเงินเดิมพันเริ่มต้นของผู้เล่น - ก็คือแจกเงินตอนแรกอะแหละ
        double bet=2000; //เงินที่เดิมพันในแต่ละตา 
        int page_1 = 0, page_2 = 1, page_3 = 3;
        int pic_1 = 0, pic_2 = 1,pic_3= 2,pic_4 = 3;

        public ST111()
        {
            InitializeComponent();
            tab_list = new List<TabPage> { page_main, page_newgame_pokdeng, page_play_pokdeng };
            //ออกแบบ dic ให้แก้ง่ายหน่อย
            tab = new Tab(tabControl, dic_tab());
            pic = new Picture(dic_pic());
            cards_game = new Cards_Games();
            cards_ui = new Cards_Games_UI_deal();

        }

        Dictionary<int, TabPage> dic_tab()
        {
            return new Dictionary<int, TabPage>
            {
                //เวลาแก้ไขชื่อไรได้ง่าย ๆ
                { page_1, page_main },
                { page_2, page_newgame_pokdeng },
                { page_3, page_play_pokdeng }
            };
        }

        Tuple<PictureBox, Point, Size> tuple_bet_chip(PictureBox chip) => Tuple.Create(chip, chip.Location, chip.Size);

        Dictionary<int, Tuple<PictureBox, Point, Size>> dic_pic()
        {
            return new Dictionary<int, Tuple<PictureBox, Point,Size>>
            {
                { pic_1, tuple_bet_chip(bet_2K) },
                { pic_2, tuple_bet_chip(bet_5K) },
                { pic_3, tuple_bet_chip(bet_10K)},
                { pic_4, tuple_bet_chip(bet_50K)}
            };
        }

        /*
         อยากลืมทำยอดติดลบ ถ้าติดลบเชิญออก*/


        async Task fetch_data_cards()
        {
            cards = new Cards();
            cards_data = await cards.fetchDataCards();
        }
        void game_intro()
        {
            //await Task.Delay(5000);
            //this.BackgroundImage = Image.FromFile("D:\\USE\\img\\project363\\Intro.png");

        }

        void displayTXT_display_bet_start(double bet) => display_bet_start.Text = "เงินเดิมพันเริ่มต้น : $ " + cal.display_money(bet);


        void new_game_pokdeng()
        {
            //แสดงเงินปัจจุบัน
            money_player_waitBet.Text = player.display();
            //ตั้งค่า default ว่าเลือก chip 2k
            select_bet2K();

            tab.new_pokdeng_game();
            pic.restore_size_chip(); //จัดการชิปให้เข้าที่และขนาดเท่ากัน
        }

        void testSystem()
        {
            player = new Player(10000);
            money_player_label.Text = player.display();
            money_player_waitBet.Text= player.display();
        }






        private void ST111_Load(object sender, EventArgs e)
        {
            fetch_data_cards();

            //tab.hide_start_program();
            testSystem();

            displayTXT_display_bet_start(bet_default);
            textbox_bet_start.Text = bet_default.ToString();
        }

        private void btn_new_pokdeng_game_Click(object sender, EventArgs e)
        {
            double bet_start = cal.string_ToDouble(textbox_bet_start.Text);
            //แปลว่ามีข้อผิดพลาดเกิดขึ้น - อย่าพึ่งเข้าหน้าเปิดเกม
            if (bet_start == -1) return;

            player = new Player(bet_start); //เริ่มใหม่ทุกครั้งที่กดเริ่มเกมใหม่น่ะ
            new_game_pokdeng();
        }
        private void textbox_bet_start_TextChanged(object sender, EventArgs e)
        {
            double bet_start = cal.string_ToDouble(textbox_bet_start.Text, false);
            displayTXT_display_bet_start(bet_start);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            animation_deal();

        }

        void animation_deal()
        {
            int card_number_change = 0; //ไว้แก้อะนะ มันไปแก้ใน if ข้างในไม่ได้ เพราะเปลี่ยนแปลงข้อมูล dic ขณะวน loop อยู่ไม่ได้
            foreach (var card in dic_deck)
            {
                var (pic, loc_target, _, start_move) = card.Value;
                int card_number = card.Key;
                int current_X = pic.Location.X, current_Y = pic.Location.Y;

                /*ถ้าใบที่ 1 2 3 ถึงที่หมายแล้ว ให้ใบถัดไปแจกต่ออะนะ เหมือนเวลาแจกไพ่ทีละใบอะแหละ
                //ส่วนต่อจากใบที่ 4 จะเป็น 5 ซึ่ง 5 กับ 6 มันเป็นใบที่แล้วแต่คนว่าอยากจั่วป่าวน่ะ*/
                if (current_X <= loc_target.X && current_Y >= loc_target.Y && (card_number == 1 || card_number == 2 || card_number == 3))
                {
                    card_number_change = card_number + 1;
                    continue;
                }
                if (!start_move) continue; // ข้ามถ้ายังไม่ต้องเคลื่อนที่ - ยังไม่แสดงอนิเมชันแจกอะแหละ

                //ในที่นี้ค่าราว ๆ x = 742 y = 46 || เป้าหมายโซน x 375 , y บน150 ล่าง 450
                if (current_X > loc_target.X) current_X -= speed;
                if (current_Y < loc_target.Y) current_Y += speed;

                pic.Location = new Point(current_X, current_Y);


                this.Invalidate();
            }

            dic_deck = cards_ui.setting_deal_default(dic_deck, timer, card_number_change,startGame_deal, btn_draw_card, btn_not_draw_card, richTextBox1);

            /*//จะเช็กว่าแจกเริ่มต้นคนละสองใบเสร็จหรือยังอะแหละ
            //เช็กที่ใบที่สี่ เพราะทำทีละใบ ถ้าใบที่สี่เสร็จ แปลว่าแจกคนละสองใบเสร็จละ
            //มี startGame_deal ด้วย ให้รู้ว่าเป็น timer ที่ให้เริ่มทำเพราะอยากแจกคนละสองใบ ไม่ใช่ timer ที่ให้ทำเพื่อให้จั่วเพิ่ม(กรณีให้สิทธิ์จั่ว) - ไม่งั้นกลายเป็นหยุดจับเวลา ทั้ง ๆ ที่ไพ่ใบที่จั่วเพิ่มยังแจกไม่เสร็จ เพราะไปเช็กพบว่าไพ่ที่ต้องแจกคนละสองใบแจกเสร็จแล้ว เลยหยุดจับเวลาอะนะ*/
            Point loc_card_number_fourth = dic_deck[4].pic.Location;
            Point target_card_number_fourth = dic_deck[4].loc_target;

            cards_ui.dealing_cards_each_player
            (player, dic_deck, card_hands, timer, startGame_deal, btn_draw_card, btn_not_draw_card, bet, money_player_label ,loc_card_number_fourth, target_card_number_fourth);


        }

        void setting_pic_Stretch(PictureBox pic) => pic.SizeMode = PictureBoxSizeMode.StretchImage;

        bool startGame_deal = false; //เอาไว้เช็กเพื่อจะหยุด timer น่ะ ไม่ให้มันจับตลอดเวลา

        private Dictionary<int, (PictureBox pic, Point loc_target, bool display, bool start_move)> dic_deck;

        (PictureBox pic, Point loc_target, bool display, bool start_move) //dis=true,bool move=false | เช่น รอหนึ่งเสร็จ สองค่อยขยับ
            tuple_dic_deck(PictureBox pic, int x, int y,bool dis=true,bool move=false) => (pic, new Point(x, y),dis,move);

        void test_()
        {
            pictureBox18.Image = Image.FromStream(new MemoryStream(card_hands[0][0].picture));
            pictureBox9.Image = Image.FromStream(new MemoryStream(card_hands[1][0].picture));
            pictureBox19.Image = Image.FromStream(new MemoryStream(card_hands[0][1].picture));
            pictureBox10.Image = Image.FromStream(new MemoryStream(card_hands[1][1].picture));
            /*pictureBox12.Image = Image.FromStream(new MemoryStream(card_hands[0][2].picture));
            pictureBox11.Image = Image.FromStream(new MemoryStream(card_hands[1][2].picture));*/

            pic.setting_pic_list_Stretch(new List<PictureBox>{ pictureBox18,pictureBox9,pictureBox19,pictureBox10,pictureBox12,pictureBox11});
        }

        //ป็อกเด้งเริ่มต้นแจกสองอะสิ
        void animation_deal_default(List<List<Cards>> card_hands)
        {
            startGame_deal = true;
            test_();

            dic_deck = new Dictionary<int, (PictureBox pic, Point loc_target,bool display,bool start_move)>
            {
                //การแก้ key จะส่งผลต่อเมธอดที่ใช้แจกไพ่นะ
                {1,tuple_dic_deck(deck_first,375,450,true,true)  },
                {2,tuple_dic_deck(deck_second,375, 150)  },
                {3,tuple_dic_deck(deck_third,450, 450)  },
                {4,tuple_dic_deck(deck_fourth,450, 150)  },
                {5,tuple_dic_deck(deck_fifth,525, 450,false)  }, //ยังไม่จั่วอะนะ
                {6,tuple_dic_deck(deck_sixth,525, 150,false)  },
            };

            timer.Start();
            /*int cards_per_player = 2, count_player =2; //การ์ดสองใบต่อคน , ผู้เล่นสองคน-รวมเจ้ามืออะนะ
            Point card_coordinates_start = deck.Location;

            for (int round = 0; round < cards_per_player; round++)
            {
                for (int i = 0; i < count_player; i++)
                {
                    deck_first
                }
            }*/


        }

        void setting_page_pokdengBasic()
        {
            //แสดงยอดเงินหลังหักเดิมพันเรียบร้อยแล้ว
            money_player_label.Text = player.display();
            tab.play_pokdeng_game();
            btn_draw_card.Hide();
            btn_not_draw_card.Hide();
        }


        void pokdeng_game()
        {
            //pokdeng = new PokDeng();

            setting_page_pokdengBasic();

            //สับไพ่แบบข้อมูล
            List<Cards> shuffleCards = cards_game.shuffle_cards(cards_data);
            //แจกไพ่แบบข้อมูล
            card_hands = cards_game.deal_cards(shuffleCards, 2);
            animation_deal_default(card_hands);

            /*PictureBox card1 = pictureBoxes["card1"];
            MessageBox.Show(card1.Name);*/

            //List<int> player_draw = new List<int> { 0, 1 }; //ใครเลือกจั่วบ้าง ใส่ index คนที่จั่ว ที่นี่เล่นสองคน มี 0,1 โดย 1 เป็นเจ้ามือ
            //cardCard_hands = cards_game.draw_additionalCard(cardCard_hands, player_draw, 1, shuffleCards);

            //อนิเมชันไพ่สองใบ - ผู้เล่น | เจ้ามือ
        }

        //กดปุ่มเริ่มเดิมพัน
        private void btn_start_bet_Click(object sender, EventArgs e)
        {
            //เช็กว่ายอดเงินพอจากที่อื่นแล้ว ตรงนี้เลยไม่ได้เช็กเพิ่มน่ะ
            player.deduct_bet(bet);
            pokdeng_game();
        }

        void select_bet2K()
        {
            double old_bet = bet;
            bet = 2000;
            if (!setting_select_betBasic(bet))
            {
                bet = old_bet;
                return;
            }

            pic.restore_size_chip();
            pic.resize_chip(bet_2K);
        }

        void select_bet5K()
        {
            double old_bet = bet;
            bet = 5000;
            if (!setting_select_betBasic(bet))
            {
                bet = old_bet;
                return;
            }

            pic.restore_size_chip();
            pic.resize_chip(bet_5K);
        }

        void select_bet10K()
        {
            double old_bet = bet;
            bet = 10000;
            if (!setting_select_betBasic(bet))
            {
                bet = old_bet;
                return;
            }

            pic.restore_size_chip();
            pic.resize_chip(bet_10K);
        }

        void select_bet50K()
        {
            double old_bet = bet; //เผื่อชิปใหม่ที่เลือก เงินไม่พอ จะให้ย้อนกลับไปเลือกชิปเก่าน่ะ
            bet = 50000;
            if (!setting_select_betBasic(bet))
            {
                bet = old_bet;
                return;
            }

            pic.restore_size_chip();
            pic.resize_chip(bet_50K);
        }

        private void btn_bet_all_in_Click(object sender, EventArgs e)
        {
            bet = player.get_money();
            if(!setting_select_betBasic(bet))
                return;
            pic.restore_size_chip();
        }

        bool setting_select_betBasic(double bet)
        {
            double money_current = player.get_money();
            if (money_current >= bet)
            {
                textbox_display_bet.Text = cal.display_money(bet);
                return true; //ผ่าน
            }

            MessageBox.Show("ยอดเงินปัจจุบันของคุณ คือ " + cal.display_money(money_current) + Environment.NewLine +
                "ซึ่งไม่เพียงพอสำหรับการเดิมพัน : " + cal.display_money(bet));
            return false; //เงินไม่พอ ไม่ให้เดิมพัน
        }

        private void bet_2K_Click(object sender, EventArgs e)
        {
            select_bet2K();
        }

        private void bet_2K_txt_Click(object sender, EventArgs e)
        {
            select_bet2K();
        }

        private void bet_5K_Click(object sender, EventArgs e)
        {
            select_bet5K();
        }

        private void bet_5K_txt_Click(object sender, EventArgs e)
        {
            select_bet5K();
        }

        private void bet_10K_Click(object sender, EventArgs e)
        {
            select_bet10K();
        }

        private void bet_10K_txt_Click(object sender, EventArgs e)
        {
            select_bet10K();
        }

        private void bet_50K_Click(object sender, EventArgs e)
        {
            select_bet50K();
        }

        private void bet_50K_txt_Click(object sender, EventArgs e)
        {
            select_bet50K();
        }

    }
}
