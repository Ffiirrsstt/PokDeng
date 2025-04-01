using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
