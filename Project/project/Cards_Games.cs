using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace project
{
    internal class Cards_Games
    {
        // การสับไพ่
        public List<Cards> shuffle_cards(List<Cards> cards)
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);  // เลือก index แบบสุ่ม
                Cards value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
            return cards;
        }

        // แจกไพ่แบบ data
        public List<List<Cards>> deal_cards(List<Cards> deck, int cardsPerPlayer)
        {
            int count_player = 2; //รวมเจ้ามือ
            List<List<Cards>> hands = new List<List<Cards>>(); //เก็บไพ่บนมือ

            for (int i = 0; i < count_player; i++)
            {
                hands.Add(new List<Cards>());
            }

            int cardIdx = 0;
            return cardData_to_list(hands, deck, cardIdx, cardsPerPlayer, count_player);
        }

        //hands ไพ่ทั้งหมดบนมือที่ถูกแจกไปก่อนหน้านี้ของทั้งสองฝ่าย | deck คือกองไพ่ที่จะใช้จั่ว - กองกลาง
        public List<List<Cards>> draw_additionalCard(List<List<Cards>> hands, int count_addCardsPerPlayer, List<Cards> deck)
        {
            int count_player = hands.Count; //จำนวนผู้เล่น รวมเจ้ามือ
            //ถ้าใช้ไปแล้ว 4 ใบจากในกอง (0-3) แปลว่าใบถัดไปที่จะถูกใช้คือใบที่ 4 
            //ใช้ไป 5 ใบแล้ว (0-4) แปลว่าจั่วใบถัดไปต้องเป้นใบที่ 5
            int cardIdx = hands.Sum(hand => hand.Count);

            return cardData_to_list(hands, deck, cardIdx, count_addCardsPerPlayer, count_player);
        }

        //drawn_cardsPerPlayer , count_player| รวมเจ้ามือ
        List<List<Cards>> cardData_to_list(List<List<Cards>> hands, List<Cards> deck, int cardIdx, int drawn_cardsPerPlayer, int count_player)
        {
            
            for (int round = 0; round < drawn_cardsPerPlayer; round++)
            {
                for (int i = 0; i < count_player; i++) 
                {
                    hands[i].Add(deck[cardIdx]);
                    cardIdx++;
                }
            }

            return hands;
        }
    }
}
