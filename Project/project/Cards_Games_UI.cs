using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace project
{
    internal class Cards_Games_UI
    {

        Calculate cal = new Calculate();
        dictionary_PokDeng dic = new dictionary_PokDeng();
        PokDeng pokdeng = new PokDeng();
        int delay_loading_newGame = 50;

        public void displayTXT_display_bet_start(Label display_label, double bet) => display_label.Text = "เงินเดิมพันเริ่มต้น : $ " + cal.display_money(bet);

        //คว่ำการ์ด
        public void all_flip_card_down(Dictionary<int, Picture_move> dic_deck, PictureBox deck)
        {
            foreach(var card_deck in dic_deck)
            {
                PictureBox pic = card_deck.Value.pic;
                pic.Image = deck.BackgroundImage;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        //คำนวณและแสดงผลลัพธ์
        public void cal_display_resultUI(Player player,List<List<Cards>>card_hands,double bet,Label money_player_label, Label display,ProgressBar loader,Tab tab)
        {
            List<Cards> hand_player = card_hands[0];
            List<Cards> hand_dealer = card_hands[1];
            PokDeng result_player = pokdeng.much_cards_hand(hand_player);
            PokDeng result_dealer = pokdeng.much_cards_hand(hand_dealer);

            string result = pokdeng.win_lose_draw(result_player, result_dealer);

            result_ui(player, card_hands, result, result_player.times_pay, result_dealer.times_pay, bet, money_player_label , display,loader,tab);
        }

        //ประกาศผลชนะ แพ้ เสมอ
        public async Task result_ui
           (Player player, List<List<Cards>> hands, string result, int player_times_pay, int dealer_times_pay,
           double bet, Label money_player_label,Label display,ProgressBar progress,Tab tab)
        {
            await Task.Delay(500); //อยากให้ดีเลย์สักนิด ไม่ให้ข้อความขึ้นไวไป
            display.Text = text_display_ui(player, result, player_times_pay, dealer_times_pay, bet);
            money_player_label.Text = player.display();

            await loading(progress);

            double money = player.get_money();
            if(money<1000) //เพราะเดิมพันขั้นต่ำที่หนึ่งพัน
            {
                MessageBox.Show("ยอดเงินคงเหลือของคุณน้อยเกินไป กรุณาเริ่มเล่นเกมใหม่");
                tab.hide_start_program();
                return;
            }
            tab.new_pokdeng_game();
        }

        async Task loading(ProgressBar progress)
        {
            progress.Value = 0;
            progress.Visible = true; //แสดง
            for (int i = 1; i <= 100; i++)
            {
                //1000 รอ 1 วินาที
                await Task.Delay(delay_loading_newGame);
                progress.Value = i;
            }
        }

        string text_display_ui(Player player,string result, int player_times_pay, int dealer_times_pay,double bet)
        {
            List<string> result_list = dic.result_game; //ออกแบบมุมผู้เล่น จะยึดฝั่งผู้เล่นว่าแพ้ ชนะ เสมอ

            if (result == result_list[3]) return "ขออภัย ระบบขัดข้อง";

            if (result == result_list[0]) //ชนะ
            {
                int times_pay = player_times_pay;
                player.money_in(times_pay, bet);
                return "WIN รับเงิน " + times_pay + " เท่า!";
            }

            if (result == result_list[1])
            {
                //แพ้
                int times_pay = dealer_times_pay;
                player.money_out(times_pay, bet);
                return "LOSE จ่าย " + times_pay + " เท่า!";
            }

            if (result == result_list[2])
            {
                player.refund_bet(bet);
                return "DRAW ได้รับเงินคืน!";
            }

            return "ขออภัย ระบบขัดข้อง";
        }

    }
}
