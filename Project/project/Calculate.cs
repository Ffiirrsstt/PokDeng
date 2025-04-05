
using System;
using System.Drawing;
using System.Windows.Forms;

namespace project
{
    internal class Calculate
    {
        public string display_money(double number)
        {

            if (number >= 1000000)
                return (number / 1000000.0).ToString("0.#") + "M"; // สำหรับล้าน
            
            if (number >= 1000)
            {
                // แสดงจุดทศนิยม 1 ตำแหน่ง
                double value = number / 1000.0;
                return value.ToString("0.#") + "K"; 
            }

            // น้อยกว่าพันแสดงผลตัวเลขแบบคอมม่า
            string formatted_number = number.ToString("N0");
            return formatted_number; // สำหรับตัวเลขที่น้อยกว่า 1000
        }

        //messageBox_warning - true คือแจ้งผ่าน MessageBox | false คือแจ้งผ่านทาง Label
        public double betStart_string_ToDouble(string numberString,Label display_label, bool messageBox_warning=true)
        {
            double number;
            string txt = "";
            //เอา , ออก และช่องว่างออก (ป้องกันพวก 123,22 | 12 123)
            string numberString_manage = numberString.Replace(",","").Replace(" ",""); 
            bool result_success = double.TryParse(numberString, out number);

            //แปลงข้อมูลได้มั้ย และเงินเดิมพันตั้งต้นมากกว่าหนึ่งพันมั้ย (เล่นขั้นต่ำตาละพัน)
            if (result_success&& number >= 1000 && number < 2147483647) return number; // Output: 123.45

            //numberString_manage ถูกใช้ .Replace(" ","") แล้ว ถ้าเป็นเว้นวรรคใด ๆ จะกลายเป็น ""
            if (numberString_manage == "") txt = "กรุณากรอกตัวเลข !"; //กันเดอะช่องว่างน่ะ
            //มีข้อผิดพลาดอะไรสักอย่างที่ทำให้แปลงไม่ได้ เช่น ตัวอักษร , ตัวเลขปนตัวอักษรไรงี้
            else if (!result_success) txt = "กรุณากรอกข้อมูล และต้องกรอกเป็นตัวเลขเท่านั้น !";
            else if (number < 1000) txt = "กรุณากรอกยอดเงินเป็นจำนวนมากกว่า 1,000 !";
            else if (number > 2147483647) //2,147,483,647 คือค่าสูงสุดที่ trackber รับได้น่ะ
                txt = "กรุณากรอกยอดเงินเป็นจำนวนน้อยกว่า 2,147,483,647 !";
            else txt = "ข้อมูลที่คุณกรอกมีข้อผิดพลาด กรุณาแก้ไข" +Environment.NewLine+"หรือติดต่อผู้ดูแลระบบหากการกรอกข้อมูลของคุณถูกต้องแล้ว";

            if (messageBox_warning) MessageBox.Show(txt);
            else{ 
                display_label.Text = txt;
                //SystemColors เป็นคลาสที่รวมสีของระบบ เช่น Control, ControlDark, Highlight ใน Color.ControlDark มันไม่มี ControlDark
                display_label.BackColor = SystemColors.ControlDark; 
            }

            return 0; //มีข้อผิดพลลาดใด ๆ return 0
        }


    }
}
