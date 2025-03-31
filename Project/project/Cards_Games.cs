using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class Cards_Games
    {
        // การสับไพ่
        public List<Cards> ShuffleCards(List<Cards> cards)
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
        public List<List<Cards>> deal_cards(List<Cards> cards ,int cardsPerPlayer)
        {
            int count_player = 1; //ยังไม่นับรวมเจ้ามือ
            List<List<Cards>> hands = new List<List<Cards>>(); //เก็บไพ่บนมือ

            for (int i = 0; i < count_player+1; i++)
            {
                hands.Add(new List<Cards>());
            }

            int totalCardsUse = (count_player + 1) * cardsPerPlayer; // คำนวณจำนวนไพ่ที่จะแจก

            int cardIndex = 0;
            for (int round = 0; round < cardsPerPlayer; round++)
            {
                for (int i = 0; i < count_player+1; i++) // รวมเจ้ามือแล้ว - hands[index ท้ายสุดคือเจ้ามือ]
                {
                    hands[i].Add(cards[cardIndex]);
                    cardIndex++;
                }
            }

            return hands;
        }
    }
}
