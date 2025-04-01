using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class Cards_Games_Services
    {
        //drawn_cardsPerPlayer , count_player| รวมเจ้ามือ
        public List<List<Cards>> cardData_to_list(List<List<Cards>> hands, List<Cards> deck, int cardIdx, int drawn_cardsPerPlayer, int count_player, List<int> idx_playerWants_draw = null)
        {
            //ถ้าไม่ระบุ idx_playerWants_draw ถือว่าเป็น default จั่วทุกคน (เป็นเมธอดใช้ร่วมกับแจกไพ่เริ่มต้นและจั่วไพ่เพิ่ม)
            bool everyone_draw = true;
            //ระบุไว้ว่าใครอยากจั่วบ้าง (มีระบุ idx_playerWants_draw) ดังนั้นแปลว่าไม่ได้จั่วทุกคน
            if (idx_playerWants_draw != null) everyone_draw = false;


            for (int round = 0; round < drawn_cardsPerPlayer; round++)
            {
                for (int i = 0; i < count_player; i++)
                {
                    //ไม่
                    if (!everyone_draw)
                    {
                        //มี i ใน idx_playerWants_draw ไหม - เป็นคนที่อยากจั่วมั้ย
                        if (idx_playerWants_draw.Contains(i))
                        {
                            hands[i].Add(deck[cardIdx]);
                            cardIdx++;
                        }
                    }
                    else
                    {
                        hands[i].Add(deck[cardIdx]);
                        cardIdx++;
                    }
                }
            }

            return hands;
        }
    }
}
