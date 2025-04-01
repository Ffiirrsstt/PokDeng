using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class Player
    {
        //ออกแบบเกมแบบเปิดเข้าเล่นทีเดียวจบงี้ - ตั้งค่าข้อมูลผู้เล่นนิดหน่อย แต่ไม่เก็บข้อมูล และไม่มีสมัครสมาชิก หรือเข้าสู่ระบบใด ๆ
        public int money { get; private set; } = 100000;

        //กี่เท่า, เดิมพัน
        // times-1 หรือ  times+1  เพราะตอนเริ่มเดิมพันหักส่วนเดิมพันไปแล้ว
        public void money_in(int times, int bet) => money = ((times + 1) * bet) + money;
        public void money_out(int times, int bet) => money = money - ((times-1) * bet); 

        public void refund_bet(int bet) => money += bet; //แปลว่าเสมอ
        public void deduct_bet(int bet) => money -= bet;//ลงเงินเดิมพัน
        public string display() => "$ " + money.ToString("N0"); // N0 หมายถึงไม่ต้องการทศนิยม แต่จะเพิ่มคอมม่า

    }
}
