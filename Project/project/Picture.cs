using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    internal class Picture
    {
        Dictionary<int, Tuple<PictureBox, Point, Size>> _chips;
        public Picture(Dictionary<int, Tuple<PictureBox, Point, Size>> chips = null)
        {
            _chips = chips; 
        }

        double num = 0.95;
        public void resize_chip(PictureBox pictureBox)
        {
            //Image original_image = pictureBox.BackgroundImage;
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
                PictureBox pic = chip.Value.Item1; //pictureBox
                Point point = chip.Value.Item2; //x,y
                Size size = chip.Value.Item3; //w,h

                resize_location_pic(pic, size.Width, size.Height, point.X , point.Y );
            }
        }

        public void setting_pic_Stretch(PictureBox pic) => pic.SizeMode = PictureBoxSizeMode.StretchImage;

        public void setting_pic_list_Stretch(List<PictureBox> pic_list) { 
            foreach (PictureBox pic in pic_list) setting_pic_Stretch(pic); 
        }

    }
}
