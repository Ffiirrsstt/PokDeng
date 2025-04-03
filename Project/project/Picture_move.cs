using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    internal class Picture_move
    {
        public PictureBox pic { get; set; }
        public Point loc_target { get; set; }
        public bool display { get; set; }
        public bool start_move { get; set; }

        //ให้รองรับ var (pic, loc,loc_target, _, start_move) = card.Value; น่ะ - การแตกตัวแปรออกจากออบเจ็กต์
        public void Deconstruct(out PictureBox pic, out Point loc_target, out bool display, out bool start_move)
        {
            pic = this.pic;
            loc_target = this.loc_target;
            display = this.display;
            start_move = this.start_move;
        }

        //dis=true,bool move=false | เช่น รอหนึ่งเสร็จ สองค่อยขยับ
        public Picture_move tuple_dic_deck(PictureBox pic, int x_target, int y_target, bool dis = true, bool move = false)
        {
            return new Picture_move
            {
                pic = pic,
                loc_target = new Point(x_target, y_target),
                display = dis,
                start_move = move
            };
        }

        public void dic_To_back_deck(Dictionary<int, Picture_move> dic_pic, Point deck_loc)
        {
            foreach(var pic in dic_pic)
            {
                pic.Value.pic.Location = deck_loc;
            }
        }
    }
}
