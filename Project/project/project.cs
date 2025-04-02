using System;
using System.Collections.Generic;
using System.Drawing;
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
        Cards cards;

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
            List<Cards> cards_data = await cards.fetchDataCards();
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

        void pokdeng_game()
        {
            //แสดงยอดเงินหลังหักเดิมพันเรียบร้อยแล้ว
            money_player_label.Text= player.display();
            tab.play_pokdeng_game();

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
