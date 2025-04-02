using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public double string_ToDouble(string numberString,bool warning=true) //จะให้แจ้งเตือนมั้ยว่าการแปลงเลขมีปัญหา
        {
            double number;
            bool result_success = double.TryParse(numberString, out number);

            if (result_success) return number; // Output: 123.45
             
            if(warning) MessageBox.Show("ยอดเงินจะต้องเป็นตัวเลขเท่านั้น");
            return -1;
        }


    }
}
