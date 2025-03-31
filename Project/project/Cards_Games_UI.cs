using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class Cards_Games_UI
    {
        // สับไพ่

        /*
        // แจกไพ่
        public void DealCards(List<Card> cards)
        {
            // แสดงผลการแจกไพ่
            foreach (Cards card in Card)
            {
                Console.WriteLine($"{card.Rank} of {card.Suit}");
            }
        }*/


        /*public MyForm()
{
    // สร้าง PictureBox
    pic = new PictureBox
    {
        Image = Image.FromFile("your_image.png"),
        SizeMode = PictureBoxSizeMode.AutoSize,
        Location = new Point(100, 100) // ตำแหน่งเริ่มต้น
    };
    Controls.Add(pic);

    // ตั้งค่า Timer
    timer = new Timer { Interval = 10 }; // หน่วงเวลา 10ms
    timer.Tick += Timer_Tick;
    timer.Start();
}

private void Timer_Tick(object sender, EventArgs e)
{
    // คำนวณตำแหน่งใหม่แบบ Smooth
    int newX = pic.Left + Math.Sign(targetX - pic.Left) * Math.Min(step, Math.Abs(targetX - pic.Left));
    int newY = pic.Top + Math.Sign(targetY - pic.Top) * Math.Min(step, Math.Abs(targetY - pic.Top));

    pic.Location = new Point(newX, newY);

    // หยุดเมื่อถึงเป้าหมาย
    if (pic.Left == targetX && pic.Top == targetY)
    {
        timer.Stop();
    }
}

[STAThread]
public static void Main()
{
    Application.Run(new MyForm());
}*/
    }
}
