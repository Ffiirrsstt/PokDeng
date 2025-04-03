using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace project
{
    internal class Cards_Games_UI_deal
    {
        // สับไพ่
        Picture pic = new Picture();
        PokDeng pokdeng = new PokDeng();
        Cards_Games_UI ui = new Cards_Games_UI();


        // แจกไพ่ - ปุ่มมันมีขอบขาว ไม่สวย เลยใช้ label เอาน่ะ....
        public Dictionary<int, Picture_move> 
            setting_deal_default
            (Dictionary<int, Picture_move>  dic_deck, Timer timer, int card_number_change,bool startGame_deal,Label btn_draw_card, Label btn_not_draw_card)
        {
            //setting ให้เริ่มต้นแจกใบถัดไปอะแหละ
            if (card_number_change != 0)
            {
                dic_deck[card_number_change].start_move = true;
                return dic_deck;
            }
            return dic_deck;
        }

        //return เจอป็อกมั้ยอะแหละ(t-เจอ/f-ไม่) , ผล(แพ้-ชนะ-เสมอ) , ดึงข้อมูลต่าง ๆ ของการ์ดบนมือ (ฝั่งผู้เล่น , ฝั่งดีลเลอร์)
        public (bool,string,(PokDeng,PokDeng)) dealing_cards_each_player
            (Dictionary<int, Picture_move>  dic_deck, List<List<Cards>> card_hands, Timer timer, bool startGame_deal,
            Label draw_card, Label not_draw_card, Point loc_card_number_fourth, Point target_card_number_fourth)
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

                timer.Enabled = false; //หยุดจำเวลาชั่วคราว - จนกว่าจะ start ใหม่อะแหละนะ
                //ถ้าเจอป็อกแล้วคือเปิดไพ่อะแหละ ดังนั้นไปหาว่าใครชนะ
                if (check_pok)
                {
                    string result = pokdeng.win_lose_draw(result_player, result_dealer);
                    return (true,result,(result_player, result_dealer)); 
                }

                return (false, "", (result_player, result_dealer));
            }

            return (false, "", (new PokDeng(), new PokDeng()));
        }

        public void animation_deal
            (Form form,Player player, Dictionary<int, Picture_move>  dic_deck, List<List<Cards>> card_hands, Timer timer, int speed,bool startGame_deal, Label draw_card, Label not_draw_card,double bet,Label money_player_label)
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

                form.Invalidate();
            }

            dic_deck = setting_deal_default(dic_deck, timer, card_number_change, startGame_deal, draw_card, not_draw_card);

            /*//จะเช็กว่าแจกเริ่มต้นคนละสองใบเสร็จหรือยังอะแหละ
            //เช็กที่ใบที่สี่ เพราะทำทีละใบ ถ้าใบที่สี่เสร็จ แปลว่าแจกคนละสองใบเสร็จละ
            //มี startGame_deal ด้วย ให้รู้ว่าเป็น timer ที่ให้เริ่มทำเพราะอยากแจกคนละสองใบ ไม่ใช่ timer ที่ให้ทำเพื่อให้จั่วเพิ่ม(กรณีให้สิทธิ์จั่ว) - ไม่งั้นกลายเป็นหยุดจับเวลา ทั้ง ๆ ที่ไพ่ใบที่จั่วเพิ่มยังแจกไม่เสร็จ เพราะไปเช็กพบว่าไพ่ที่ต้องแจกคนละสองใบแจกเสร็จแล้ว เลยหยุดจับเวลาอะนะ*/
            Point loc_card_number_fourth = dic_deck[4].pic.Location;
            Point target_card_number_fourth = dic_deck[4].loc_target;

            var (check_pok, result, (result_hand_player, result_hand_dealer)) = dealing_cards_each_player
            (dic_deck, card_hands, timer, startGame_deal, draw_card, not_draw_card, loc_card_number_fourth, target_card_number_fourth);

            if (check_pok) ui.result_ui(player, card_hands, result, result_hand_player.times_pay, result_hand_dealer.times_pay,
            bet, money_player_label);
        }
    }
}
