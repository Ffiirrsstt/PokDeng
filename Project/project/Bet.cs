using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    internal class Bet
    {
        Calculate cal = new Calculate();

        bool setting_select_betBasic(Player player, double bet,TextBox textbox_display_bet)
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

        public void select_betK(Player player, Picture pic,PictureBox picturebox, double bet, double betK ,TextBox textbox_display_bet,bool allin=false)
        {
            double old_bet = bet; //เผื่อชิปใหม่ที่เลือก เงินไม่พอ จะให้ย้อนกลับไปเลือกชิปเก่าน่ะ
            bet = betK;
            if (!setting_select_betBasic(player, bet,  textbox_display_bet))
            {
                bet = old_bet;
                return;
            }

            pic.restore_size_chip();
            if(!allin)pic.resize_chip(picturebox);
        }
    }
}
