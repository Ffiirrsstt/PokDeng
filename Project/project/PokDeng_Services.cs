using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    internal class PokDeng_Services
    {
        //ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true / false), ไพ่ชนิดอะไร(พวกตอง เซียน เรียง - three_kind, royal_cards, straight)
        public (bool, string) check_special_hands(int number_cards, bool royal_cards, bool straight, bool same_cards)
        {
            bool three_kind = (same_cards && number_cards == 3); //ตอง
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
        public int many_times_pay(int number_cards, bool suit_cards, bool royal_cards, bool straight, bool same_cards)
        {
            //ตองจ่าย 5 เท่า
            if (same_cards && number_cards == 3) return 5;
            else if (royal_cards || straight) return 3;
            else if (suit_cards) return number_cards;
            //ซ้ำสอง ไม่สนดอก เช่น 2 2 || 3 3 จ่าย 2 เท่า
            else if (same_cards && number_cards == 2) return 2;

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
        public bool check_straight(int min, int mid, int max)
        {
            //มีซ้ำกันถือว่าไม่เรียง check_straight return false ไปเลย ไม่คำนวณต่อ
            if (min == mid || mid == max || min == max)
                return false;

            // คำนวณมาเก็บจริง A ให้ 1 แต่ตอนคำนวณว่าเป็นเรียงไหมจะแปลง 1 เป็น 14 อีกที

            //เช็กว่าเรียงไหม
            //ถ้าเช็กว่าเรียงให้ return true ถ้าไม่เรียงก็ไปคำสั่งเช็กว่ามี 1 ไหม
            if (check_order(min, mid, max))
                return true;

            //เช็กว่ามี 1 ไหม (A เป็นได้ทั้ง 14 และ 1 แต่ตอนนี้ให้ default 1 อยู่)
            //ไม่มี 1 เลย ถือว่าจากข้างต้นได้เช็กละ ว่าไม่มีเรียง ดังนั้น return false
            //เช็กแค่ min เพราะ 1 น้อยสุด
            if (min != 1)
                return false;

            //มี 1 ก็ให้ 1 กลายเป็น 14
            //และเรียงใหม่
            if (min == 1)
            {
                //เพราะ 14 มากสุดทางตัวเลขแล้ว (มี 1,2,...,13,14 - A,...,Q,K,A)
                min = mid;
                mid = max;
                max = 14;
            }

            //ตอนนี้แทนที่ 14 ด้วย 1 ละ
            //เช็กว่าเรียงมั้ย
            return check_order(min, mid, max);
        }

        //เช็กว่าเรียงไหมน่ะ
        public bool check_order(int min, int mid, int max)
        {
            if ((min < mid && mid < max) || (max < mid && mid < min) ||
                (min < max && max < mid) || (mid < max && max < min) ||
                (mid < min && min < max) || (max < min && min < mid))
                return true;
            return false;
        }

        //เอา points_card หาไพ่ max min mid เพื่อใช้ในการเช็กว่าเป็นไพ่เรียงไหม
        public (int min, int mid, int max) min_mid_max(int cardIdx, int points_card, int min, int mid, int max)
        {
            //กำหนด defualt
            if (cardIdx == 0)
                min = mid = max = points_card;
            else if (cardIdx != 0)
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

        //เช็กดอกกับไพ่ว่าเหมือนกันมั้ย
        public (string suit_firstCard,string rank_firstCard, bool suit_cards, bool same_cards) check_Bonus(
            List<Cards> cards_hand, int cardIdx, string suit_firstCard, string rank_firstCard, string rank_currentCard, bool suit_cards, bool same_cards)
        {
            string suit_currentCard = cards_hand[cardIdx].suit;
            if (cardIdx == 0)
            {
                suit_firstCard = suit_currentCard;
                rank_firstCard = rank_currentCard;
            }
            //ถ้าใบอื่น ๆ ไม่เหมือนใบแรกจะมีการเปลี่ยนแปลงตั้งค่า
            //ดอกไม่เหมือนกัน แปลว่าไม่ใช่เด้ง
            else
            {
                if (suit_firstCard != suit_currentCard)
                {
                    MessageBox.Show(suit_firstCard.ToString() + " | " + suit_currentCard.ToString());
                    suit_cards = false;
                }

                //คนละตัวแปลว่าไม่ตอง
                if (rank_firstCard != rank_currentCard)
                    same_cards = false;
            }
            return (suit_firstCard, rank_firstCard, suit_cards, same_cards);
        }

        //แปลงไพ่เป็นแต้ม เช็กว่ามีเป็นเซียนไหม แปลงเลขรูปแบบ points_card_number (1,...,13,14 สำหรับทำไพ่เรียง)
        public (int points_card_number, int points_cards, bool royal_cards) convertCardsToPoint_checkFaceCard(int points_cards, string rank_currentCard, bool royal_cards)
        {
            int points_card_number;
            //.rank_currentCard[0] ให้ดูเฉพาะตัวแรกน่ะ
            //เช็กว่าเป็นไพ่หน้าคนไหม
            if (rank_currentCard[0] == 'J' || rank_currentCard[0] == 'Q' || rank_currentCard[0] == 'K')
            {
                //หน้าคน 0 แต้ม
                points_cards += 0;
                if (rank_currentCard[0] == 'J') points_card_number = 11;
                else if (rank_currentCard[0] == 'Q') points_card_number = 12;
                else points_card_number = 13;
            }
            //ถ้าไม่ใช่ไพ่หน้าคน
            else
            {
                royal_cards = false;
                if (rank_currentCard == "10")
                {
                    points_cards += 0;
                    points_card_number = 10;
                }
                if (rank_currentCard[0] == 'A')
                {
                    points_cards += 1;
                    points_card_number = 1;
                }
                else
                {
                    int rank_int = int.Parse(rank_currentCard);
                    points_cards += rank_int;
                    points_card_number = rank_int;
                }
            }

            return (points_card_number, points_cards, royal_cards);
        }
    }
}
