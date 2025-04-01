using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        //ไว้รวมให้เรียกง่าย ๆ
        //เมธอดเขียนแยก ๆ ไปเพื่อให้จำนวนบรรทัดไม่มากจนเกินไปน่ะ - แล้วมาใช้ PokDeng เป็นตัวกลางเรียกแต่ละอันมาทำอีกที
        public PokDeng much_cards_hand(List<Cards> cards_hand)
        {
            PokDeng_muchHand calculate = new PokDeng_muchHand();
            return calculate.much_cards_hand(cards_hand);
        }

        public void win_lose_draw()
        {

        }
    }
}
