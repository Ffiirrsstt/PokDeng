using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace project
{
    internal class PokDeng
    {
        public int points_cards { get; set; }
        public int times_pay { get; set; }
        public bool special_hands { get; set; }
        public string special_hands_type { get; set; }
        public int hierarchy { get; set; }

        int dic_hands_type(string special_hands_type)
        {
            dictionary_PokDeng dic = new dictionary_PokDeng();
            return dic.dic_special_hands_type[special_hands_type];
        }

        //ไว้รวมให้เรียกง่าย ๆ
        //เมธอดเขียนแยก ๆ ไปเพื่อให้จำนวนบรรทัดไม่มากจนเกินไปน่ะ - แล้วมาใช้ PokDeng เป็นตัวกลางเรียกแต่ละอันมาทำอีกที
        public PokDeng much_cards_hand(List<Cards> cards_hand)
        {
            PokDeng_muchHand calculate = new PokDeng_muchHand();
            return calculate.much_cards_hand(cards_hand);
        }

        //เนื่องจาก pokdeng ตัดสินแพ้ชนะหรือเสมอกันที่ระหว่างเจ้ามือกับผู้เล่น แบบ 1:1 เท่านั้น
        public string win_lose_draw(PokDeng hand_user, PokDeng hand_dealer)
        {
            //สร้าง list ให้แก้ง่าย ๆ น่ะ เวลาไม่อยากใช้คำว่า win lose draw
            dictionary_PokDeng dic = new dictionary_PokDeng();
            List<string> result = dic.result_game; //ออกแบบมุมผู้เล่น จะยึดฝั่งผู้เล่นว่าแพ้ ชนะ เสมอ

            //เช็กว่าเป็นไพ่พิเศษไหม - ตอง > เซียน > เรียง> แต้ม หรือก็คือ ไพ่พิเศษ > ไพ่ไม่พิเศษ
            if (hand_user.special_hands && !hand_dealer.special_hands)
                return result[0];

            if (!hand_user.special_hands && hand_dealer.special_hands)
                return result[1];

            if(!hand_user.special_hands && !hand_dealer.special_hands)
                return check_higher_value(hand_user.points_cards, hand_dealer.points_cards, result);

            if (hand_user.special_hands && hand_dealer.special_hands)
            {
                // เพราะออกแบบเป็นตัวอักษรไว้น่ะ เลยมาแปลงเป็นตัวเลขอีกที เพื่อจะได้เอาไปคำนวณง่าย ๆ 
                int user_hands_type = dic_hands_type(hand_user.special_hands_type);
                int dealer_hands_type = dic_hands_type(hand_dealer.special_hands_type);

                //ถ้าเท่ากัน เช่น เซียนเหมือนกัน ให้เทียบที่ศักดิ์ hierarchy
                string decide = check_higher_value(user_hands_type, dealer_hands_type, result);
                if (decide == result[0] || decide == result[1]) return decide;

                //สนศักดิ์คิง ควีนไรงี้ แต่เล่นแบบไม่สนดอก
                return check_higher_value(hand_user.hierarchy, hand_dealer.hierarchy, result);
            }

            return result[3];
        }

        //เช็กมากกว่า น้อยกว่า เท่ากับ - ชนะ แพ้ เสมอน่ะ
        string check_higher_value(int user,int dealer,List<string> result)
        {
            if (user > dealer)
                return result[0];

            if (user < dealer)
                return result[1];

            //ไม่มากกว่า ไม่น้อยกว่า ดังนั้นเหลือแค่เท่ากับ - if(user == dealer)
            return result[2];
        }
    }
}
