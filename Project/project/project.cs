using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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
        PokDeng pokdeng;
        List<List<Cards>> card_hands;
        List<Cards> cards_data ,shuffleCards; //ข้อมูลการ์ดทั้งหมด , ข้อมูลการ์ดที่สับเรียบร้อยแล้ว
        (string result,PokDeng result_player, PokDeng result_dealer) result_hand;
        Bet bet_services = new Bet();
        Dictionary<int, Picture_move> dic_deck;
        Calculate cal = new Calculate();
        Cards_Games cards_game = new Cards_Games();
        Cards_Games_UI_deal cards_ui_deal = new Cards_Games_UI_deal();
        Deal_Card_5thAND6th deal_5th6th = new Deal_Card_5thAND6th();
        Cards_Games_UI cards_ui = new Cards_Games_UI();
        Picture_move pic_move = new Picture_move();

        int speed = 15; //ความไวไพ่ (ความไวเคลื่อนที่ของไพ่อะแหละจ้ะ)
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
            trackBar_bet.Minimum = 1000;   // ค่าน้อยสุด
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
            timer.Enabled = false;
            tab.new_pokdeng_game();

            select_page_newgame_pokdeng();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == page_newgame_pokdeng) select_page_newgame_pokdeng();
        }

        void select_page_newgame_pokdeng()
        {
            int betK = 2000; //เงินเดิมพันเริ่มต้น
            //แสดงเงินปัจจุบัน
            money_player_waitBet.Text = player.display();
            //ตั้งค่า default ว่าเลือก chip 2k
            bet = bet_services.select_betK(player, pic, bet_2K, betK, betK, textbox_display_bet);
            pic.restore_size_chip(); //จัดการชิปให้เข้าที่และขนาดเท่ากัน

            trackBar_bet.Maximum = (int)player.get_money();
            trackBar_bet.Value = 2000;
        }






        private void ST111_Load(object sender, EventArgs e)
        {
            fetch_data_cards();

            tab.hide_start_program();

            cards_ui.displayTXT_display_bet_start(display_bet_start,bet_default);
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
            cards_ui.displayTXT_display_bet_start(display_bet_start,bet_start);
        }

        bool startGame_player_deal = false, startGame_dealer_deal = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            if(startGame_deal)
                (startGame_deal,result_hand) = cards_ui_deal.animation_deal_default(this, player,  dic_deck, card_hands, timer, speed, startGame_deal, btn_draw_card, btn_not_draw_card, bet, money_player_label, display_result,loader_new_game,tab);
            if (startGame_player_deal || startGame_dealer_deal)
            {
                int point_dealer = result_hand.result_dealer.points_cards;
                List<Cards> deck = shuffleCards; //แค่ตั้งใหม่ไม่ให้งงน่ะ
                (startGame_player_deal, startGame_dealer_deal, dic_deck, card_hands) = deal_5th6th.animation_deal_draw(this, player, dic_deck, card_hands, timer, speed, startGame_player_deal, startGame_dealer_deal, btn_draw_card, btn_not_draw_card, bet, money_player_label, point_dealer, deck, display_result,loader_new_game,tab);
            }
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

        //ป็อกเด้งเริ่มต้นแจกสองอะสิ
        void animation_deal_default()
        {
            startGame_deal = true;

            pic_move.dic_To_back_deck(dic_deck, deck.Location);

            timer.Enabled = true;
        }

        void setting_page_pokdengBasic()
        {
            //แสดงยอดเงินหลังหักเดิมพันเรียบร้อยแล้ว
            money_player_label.Text = player.display();
            tab.play_pokdeng_game();
            hide_btn_draw();
        }

        void pokdeng_game()
        {
            display_result.Text = "";
            loader_new_game.Visible = false; //ซ่อน

            setting_page_pokdengBasic();

            //สับไพ่แบบข้อมูล
            shuffleCards = cards_game.shuffle_cards(cards_data);
            //แจกไพ่แบบข้อมูล
            card_hands = cards_game.deal_cards(shuffleCards, 2);
            dic_deck = dic_data_deck(); //ข้อมูลเกี่ยวกับอนิเมชันการ์ดน่ะ
            cards_ui.all_flip_card_down(dic_deck, deck); //ปิดการ์ดทั้งหมดก่อน เพราะเริ่มตาใหม่
            animation_deal_default(); //แจกไพ่แบบอนิเมชัน - ทำงานร่วมกับ timer
        }


        private async void btn_draw_card_Click(object sender, EventArgs e)
        {
            hide_btn_draw();
            startGame_player_deal = true;   
            List<Cards> deck = shuffleCards; 
            //ผู้เล่นเลือกจั่ว หลังจากนั้นดีลเลอร์จะตัดสินใจภายในเมธอดที่ถูกเรียกใช้โดย timer (timer เพราะตัดสินใจหลังแจกผู้เล่นเสร็จ) - ผลแพ้ชนะถูกเรียกใช้ใน timer เพราะรอแจกการ์ดเสร็จน่ะ
            (dic_deck, card_hands) = deal_5th6th.player_animation( card_hands, dic_deck,deck, timer);
        }

        private void btn_not_draw_card_Click(object sender, EventArgs e)
        {
            hide_btn_draw();

            int point_dealer = result_hand.result_dealer.points_cards;
            bool result_decide = deal_5th6th.dealer_decide(point_dealer);
            
            //ผู้เล่นไม่จั่ว งั้นดีลเลอร์ตัดสินใจว่าจะจั่วไหม เสร็จแล้วคำนวณผลแพ้-ชนะ - ผลแพ้ชนะถูกเรียกใช้ใน timer เพราะรอแจกการ์ดเสร็จน่ะ
            if (result_decide)
            {
                List<Cards> deck = shuffleCards;

                startGame_dealer_deal = true;
                (dic_deck, card_hands) = deal_5th6th.dealer_decide_animation(card_hands, dic_deck, deck, timer);
                return;
            }

            int times_pay_palyer = result_hand.result_player.times_pay;
            int times_pay_dealer = result_hand.result_player.times_pay;

            pic.show_card(dic_deck, card_hands, 2, 1); //เปิดการ์ดทั้งหมดของดีลเลอร์

            //กรณีตัดสินใจดีลเลอร์ไม่จั่ว เนื่องจากไม่ได้จั่วทั้งผู้เล่นและดีลเลอร์ จึงใช้ผลเดิม - result_hand (ถูกคำนวณเอาไว้แล้ว แต่แค่ถ้าไม่ pok เลยยังไม่ได้แสดงเฉย ๆ)
            cards_ui.result_ui(player,card_hands, result_hand.result, times_pay_palyer, times_pay_dealer, bet,money_player_label , display_result, loader_new_game, tab);
        }

        void hide_btn_draw()
        {
            btn_draw_card.Hide();
            btn_not_draw_card.Hide();
        }

        private void trackBar_bet_ValueChanged(object sender, EventArgs e) =>
            bet = bet_services.select_betK(player, pic, null, bet, trackBar_bet.Value, textbox_display_bet,false); //เลือก false คือไม่ต้องให้มีการตอบสนอง เช่น เปลี่ยนขนาดชิปไรงี้

        //กดปุ่มเริ่มเดิมพัน
        private void btn_start_bet_Click(object sender, EventArgs e)
        {
            //เช็กว่ายอดเงินพอจากที่อื่นแล้ว ตรงนี้เลยไม่ได้เช็กเพิ่มน่ะ
            player.deduct_bet(bet);
            pokdeng_game();
        }

        //- ตัวแรกเงินเดิมพันเก่าว่าเคยเดิมพันไรไว้ | ตัวสองเงินที่อยากเดิมพัน
        void select_bet2K(object sender, EventArgs e) =>
            bet = bet_services.select_betK(player, pic, bet_2K, bet, 2000, textbox_display_bet);

        void select_bet5K(object sender, EventArgs e) =>
            bet =bet_services.select_betK(player, pic, bet_5K, bet, 5000, textbox_display_bet);

        void select_bet10K(object sender, EventArgs e) =>
            bet = bet_services.select_betK(player, pic, bet_10K, bet, 10000, textbox_display_bet);

        void select_bet50K(object sender, EventArgs e) =>
            bet = bet_services.select_betK(player, pic,bet_50K, bet, 50000, textbox_display_bet);

        private void btn_bet_all_in_Click(object sender, EventArgs e) =>
            bet = bet_services.select_betK(player, pic, null, bet, player.get_money(), textbox_display_bet,false);

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
