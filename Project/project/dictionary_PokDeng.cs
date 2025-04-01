using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class dictionary_PokDeng
    {
        Dictionary<string, int> dic_special_hands_type = new Dictionary<string, int>
        {
                    { "three_kind", 3 },
                    { "royal_cards", 2 },
                    { "straight", 1 },
        };
        List<string> result_game = new List<string> { "win", "lose", "draw", "error" };

        //อ่านเท่านั้น
        public IReadOnlyDictionary<string, int> Dic_SpecialHandsTypeReadOnly => dic_special_hands_type;
        public List<string> list_result_game => result_game;
    }
}
