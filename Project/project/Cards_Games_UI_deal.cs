using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace project
{
    internal class Cards_Games_UI_deal
    {
        // สับไพ่
        Picture pic = new Picture();
        PokDeng pokdeng = new PokDeng();
        dictionary_PokDeng dic = new dictionary_PokDeng();

        // แจกไพ่ - ปุ่มมันมีขอบขาว ไม่สวย เลยใช้ label เอาน่ะ....
        public Dictionary<int, (PictureBox pic, Point loc_target, bool display, bool start_move)> setting_deal_default
            (Dictionary<int, (PictureBox pic, Point loc_target, bool display, bool start_move)>  dic_deck, Timer timer, int card_number_change,bool startGame_deal,Label btn_draw_card, Label btn_not_draw_card,RichTextBox richTextBox1)
        {
            //setting ให้เริ่มต้นแจกใบถัดไปอะแหละ
            if (card_number_change != 0)
            {
                dic_deck[card_number_change] = (dic_deck[card_number_change].pic, dic_deck[card_number_change].loc_target, dic_deck[card_number_change].display, true);
                return dic_deck;
            }
            return dic_deck;
        }

        public void dealing_cards_each_player
            (Player player, Dictionary<int, (PictureBox pic, Point loc_target, bool display, bool start_move)>  dic_deck, List<List<Cards>> card_hands, Timer timer, bool startGame_deal,
            Label draw_card, Label not_draw_card, double bet_current, Label money_player_label, Point loc_card_number_fourth, Point target_card_number_fourth)
        {
            if (startGame_deal && loc_card_number_fourth.X <= target_card_number_fourth.X && loc_card_number_fourth.Y >= target_card_number_fourth.Y)
            {
                PictureBox pic_player_num1 = dic_deck[1].pic;
                PictureBox pic_player_num2 = dic_deck[3].pic; //dic_deck[3] เพราะแจกสลับ ดังนั้นใบที่สองเป็นของดีลเลอร์ ซึ่งไม่แสดงไพ่ดีลเลอร์จนกว่าจะถึงเวลาเปิดไพ่

                //true(เปิดไพ่ - เจอป็อก) | false(ปิดไพ่ - ยังไม่มีใครป็อก) สั้น ๆ เช็กว่าผู้เล่นและดีลเลอร์ป็อก (ผลรวม 8/9 ไหม)
                var (check_pok, (result_player, result_dealer)) = pokdeng.check_pok_All(dic_deck, card_hands, draw_card, not_draw_card);

                pic_player_num1.Image = Image.FromStream(new MemoryStream(card_hands[0][0].picture)); //นอกจากหยุดจับเวลาก็มาแสดงไพ่ให้ผู้เล่นเห็นด้วยว่าแจกได้ไร
                //card_hands[0][2].picture - คนที่ 0 คือ ผู้เล่น | ถ้าคนที่ 1 คือดีลเลอร์ และ 1 คือ ไพ่ใบที่สองอะนะ
                pic_player_num2.Image = Image.FromStream(new MemoryStream(card_hands[0][1].picture));

                pic.setting_pic_list_Stretch(new List<PictureBox> { pic_player_num1, pic_player_num2 });

                //ถ้าเจอป็อกแล้วคือเปิดไพ่อะแหละ ดังนั้นไปหาว่าใครชนะ
                if (check_pok)
                    check_player_win(player, card_hands, result_player, result_dealer, bet_current, money_player_label);


                timer.Stop(); //หยุดจำเวลาชั่วคราว - จนกว่าจะ start ใหม่อะแหละนะ
            }
        }

        void check_player_win(Player player , List<List<Cards>> hands,PokDeng hand_player, PokDeng hand_dealer, double bet,Label money_player_label)
        {
            List<string> result_list = dic.result_game; //ออกแบบมุมผู้เล่น จะยึดฝั่งผู้เล่นว่าแพ้ ชนะ เสมอ
            string result = pokdeng.win_lose_draw(hand_player, hand_dealer);

            if (result == result_list[3]) MessageBox.Show("ขออภัย ระบบขัดข้อง");

            string txt = "";

            if (result == result_list[0]) //ชนะ
            {
                int times_pay = hand_player.times_pay;
                txt = "ได้รับ : " + times_pay + " เท่า";
                player.money_in(times_pay, bet);
            }
            else if (result == result_list[1])
            {
                //แพ้
                int times_pay = hand_dealer.times_pay;
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
