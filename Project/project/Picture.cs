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
    internal class Picture
    {
        Dictionary<int, (PictureBox pic,Point point,Size size)> _chips;
        double num = 0.95;
        public Picture(Dictionary<int, (PictureBox pic, Point point, Size size)> chips = null)
        {
            _chips = chips; 
        }

        public void resize_chip(PictureBox pictureBox)
        {
            int new_width = (int)(pictureBox.Width * num); 
            int new_height = (int)(pictureBox.Height * num);
            int move = 5;

            resize_location_pic(pictureBox, new_width, new_height, pictureBox.Location.X + move, pictureBox.Location.Y + move);
        }

        void resize_location_pic(PictureBox pictureBox,int new_width,int new_height,int x,int y)
        {
            pictureBox.Width = new_width;
            pictureBox.Height = new_height;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Location = new Point(x, y);
        }

        public void restore_size_chip() {
            foreach (var chip in _chips) { 
                PictureBox pic = chip.Value.pic; //pictureBox
                Point point = chip.Value.point; //x,y
                Size size = chip.Value.size; //w,h

                resize_location_pic(pic, size.Width, size.Height, point.X , point.Y );
            }
        }

        public void setting_pic_Stretch(PictureBox pic) => pic.SizeMode = PictureBoxSizeMode.StretchImage;

        public void setting_pic_list_Stretch(List<PictureBox> pic_list) { 
            foreach (PictureBox pic in pic_list) setting_pic_Stretch(pic); 
        }

        //number คือจำนวนไพ่ที่จะเปิดน่ะ เช่น 2 คือเปิดตั้งแต่ใบแรกถึงใบที่สอง | ถ้า 3 คือใบแรกถึงใบที่สาม 
        //player_dealer คือ 0 - โชว์การ์ดของ player , 1 - โชว์การ์ดของ dealer
        public void show_card(Dictionary<int,Picture_move> dic_deck,List<List<Cards>> card_hand,int number,int player_dealer)
        {
            /*
             * ตัวอย่าง 
             * //2i + 1 = 0; i=0|1 i=1|3 i=2|5
                //player = 1 3 5 ถ้า 2i + 1 หรีือ 2i + 2 - 0
                //dealer = 2 4 6 เป็น 2i + 1 หรือ 2i + 1 + 1

             * dic_deck[2].pic.Image = Image.FromStream(new MemoryStream(card_hands[1][0].picture)); //ดึงรูปของดีลเลอร์ ใบที่ 1
            dic_deck[4].pic.Image = Image.FromStream(new MemoryStream(card_hands[1][1].picture)); //ดึงรูปของดีลเลอร์ ใบที่ 2
            dic_deck[6].pic.Image = Image.FromStream(new MemoryStream(card_hands[1][2].picture)); //ดึงรูปของดีลเลอร์ ใบที่ 3*/
            for (int i = 0; i < number; i++)
            {
                PictureBox pic = dic_deck[2 * i + 1 + player_dealer].pic;

                pic.Image = Image.FromStream(new MemoryStream(card_hand[player_dealer][i].picture));
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

    }
}
