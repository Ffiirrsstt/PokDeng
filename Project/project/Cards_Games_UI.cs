using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace project
{
    internal class Cards_Games_UI
    {

        Calculate cal = new Calculate();
        dictionary_PokDeng dic = new dictionary_PokDeng();

        public void displayTXT_display_bet_start(Label display_label, double bet) => display_label.Text = "เงินเดิมพันเริ่มต้น : $ " + cal.display_money(bet);

        //ประกาศผลชนะ แพ้ เสมอ
        public void result_ui
           (Player player, List<List<Cards>> hands, string result, int player_times_pay, int dealer_times_pay,
           double bet, Label money_player_label)
        {
            List<string> result_list = dic.result_game; //ออกแบบมุมผู้เล่น จะยึดฝั่งผู้เล่นว่าแพ้ ชนะ เสมอ
            string txt = "";

            if (result == result_list[3]) MessageBox.Show("ขออภัย ระบบขัดข้อง");

            if (result == result_list[0]) //ชนะ
            {
                int times_pay = player_times_pay;
                txt = "ได้รับ : " + times_pay + " เท่า";
                player.money_in(times_pay, bet);
            }
            else if (result == result_list[1])
            {
                //แพ้
                int times_pay = dealer_times_pay;
                txt = "จ่าย : " + times_pay + " เท่า";
                player.money_out(times_pay, bet);
            }
            else if (result == result_list[2])
            {
                txt = "จ่าย : " + "ได้รับเงินคืน";
                player.refund_bet(bet);
            }

            money_player_label.Text = player.display();
            MessageBox.Show("ผล : " + result + Environment.NewLine + txt);
        }

    }
}
