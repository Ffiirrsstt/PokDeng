using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace project
{
    internal class dictionary_PokDeng
    {
        public Dictionary<string, int> dic_special_hands_type { get; private set; }  = new Dictionary<string, int>
        {
               { "three_kind", 3 },
               { "royal_cards", 2 },
               { "straight", 1 },
        };

        public List<string> result_game { get; private set; }  = new List<string> { "win", "lose", "draw", "error" };

        //ตำแหน่งเริ่มต้นน่ะ
        /*
        objects = new Dictionary<int, (int, int, int)>
        {
            { 0, (200, 50, 0) },   // ชิ้นที่ 1 เริ่มทันที
            { 1, (300, 200, 5) },  // ชิ้นที่ 2 รอ 5 นาที
            { 2, (50, 300, 6) },   // ชิ้นที่ 3 รอ 6 นาที
            { 3, (250, 350, 7) }   // ชิ้นที่ 4 รอ 7 นาที
        };*/
    }
}
