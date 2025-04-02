using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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

        List<TabPage> tab_list;
        Tab tab;
        Player player;
        Calculate cal = new Calculate();

        int bet_default = 100000;

        int page_1 = 0, page_2 = 1, page_3 = 3;

        public ST111()
        {
            InitializeComponent();
            tab_list = new List<TabPage> { page_main, page_newgame_pokdeng, page_play_pokdeng };
            //ออกแบบ dic ให้แก้ง่ายหน่อย
            tab = new Tab(tabControl, dic_tab());

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

        /*
         อยากลืมทำยอดติดลบ ถ้าติดลบเชิญออก*/


        async Task fetch_data_cards()
        {
            Cards cards = new Cards();
            List<Cards> cards_data = await cards.fetchDataCards();
        }
        void game_intro()
        {
            //await Task.Delay(5000);
            //this.BackgroundImage = Image.FromFile("D:\\USE\\img\\project363\\Intro.png");

        }

        void displayTXT_display_bet_start(double bet) => display_bet_start.Text = "เงินเดิมพันเริ่มต้น : $ " + cal.display_money(bet);

        private void textbox_bet_start_TextChanged(object sender, EventArgs e)
        {
            double bet_start = cal.string_ToDouble(textbox_bet_start.Text,false);
            displayTXT_display_bet_start(bet_start);
        }

        private void ST111_Load(object sender, EventArgs e)
        {
            fetch_data_cards();
            tab.hide_start_program();
            displayTXT_display_bet_start(bet_default);
            textbox_bet_start.Text = bet_default.ToString();
        }

        private void btn_new_pokdeng_game_Click(object sender, EventArgs e)
        {
            double bet_start =  cal.string_ToDouble(textbox_bet_start.Text);
            //แปลว่ามีข้อผิดพลาดเกิดขึ้น - อย่าพึ่งเข้าหน้าเปิดเกม
            if(bet_start==-1) return;

            player = new Player(bet_start); //เริ่มใหม่ทุกครั้งที่กดเริ่มเกมใหม่น่ะ
            money_player_waitBet.Text = player.display();
            tab.new_pokdeng_game(page_2);
        }
    }
}
