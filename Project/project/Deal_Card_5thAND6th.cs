using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    internal class Deal_Card_5thAND6th
    {
        Cards_Games_UI_deal ui_deal = new Cards_Games_UI_deal();
        Cards_Games cards_game = new Cards_Games();
        Cards_Games_UI ui = new Cards_Games_UI();
        Picture picture = new Picture();

        //int point_dealer = result_hand.result_dealer.points_cards;
        public bool dealer_decide(int point_dealer_2Card) //t-แจก | f-ไม่แจก      point_dealer_2Card = คะแนนตอนการ์ดบนมือมีแค่สองใบน่ะ
        {
            if (point_dealer_2Card <= 4)
                return true;
            return false;
        }

        public (Dictionary<int, Picture_move>, List<List<Cards>>) dealer_decide_animation(List<List<Cards>> card_hands, Dictionary<int, Picture_move> dic_deck, List<Cards> deck, Timer timer)
        {
            int idx_dealer = 1, draw_card = 1;
            card_hands = cards_game.draw_additionalCard(card_hands, new List<int> { idx_dealer }, draw_card, deck);
            dic_deck[6].display = true; //แค่กำหนดอนิเมชันเฉย ๆ น่ะ ไม่เกี่ยวกับข้อมูล
            dic_deck[6].start_move = true;
            timer.Enabled = true;
            return (dic_deck, card_hands);
        }

        public (Dictionary<int, Picture_move>, List<List<Cards>>) player_animation(List<List<Cards>> card_hands, Dictionary<int, Picture_move> dic_deck, List<Cards> deck, Timer timer)
        {
            int idx_player = 0, draw_card = 1;
            //shuffleCards;
            card_hands = cards_game.draw_additionalCard(card_hands, new List<int> { idx_player }, draw_card, deck);
            dic_deck[5].display = true;
            dic_deck[5].start_move = true;
            timer.Enabled = true;
            return (dic_deck, card_hands);
        }

        //return 1.เช็กว่าเงื่อนไขนี้ทำไหม ถ้าทำให้ return จบการเช็กรอบนี้ , 2. startGame_player_draw 3. startGame_dealer_draw
        (bool, bool, bool, Dictionary<int, Picture_move>, List<List<Cards>>) check_after_5th(Player player, Dictionary<int, Picture_move> dic_deck,
            bool startGame_player_draw, bool startGame_dealer_draw, int point_dealer_2Card, List<List<Cards>> card_hands, List<Cards> deck, Timer timer, double bet, Label money_player_label)
        { //return แรกบอกว่าจบการ return ของ animation_deal_draw ไหม
            Point loc_card_number_fifth = dic_deck[5].pic.Location;
            Point target_card_number_fifth = dic_deck[5].loc_target;
            if (startGame_player_draw && loc_card_number_fifth.X <= target_card_number_fifth.X && loc_card_number_fifth.Y >= target_card_number_fifth.Y)
            {
                //ถ้าใบที่ห้าถึงที่หมาย ให้เรียกใช้ตัดสินใจว่าจะจั่วไพ่ dealer มั้ย ถ้าไม่จั่วก็ปิด timerนะ
                if (dealer_decide(point_dealer_2Card))
                {
                    //ดีลเลอร์ตัดสินใจจั่ว รอให้ดีลเลอร์จั่วเสร็จค่อยหาผลลัพธ์ - ปล. ในกรณีที่ดีลเลอร์จั่ว ยังไม่แสดงรูปไพ่ใบที่ 3 ของผู้เล่น รอแสดงพร้อมดีลเลอร์
                    (dic_deck, card_hands) = dealer_decide_animation(card_hands, dic_deck, deck, timer);
                    // startGame_player_draw มีไว้ไม่ให้มันมาทำ if นี้ซ้ำตอนที่มาวนไพ่แจกให้ dealer น่ะ
                    // startGame_player_draw ทำแล้วเปลี่ยนเป็น false ส่วน startGame_dealer_draw เนื่องจากดีลเลอร์ตัดสินใจจั่ว ดังนั้นให้ true
                    return (true, false, true, dic_deck, card_hands);
                }
                else
                {
                    //ดีลเลอร์ตัดสินใจไม่จั่ว หาผลลัพธ์มาแสดง ui เลย
                    timer.Enabled = false;
                    picture.show_card(dic_deck, card_hands, 2, 1);
                    //จัดการเรื่องแจกไรเรียบร้อยแล้วมาหาเรื่องผลลัพธ์
                    dic_deck[5].pic.Image = Image.FromStream(new MemoryStream(card_hands[0][2].picture)); //ดึงรูปของผู้เล่น ใบที่ 3
                    ui.cal_display_resultUI(player, card_hands, bet, money_player_label);

                    //จริง ๆ จบเกมไม่ได้ใช้ทำไรแล้วตรง return นี้
                    return (true, false, false, dic_deck, card_hands);
                }
            }
            return (false, startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands); //อันนี้เน้นแค่ค่าแรก result_return ไว้เช็กว่าเงื่อนไขนี้ทำไหม ถ้าทำจะได้ return 
        }

        (bool, bool, bool, Dictionary<int, Picture_move>, List<List<Cards>>) check_after_6th(Player player, Dictionary<int, Picture_move> dic_deck,
            bool startGame_player_draw, bool startGame_dealer_draw, List<List<Cards>> card_hands, List<Cards> deck, double bet, Label money_player_label,Timer timer)
        {
            Point loc_card_number_sixth = dic_deck[6].pic.Location;
            Point target_card_number_sixth = dic_deck[6].loc_target;
            //แจกไพ่ที่ 6 (ไพ่ดีลเลอร์เสร็จก็แสดงผลแพ้-ชนะ)
            if (startGame_dealer_draw && loc_card_number_sixth.X <= target_card_number_sixth.X && loc_card_number_sixth.Y >= target_card_number_sixth.Y)
            {
                timer.Enabled = false;
                //เช็กว่าผู้เล่นจั่วมั้ย (มีข้อมูลใบที่ 3 ไหม) ถ้ามีก็แสดงผล
                if (card_hands[0].Count > 2) //เช็กว่ามีใบที่สามไหมอะแหละ - card_hands[0][2].picture;
                    dic_deck[5].pic.Image = Image.FromStream(new MemoryStream(card_hands[0][2].picture)); //ดึงรูปของผู้เล่น ใบที่ 3

                picture.show_card(dic_deck, card_hands, 3,1); //เปิดไพ่ดีลเลอร์ทั้งหมด

                ui.cal_display_resultUI(player, card_hands, bet, money_player_label);
                return (true, false, false, dic_deck, card_hands);
            }
            return (false, startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands);
        }

        //สำหรับแจกใบที่5 และ 6 รวมทั้งแสดงผล ui ชนะไรงี้ | return สองตัวแรก คือ startGame_player_draw, startGame_dealer_draw
        public (bool, bool, Dictionary<int, Picture_move>, List<List<Cards>>) animation_deal_draw(Form form, Player player, Dictionary<int, Picture_move> dic_deck, List<List<Cards>> card_hands, Timer timer, int speed, bool startGame_player_draw, bool startGame_dealer_draw, Label draw_card, Label not_draw_card, double bet, Label money_player_label, int point_dealer_2Card, List<Cards> deck, RichTextBox r)
        {
            bool result_return; //เก็บผลลัพธ์การเข้าเงื่อนไขน่ะ

            ui_deal.move_deal(form, dic_deck, 0, speed); //0 คือ card_number_change ในที่นี้ไม่ได้ใช้ เพราะไม่ได้จะแก้ข้อมูล dic_deck

            (result_return, startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands) = check_after_5th(player, dic_deck,
             startGame_player_draw, startGame_dealer_draw, point_dealer_2Card, card_hands, deck, timer, bet, money_player_label);
            if (result_return) return (startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands); //เช็กเข้าเงื่อนไขใน check_after_5th ไหม ถ้าเข้าแล้ว ไม่ต้องไปทำอันอื่นน่ะ


            (result_return, startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands) = check_after_6th(player, dic_deck, startGame_player_draw, startGame_dealer_draw, card_hands, deck, bet, money_player_label, timer);
            if (result_return) return (startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands);

            //ถ้าไม่เข้าเงื่อนไขไรเลย ให้ return startGame_player_draw, startGame_dealer_draw [ไม่ได้ setting เปลี่ยนไร]
            return (startGame_player_draw, startGame_dealer_draw, dic_deck, card_hands);
        }
    }
}
