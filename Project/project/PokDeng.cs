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
         */

        //อันนี้ชนศักดิ์แบบวัดคิงมี แต่ไม่สนชนศักดิ์วัดดอกนะ ถ้าคิงโพธิ์ดำ vs คิงโพธิ์แดงให้ถือว่าเจ๊า

        //ไพ่บนมือมีค่าเท่าไหร่ - บนมือของแต่ละคน
        // return - เท่าไหร่ , จ่ายกี่เท่า, ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true/false), ไพ่ชนิดอะไร(พวก.. เซียน เรียง -.. royal_cards , straight) , ศักดิ์อะไร (เช่น เซียนชนเซียน อีกฝั่งเป็นเซียนแต่มีคิงโพธิ์ดำ อีกอันเป็นเซ๊ยนแต่สูงสุดเป็นควีนงี้ - เอาไว้ประชันกันว่าศักดิ์ไหนชนะ ; เก็บเป็นข้อมูล แบบ 1,2,3,...,13 [A คือ 14 แต่เก็บสูงสุดที่ 13 เพราะ A มีได้ทั้ง A 2 3 กับ Q K A ซึ่งสูงสุดที่เป็นไปได้ คือ Q K A รวมทั้งถ้าตองชนตอง ตอง K สูงสุด ดังนั้นจึงเป็น 1,...,13 [13 = K])
        public (int points_cards, int times_pay, bool special_hands, string special_hands_type, int hierarchy) 
            much_cards_hand(List<Cards> cards_hand)
        {
            int cards_count = cards_hand.Count;
            int points_cards = 0; //แต้มทั้งหมดบนมือ

            //same_cards ใช้เช็กว่ายังเหมือนกันไหม เช่น 2 2 | 3 3 เอาไว้เช็กว่า 2เด้งไหม หรือตองงี้
            bool suit_cards = true, royal_cards = true,straight = true, same_cards = true;
            //ดอกของใบแรก , ค่าใบของใบแรก
            string suit_firstCard = "", rank_firstCard = "";
            int max=-1, min=-1, mid=-1; //ตั้งไม่ให้ error เพราะของจริงมีตั้ง default ไว้ตอน cardIdx == 0
            int times_pay;//จ่ายกี่เท่า

            bool special_hands = false; //ไว้ใช้ return น่ะ ว่าไพ่บนมือเป็นพวกเซียนไรงี้มั้ย
            string special_hands_type = ""; //ไว้ใช้ return ว่าเป็นไพ่ชนิดพิเศษแบบไหน เช่น ตอง เซียน เรียง งี้ (ยกตัวอย่างมันอาจเป็นทั้งตอง และเซียน แต่ตองมีมูลค่ามากกว่า นับว่าเป็นตอง งี้)
            int hierarchy; //ศักดิ์อะแหละ เก็บ 1,...,13 ไว้ใช้เวลาชนกัน แบบไพ่เซียนเจอเซียน
            //จะเช็กว่าเรียงไหม โดยเก็บเป็น min , max และตัวกลาง (จะได้ไม่ต้องเก็บเป็นอาเรย์แล้วมาไล่เช็กว่าเรียงไหมใหม่)
            // min mid max เก็บในรูป 1,...,10,11,12,13 (ให้ A = 1,...,10 = 10,J=11,Q=12,K=13,A=14)
            // A = 1 และ A = 14 เพราะเล่นแบบ A ,2 ,3 และ Q K A นับว่าเรียง
            // คำนวณมาเก็บจริง A ให้ 1 แต่ตอนคำนวณว่าเป็นเรียงไหมจะแปลง 1 เป็น 14 อีกที

            PokDeng_Services services = new PokDeng_Services();

            for (int cardIdx = 0; cardIdx < cards_count; cardIdx++)
            {
                int points_card_number; //แต้มแบบ 1 ,...,13,14 ใช้หา min mid max
                string rank_currentCard = cards_hand[cardIdx].rank;

                //เช็กดอกกับไพ่ว่าเหมือนกันมั้ย - ถ้าเหมือนกัน 2 ใบ = 2 เด้ง เช่น 3 3 , ถ้าเหมือนกัน 3 ใบเรียกตอง
                (suit_cards, same_cards) = services.check_threeOfKind_Bonus(cards_hand, cardIdx, suit_firstCard, rank_firstCard, rank_currentCard, suit_cards, same_cards);

                //แปลงไพ่เป็นแต้มแล้วรวมแบบ total
                //เช็กว่ามีเป็นเซียนไหม
                //แปลงเลขรูปแบบ points_card_number (1,...,13,14 สำหรับทำไพ่เรียง)
                (points_card_number, points_cards, royal_cards) = services.convertCardsToPoint_checkFaceCard(points_cards,rank_currentCard, royal_cards);

                //เอา points_card_number หาไพ่ max min mid เพื่อใช้ในการเช็กว่าเป็นไพ่เรียงไหม
                //ยังให้ A = 1 ตรง ๆ ยังไม่แปลง A = 14 เรื่องนั้นไว้ใช้ตอนเช็กเรียงทีเดียว
                (min, mid, max) = services.min_mid_max(cardIdx, points_card_number, straight, min, mid, max);
            }

            //ถ้า มีไพ่เหมือนกันหรือเซียน ไม่มีโอกาสเป็นเรียง - ดังนั้นต้องไม่ใช่เซียน ไม่ใช่ไพ่ซ้ำ เช่นตอง และต้องมีไพ่ถึงสามใบ จึงจะ 'มีโอกาส' เป็นเรียง
            //same_cards มันเช็กได้แค่เหมือนกันหมดทุกใบบนมือไหม ถ้าเหมือนสองในสามมันเช็กไม่ได้อะนะ ดังนั้นเลยเขียนเช็กอีกทีใน check_straight อยู่แล้ว
            //check_straight ไม่มีเช็กว่าไพ่ซ้ำก็ได้ แต่ที่ใส่เช็ก เพื่อที่ถ้าซ้ำจะได้ไม่ไปคำนวณอย่างอื่นต่อ ให้ออก check_straight น่ะ

            hierarchy = max; //max จะเป็น ..,13 อยู่แล้ว (A default 1 ก่อน ข้างล่างจุึงแปลง ดังนั้นจึงมากำหนดค่าศักดิ์ตรงนี้)

            if (straight && royal_cards != true && cards_count == 3)
            {
                // คำนวณมาเก็บจริง A ให้ 1 แต่ตอนคำนวณว่าเป็นเรียงไหมจะแปลง 1 เป็น 14 อีกที
                straight = services.check_straight(min, mid, max);
            }


            //เช็กว่าจ่ายกี่เทา
            times_pay = services.many_times_pay(cards_count, suit_cards, royal_cards, straight, same_cards);

            /*
             ไพ่ 
            10 = 0
            30 = 0
            28 = 8
            11 = 1
             */
            points_cards %= 10;

            //ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true / false), ไพ่ชนิดอะไร(พวกตอง เซียน เรียง - three_kind, royal_cards, straight)
            (special_hands, special_hands_type) = services.check_special_hands(cards_count, royal_cards, straight, same_cards);

            // return - เท่าไหร่ , จ่ายกี่เท่า, ไพ่พิเศษไหม(พวกตอง เซียน เรียง - true/false), ไพ่ชนิดอะไร(พวก.. เซียน เรียง -.. royal_cards , straight) , ศักดิ์อะไร (1,...,13)

            return (points_cards, times_pay, special_hands, special_hands_type, hierarchy);
        }
    }
}
