using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Threading.Tasks;

namespace project
{
    internal class Cards
    {
        public int id { get; set; }
        public string name { get; set; }
        public byte[] picture { get; set; }
        public string rank { get; set; }
        public string suit { get; set; }

        public string conStr =
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\USE\\project363\\DB\\DB.mdb";
        OleDbConnection conn;

        DataSet data = new DataSet();

        public async Task<List<Cards>> fetchDataCards()
        {
            OleDbConnection conn = new OleDbConnection(conStr);
            //conn.Open();
            await conn.OpenAsync();
            string sql = "select * from Cards";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            //adapter.Fill(data, "Cards");

            //Fill ใช้เพราะพยายามใช้คำสั่งเดียวกับที่เรียน
            await Task.Run(() => adapter.Fill(data, "Cards"));
            DataTable cards_table = data.Tables["Cards"];

            conn.Close();
            return ConvertDataTableToList(cards_table);
        }

        public List<Cards> ConvertDataTableToList(DataTable cards_table)
        {
            List<Cards> cards_list = new List<Cards>();

            foreach (DataRow card_row in cards_table.Rows)
            {
                Cards card = new Cards
                {
                    id = Convert.ToInt32(card_row["Card_ID"]),
                    name = card_row["Card_Name"].ToString(),
                    picture = (byte[])card_row["Card_Picture"],
                    rank = card_row["Card_Rank"].ToString(),
                    suit = card_row["Card_Suit"].ToString(),
                };
                cards_list.Add(card);
            }

            return cards_list;
        }
    }
}
