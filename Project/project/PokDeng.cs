using System.Collections.Generic;
using System.Windows.Forms;

namespace project
{
    internal class PokDeng
    {
        public int points_cards { get; set; }
        public int times_pay { get; set; }
        public bool special_hands { get; set; }
        public string special_hands_type { get; set; }
        public int hierarchy { get; set; }

        public (bool, (PokDeng, PokDeng)) check_pok_All(Dictionary<int, Picture_move> dic_deck, List<List<Cards>> hands, Label draw, Label not_draw)
        {
            PokDeng_OpenCard decide = new PokDeng_OpenCard();
            return decide.decide_reveal_hide(dic_deck,hands, draw, not_draw);
        }

        //ไว้รวมให้เรียกง่าย ๆ
        //เมธอดเขียนแยก ๆ ไปเพื่อให้จำนวนบรรทัดไม่มากจนเกินไปน่ะ - แล้วมาใช้ PokDeng เป็นตัวกลางเรียกแต่ละอันมาทำอีกที
        public PokDeng much_cards_hand(List<Cards> cards_hand)
        {
            PokDeng_muchHand calculate = new PokDeng_muchHand();
            return calculate.much_cards_hand(cards_hand);
        }

        public string win_lose_draw(PokDeng hand_user, PokDeng hand_dealer)
        {
            PokDeng_Services_winLoseDraw win = new PokDeng_Services_winLoseDraw();
            return win.win_lose_draw( hand_user, hand_dealer);
        }

        public string check_higher_value(int user, int dealer, List<string> result)
        {
            PokDeng_Services_winLoseDraw win = new PokDeng_Services_winLoseDraw();
            return win.check_higher_value( user, dealer, result);
        }




    }
}
