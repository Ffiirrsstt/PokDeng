using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace project
{
    internal class Tab
    {
        TabControl _tabCtr;
        Dictionary<int, TabPage> _tab_dic;
        int page_1 = 0, page_2 = 1, page_3 = 3;

        public Tab(TabControl tabCtr, Dictionary<int, TabPage> tab_dic)
        {
            _tabCtr = tabCtr;
            _tab_dic = tab_dic;
        }

        //เวลาแก้เช่นเพิ่มหรือลด index ได้ไม่ต้องคอยแก้ index
        public void select_tab(int dic_key_tab) =>_tabCtr.SelectedIndex = _findIdx_select_key(dic_key_tab);

        async Task hide(int dic_key_tab) => _tabCtr.TabPages.Remove(_tab_dic[dic_key_tab]);

        public void hide_start_program() {
            for (int idx = 0;idx < _tab_dic.Keys.Count;idx++)
                if (idx!=0)
                _tabCtr.TabPages.Remove(_findValue_tabDic_Idx(idx)); //value จาก index
        }

        async Task show(int dic_key_tab) {
            //เผื่อเปิดค้างไว้ ต้องปิดให้เรียบร้อยก่อนน่ะ
            hide(dic_key_tab);
            _tabCtr.TabPages.Add(_tab_dic[dic_key_tab]);
        }
        public async void new_pokdeng_game()
        {
            //ในแสดงจะมีให้ปิดไปก่อน (กรณีแบบกันเปิดซ้ำกันน่ะ แต่ที่นี่ถ้าไม่ใช้ await บางทีคำสั่งเลือกแสดงหน้ามันเสร็จก่อนจะแสดงหน้าสองน่ะ ซึ่งทำให้มันไม่แสดงผลเลือกหน้าที่สองตามที่ต้องการ)
            await show(page_2); 
            hide(page_3); //ปิดเกมที่เล่นทิ้งไว้ พอดีเมธอดนี้ใช้ร่วมกับเวลาจบเกมแล้วเริ่มเล่นใหม่น่ะ
            select_tab(page_2);
        }
        
        public void play_pokdeng_game()
        {
            show(page_3);
            select_tab(page_3);
            hide(page_2); //ขณะเล่น ปิดหน้าตอนวางเดิมพัน
        }

        TabPage _findValue_tabDic_Idx(int dic_key_tab) => _tab_dic.Values.ElementAt(dic_key_tab);
        int _findIdx_select_key(int dic_key_tab)
        {
            int idx_select = 0;
            for (int idx = 0; idx < _tab_dic.Keys.Count; idx++)
            {
                TabPage value_current = _findValue_tabDic_Idx(idx);
                TabPage value_select = _tab_dic[dic_key_tab];
                if (value_current == value_select) idx_select = idx;
            }

            return idx_select;
        }

    }
}
