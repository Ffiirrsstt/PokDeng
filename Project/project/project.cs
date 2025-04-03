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
        List<TabPage> tab_list;
        Tab tab;
        Picture pic;
        Player player;
        Cards cards;
        List<Cards> cards_data;
        PokDeng pokdeng;
        List<List<Cards>> card_hands;
        Bet bet_services = new Bet();
        Dictionary<int, Picture_move> dic_deck;
        Calculate cal = new Calculate();
        Cards_Games cards_game = new Cards_Games();
        Cards_Games_UI_deal cards_ui_deal = new Cards_Games_UI_deal();
        Cards_Games_UI cards_ui = new Cards_Games_UI();
        Picture_move pic_move = new Picture_move();

        int speed = 5; //ความไวไพ่ (ความไวเคลื่อนที่ของไพ่อะแหละจ้ะ)
        int bet_default = 100000; //วงเงินเดิมพันเริ่มต้นของผู้เล่น - ก็คือแจกเงินตอนแรกอะแหละ
        double bet=2000; //เงินที่เดิมพันในแต่ละตา 

        bool startGame_deal = false; //เอาไว้เช็กเพื่อจะหยุด timer น่ะ ไม่ให้มันจับตลอดเวลา
        int page_1 = 0, page_2 = 1, page_3 = 3;
        int pic_1 = 0, pic_2 = 1,pic_3= 2,pic_4 = 3;

        public ST111()
        {
            InitializeComponent();

            this.TransparencyKey = Color.Empty;

            tab_list = new List<TabPage> { page_main, page_newgame_pokdeng, page_play_pokdeng };
            //ออกแบบ dic ให้แก้ง่ายหน่อย
            tab = new Tab(tabControl, dic_tab());
            pic = new Picture(dic_pic());
            handler_bet_chip();
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

        void new_game_pokdeng()
        {
            int betK = 2000; //เงินเดิมพันเริ่มต้น
            //แสดงเงินปัจจุบัน
            money_player_waitBet.Text = player.display();
            //ตั้งค่า default ว่าเลือก chip 2k
            bet_services.select_betK(player, pic, bet_2K, betK, betK, textbox_display_bet);

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

            cards_ui.displayTXT_display_bet_start(display_bet_start,bet_default);
            textbox_bet_start.Text = bet_default.ToString();
        }

        private void btn_new_pokdeng_game_Click(object sender, EventArgs e)
        {
            double bet_start = cal.string_ToDouble(textbox_bet_start.Text);
            //แปลว่ามีข้อผิดพลาดเกิดขึ้น - อย่าพึ่งเข้าหน้าเปิดเกม
            if (bet_start == -1) return;

            timer.Enabled = false;

            player = new Player(bet_start); //เริ่มใหม่ทุกครั้งที่กดเริ่มเกมใหม่น่ะ
            new_game_pokdeng();
        }
        private void textbox_bet_start_TextChanged(object sender, EventArgs e)
        {
            double bet_start = cal.string_ToDouble(textbox_bet_start.Text, false);
            cards_ui.displayTXT_display_bet_start(display_bet_start,bet_start);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            cards_ui_deal.animation_deal(this, player,  dic_deck, card_hands, timer, speed, startGame_deal, btn_draw_card, btn_not_draw_card, bet, money_player_label);
        }

        Dictionary<int, Picture_move> dic_data_deck()
        {
            return new Dictionary<int, Picture_move>
            {
                //การแก้ key จะส่งผลต่อเมธอดที่ใช้แจกไพ่นะ - หมายเลขจุดเริ่มต้นจำเป็นนะ ไว้ใช้ตอนเริ่มเกมใหม่อะ
                {1,pic_move.tuple_dic_deck(deck_first, 375,450,true,true)  },
                {2,pic_move.tuple_dic_deck(deck_second, 375, 150)  },
                {3,pic_move.tuple_dic_deck(deck_third,450, 450)  },
                {4,pic_move.tuple_dic_deck(deck_fourth, 450, 150)  },
                {5,pic_move.tuple_dic_deck(deck_fifth, 525, 450,false)  }, //ยังไม่จั่วอะนะ
                {6,pic_move.tuple_dic_deck(deck_sixth, 525, 150,false)  },
            };
        }
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

            dic_deck = dic_data_deck();
            pic_move.dic_To_back_deck(dic_deck, deck.Location);

            timer.Enabled = true;
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
            animation_deal_default(card_hands); //แจกไพ่แบบอนิเมชัน

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

        void select_bet2K(object sender, EventArgs e)
        {
            int betK = 2000;
            bet_services.select_betK(player, pic, bet_2K, bet, betK, textbox_display_bet);
        }

        void select_bet5K(object sender, EventArgs e)
        {
            int betK = 5000;
            bet_services.select_betK(player, pic, bet_5K, bet, betK, textbox_display_bet);
        }

        void select_bet10K(object sender, EventArgs e)
        {
            int betK = 10000;
            bet_services.select_betK(player, pic, bet_10K, bet, betK, textbox_display_bet);
        }

        void select_bet50K(object sender, EventArgs e)
        {
            int betK = 50000;
            bet_services.select_betK(player, pic,bet_50K, bet, betK, textbox_display_bet);

        }

        private void btn_bet_all_in_Click(object sender, EventArgs e)
        {
            bet = player.get_money();
            //ใช้ bet, bet เพราะเดิมพันเงินทั้งหมด ไม่มีคำว่าเงินไม่พอ มีแต่จะได้หมดหรือเสียหมดอะแหละ - ตัวแรกเงินเดิมพันเก่าว่าเคยเดิมพันไรไว้ | ตัวสองเงินที่อยากเดิมพัน
            bet_services.select_betK(player, pic, null, bet, bet, textbox_display_bet);

        }

        void handler_bet_chip() {
            bet_2K.Click += select_bet2K;
            bet_2K_txt.Click += select_bet2K;
            bet_5K.Click += select_bet5K;
            bet_5K_txt.Click += select_bet5K;
            bet_10K.Click += select_bet10K;
            bet_10K_txt.Click += select_bet10K;
            bet_50K.Click += select_bet50K;
            bet_50K_txt.Click += select_bet50K;
        }


    }
}
