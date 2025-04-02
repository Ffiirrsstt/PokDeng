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
        private double _money;
        Calculate cal = new Calculate();

        public Player(double money )
        {
            _money = money;
        }

        //กี่เท่า, เดิมพัน
        // times-1 หรือ  times+1  เพราะตอนเริ่มเดิมพันหักส่วนเดิมพันไปแล้ว
        public void money_in(int times, double bet) => _money = ((times + 1) * bet) + _money;
        public void money_out(int times, double bet) => _money = _money - ((times-1) * bet); 

        public void refund_bet(double bet) => _money += bet; //แปลว่าเสมอ
        public void deduct_bet(double bet) => _money -= bet;//ลงเงินเดิมพัน
        public string display() => "$ " + cal.display_money(_money);

    }
}
