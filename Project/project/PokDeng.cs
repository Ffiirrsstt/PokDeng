using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace project
{
    internal class PokDeng
    {

        /* suit_cards = เช็กดอก
         * royal_cards = เช็กว่าหน้าคนหมดไหม - เซียน
         * three_kind= เช็กว่าตองไหม
         */

        //ไพ่บนมือมีค่าเท่าไหร่ - บนมือของแต่ละคน
        // return - เท่าไหร่ , จ่ายกี่เท่า, ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true/false), ไพ่ชนิดอะไร(พวกตอง เซียน เรียง - three_kind , royal_cards , straight)
        public void much_cards_hand(List<Cards> cards_hand)
        {
            int cards_count = cards_hand.Count;
            int points_cards = 0; //แต้มทั้งหมดบนมือ

            bool suit_cards = true, royal_cards = true,straight = true, three_kind = true;
            //ดอกของใบแรก , ค่าใบของใบแรก
            string suit_firstCard = "", rank_firstCard = "";
            int max=-1, min=-1, mid=-1; //ตั้งไม่ให้ error เพราะของจริงมีตั้ง default ไว้ตอน cardIdx == 0
            int times_pay;//จ่ายกี่เท่า

            bool special_hands = false; //ไว้ใช้ return น่ะ ว่าไพ่บนมือเป็นพวกเซียนไรงี้มั้ย
            string special_hands_type = ""; //ไว้ใช้ return ว่าเป็นไพ่ชนิดพิเศษแบบไหน เช่น ตอง เซียน เรียง งี้ (ยกตัวอย่างมันอาจเป็นทั้งตอง และเซียน แต่ตองมีมูลค่ามากกว่า นับว่าเป็นตอง งี้)
            //จะเช็กว่าเรียงไหม โดยเก็บเป็น min , max และตัวกลาง (จะได้ไม่ต้องเก็บเป็นอาเรย์แล้วมาไล่เช็กว่าเรียงไหมใหม่)
            // min mid max เก็บในรูป 1,...,0 (ให้ A = 1,...,10 = 0) ; ตอนคำนวณเรียงค่อยเปลี่ยน 0 เป็น 0 ที่ใช้ 0 เพราะจะดึงค่ามาจาก points_card 
            for (int cardIdx = 0; cardIdx < cards_count; cardIdx++)
            {
                int points_card;
                string rank_currentCard = cards_hand[cardIdx].rank;

                //เช็กตองกับเด้ง
                (suit_cards, three_kind) = check_threeOfKind_Bonus(cards_hand, cardIdx, suit_firstCard, rank_firstCard, rank_currentCard, suit_cards, three_kind);

                //แปลงไพ่เป็นแต้ม เช็กว่ามีเป็นเซียนไหม เช็กเบื้่องต้นว่ามีไพ่หน้าคนไหม(ถ้ามีไพ่หน้าคนคือไม่มีโอกาสเป็นเรียง)
                (points_card, royal_cards, straight) = convertCardsToPoint_checkFaceCard(rank_currentCard, royal_cards, straight);
                points_cards += points_card;

                //เอา points_card หาไพ่ max min mid เพื่อใช้ในการเช็กว่าเป็นไพ่เรียงไหม
                (min, mid, max) = min_mid_max(cardIdx, points_card, straight, min, mid, max);
            }

            //ถ้าตองหรือเซียน ไม่มีโอกาสเป็นเรียง - ดังนั้นต้องไม่ใช่เซียน ไม่ใช่ตอง และต้องมีไพ่ถึงสามใบ จึงจะ 'มีโอกาส' เป็นเรียง
            // หมายเหตุ ถ้า straight = false แปลว่าก่อนหน้านี้เบื้องต้นเช็กว่าพบพวก J Q K สักใบบนมือไปน่ะ อันนี้กติกา คือ ไพ่เรียงนับเฉพาะตัวเลข (ยกเว้น A ที่ถือเป็น 1) 
            if (straight && royal_cards != true && three_kind != true && cards_count == 3)
            {
                straight = check_straight(min, mid, max);
            }


            //เช็กว่าจ่ายกี่เทา
            times_pay = many_times_pay(cards_count, suit_cards, royal_cards, straight, three_kind);

            /*
             ไพ่ 
            10 = 0
            30 = 0
            28 = 8
            11 = 1
             */
            points_cards %= 10;

            //ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true / false), ไพ่ชนิดอะไร(พวกตอง เซียน เรียง - three_kind, royal_cards, straight)

            // return - เท่าไหร่ , จ่ายกี่เท่า, ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true/false), ไพ่ชนิดอะไร(พวกตอง เซียน เรียง - three_kind , royal_cards , straight)
            return (points_cards,times_pay)
        }

        //ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true / false), ไพ่ชนิดอะไร(พวกตอง เซียน เรียง - three_kind, royal_cards, straight)
        (bool , string ) check_special_hands(bool royal_cards, bool straight, bool three_kind)
        {
            if (!royal_cards && !straight && !three_kind)
                //ไม่ใช่เซียน ไม่ใช่เรียง ไม่ใช่ตอง ไม่ใช่ไพ่พิเศษ
                //ถ้าเช็กว่า false (ไม่ใช่ไพ่พิเศษ) ก็ไม่เอาค่าว่าเป็นไพ่พิเศษชนิดไหนไปใช้ ดังนั้นก็ใส่ "" 
                return (false, "");
                //ตองใหญ่กว่าเซียน เซียนใหญ่กว่าเรียง

            //ผ่าน if แรกได้แปลว่าต้องเป็นตอง เซียน หรือเรียง
            if (three_kind) return (true, "three_kind");
            if (royal_cards) return (true, "royal_cards");

            //ไม่ได้เป็นตอง ไม่ได้เป็นเซียน ดังนั้นจึงเหลือแค่เป็นเรียง
            return (true, "straight");

        }

        /*เช็กว่าต้องจ่ายกี่เท่า
        ป็อก 9/8 - 1
        สองเด้ง - 2 (ดอกเหมือนกัน)
        สามเด้ง - 3 (ดอกเหมือนกัน)
        เรียง - 3
        เซียน - 3 (หน้าคน)
        ตอง - 5
         */
        int many_times_pay(int number_cards, bool suit_cards, bool royal_cards,bool straight, bool three_kind)
        {
            //ตองไหม
            if (three_kind) return 5;
            else if (royal_cards || straight) return 3;
            else if (suit_cards) return number_cards;

            //default = 1 เท่า
            return 1;
        }


        //เช็กว่าเรียงไหม
        /*
         * ความน่าจะเป็นในการเรียง 3! = 3*2 = 6
         * min , mid , max
         * min , max , mid
         * 
         * mid , min , max
         * mid , max , min
         * 
         * max , min , mid
         * max , mid , min
         * 
         * เรียงให้เขียนโค้ดง่ายขึ้น
         * min , mid , max - min < mid < max || max <mid < min 
         * max , mid , min - แปลงให้ถูกไวยากรณ์ (min < mid && mid < max) || (max < mid && mid < min)
         * 
         * min , max , mid - (min < max && max < mid) || (mid < max && max < min)
         * mid , max , min
         * 
         * mid , min , max - (mid < min && min < max) || (max < min && min < mid)
         * max , min , mid
         */
        bool check_straight(int min, int mid, int max)
        {
            if( (min < mid && mid < max) || (max < mid && mid < min) ||
                (min < max && max < mid) || (mid < max && max < min) ||
                (mid < min && min < max) || (max < min && min < mid))
                return true;
            return false;
        }

        //เอา points_card หาไพ่ max min mid เพื่อใช้ในการเช็กว่าเป็นไพ่เรียงไหม
        (int min,int mid ,int max) min_mid_max(int cardIdx,int points_card, bool straight, int min, int mid, int max)
        {
            //กำหนด defualt
            //หมายเหตุถ้า straight == false ถือว่าไม่สนใจเช็กเรียงละ แปลว่ามันต้องมีไพ่ใบที่เป็นหน้าคนไปแล้ว
            if (cardIdx == 0 && straight)
                min = mid = max = points_card;
            else if (cardIdx != 0 && straight)
            {
                if (points_card > max)
                    max = points_card;
                else if (points_card < min)
                    min = points_card;
                //ถ้าไม่มากกว่า ไม่น้อยกว่า และไม่เท่ากับมากกว่าหรือน้อยกว่าก็ต้องอยู่ระหว่างนั้น
                else if (points_card != max && points_card != min)
                    mid = points_card;
            }

            return (min, mid, max);
        }

        //เช็กตองกับเด้ง
        (bool suit_cards, bool three_kind) check_threeOfKind_Bonus(
            List<Cards> cards_hand, int cardIdx,string suit_firstCard,string rank_firstCard, string rank_currentCard, bool suit_cards, bool three_kind)
        {
            string suit_currentCardx = cards_hand[cardIdx].suit;
            if (cardIdx == 0)
            {
                suit_firstCard = suit_currentCardx;
                rank_firstCard = rank_currentCard;
            }
            //ถ้าใบอื่น ๆ ไม่เหมือนใบแรกจะมีการเปลี่ยนแปลงตั้งค่า
            //ดอกไม่เหมือนกัน แปลว่าไม่ใช่เด้ง
            else if (suit_firstCard != suit_currentCardx)
                suit_cards = false;
            //คนละตัวแปลว่าไม่ตอง
            else if (rank_firstCard != rank_currentCard)
                three_kind = false;
            return (suit_cards, three_kind); 
        }

        //แปลงไพ่เป็นแต้ม เช็กว่ามีเป็นเซียนไหม เช็กเบื้่องต้นว่ามีไพ่หน้าคนไหม(ถ้ามีไพ่หน้าคนคือไม่มีโอกาสเป็นเรียง)
        (int points_card, bool royal_cards, bool straight) convertCardsToPoint_checkFaceCard(string rank_currentCard, bool royal_cards, bool straight)
        {
            int points_card = 0;
            //.rank_currentCard[0] ให้ดูเฉพาะตัวแรกน่ะ
            //เช็กว่าเป็นไพ่หน้าคนไหม
            if (rank_currentCard[0] == 'J' || rank_currentCard[0] == 'Q' || rank_currentCard[0] == 'K')
            {
                //หน้าคน 0 แต้ม
                //points_card = 0;
                //มีไพ่หน้าคน = ไม่มีโอกาสเป็นไพ่เรียง
                straight = false;
            }
            //ถ้าไม่ใช่ไพ่หน้าคน
            else
            {
                royal_cards = false;
                /*if (rank_currentCard == "10")
                    points_cards = 0;*/
                if (rank_currentCard[0] == 'A')
                    points_card = 1;
                else points_card = int.Parse(rank_currentCard);
            }

            return (points_card, royal_cards, straight);
        }
    }
}
