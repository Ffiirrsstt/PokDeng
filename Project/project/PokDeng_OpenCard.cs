using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace project
{
    internal class PokDeng_OpenCard
    {
        //ไม่เรียกผ่าน Class PokDeng เพราะเด่วจะเอา check_pok ไปรวมใส่ ถ้าเรียก PokDeng ที่นี่จะมีปัญหาพึ่งพาวงกลมอะสิ
        PokDeng_muchHand pokDeng_MuchHand = new PokDeng_muchHand(); 
        Picture picture = new Picture();

        //ถ้าป็อก 8 / 9 บังคับเปิด
        (bool, PokDeng) check_pok(List<Cards> hand) //return เจอป็อกมั้ย และ result [มันใช้ดึงข้อมูล point , จำนวนเท่าการจ่ายไรงี้ได้น่ะ]
        {
            PokDeng result_hand = pokDeng_MuchHand.much_cards_hand(hand);
            int point = result_hand.points_cards;
            if (point>=8) return (true, result_hand);

            return (false, result_hand);

        }

        //return ต้องเปิดไพ่ไหม หรือยังไปว่าจะจั่วไหม - true(เปิด) | false(ปิด) และ แต้มของผู้เล่นกับดีลเลอร์
        public (bool, (PokDeng,PokDeng)) decide_reveal_hide(Dictionary<int, (PictureBox pic, Point loc_target, bool display, bool start_move)> dic_deck, List<List<Cards>> hands, Label draw, Label not_draw)
        {
            List<Cards> hand_player = hands[0]; //แค่ตั้งให้สื่อน่ะ
            List<Cards> hand_dealer = hands[1]; //แค่ตั้งให้สื่อน่ะ
            var (pok_player, result_hand_player) = check_pok(hand_player);
            var (pok_dealer, result_hand_dealer) = check_pok(hand_dealer);
            if (!pok_player && !pok_dealer)
            {
                draw.Visible = true;
                not_draw.Visible = true;
                return (false, (result_hand_player, result_hand_dealer));
            }

            int count_hand_dealer = hand_dealer.Count; //เช่น 2ใบ / 3ใบ บนมือของดีลเลอร์

            //เจอป็อก เปิดไพ่ - ผู้เล่นเปิดละ งั้นเปิดของเจ้ามือเพิ่ม (2-4-6 ต้องดูอีกทีอะนะ ซึ่งเป็นเลขคู่)
            foreach (var card in dic_deck)
            {
                var (pic, loc_target, display, start_move) = card.Value;
                int key = card.Key;
                if (key % 2 == 0) //จริง ๆ เขียนเงื่อนไข ==2 || ==4 ก็ได้แหละ...
                {
                    //key 2 ต้องดึงจาก idx 0 | 4 ดึงจาก 1 | 6 ดึงจาก 2 เขียนเป็นสมการ คือ output = (input/2) -1 ดังนั้น (key/2)-1
                    int output = (key / 2) - 1;
                    pic.Image = Image.FromStream(new MemoryStream(hand_dealer[output].picture));
                    picture.setting_pic_Stretch(pic);
                }

                if (count_hand_dealer == 2 && key == 4)
                    break; //ออก loop - ไว้จุดนี้ เพื่อให้มันแสดงรูปของใบที่สองไปก่อน
            }

            return (true, (result_hand_player, result_hand_dealer));
        }

    }
}
