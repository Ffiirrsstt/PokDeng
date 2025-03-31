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
        // เมธอดในการสับไพ่
        public List<Cards> ShuffleCards(List<Cards> cards)
        //public DataTable ShuffleCards(DataRowCollection cards)
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

        // เมธอดเพื่อแจกไพ่
        /*public void DealCards(List<Card> shuffledCards)
        {
            // แสดงผลการแจกไพ่ (ตัวอย่าง)
            foreach (var card in shuffledCards)
            {
                Console.WriteLine($"{card.Rank} of {card.Suit}");
            }
        }*/
    }
}
