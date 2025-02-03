﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulasi_Laundry_Ifa_New
{
    internal class Properti
    {

        public static SqlConnection conn()
        {
            return new SqlConnection("Data Source=DESKTOP-18L8S2S;Initial Catalog=LaundrySyifa;Integrated Security=True;MultipleActiveResultSets=true");

        }

        public static bool validasi(Control.ControlCollection container, TextBox kosong = null)
        {
            foreach (Control c in container)
            {
                if(c is TextBoxBase textBox && string.IsNullOrWhiteSpace(textBox.Text) && textBox != kosong)
                {
                    return true;
                } 
            } return false;
        }

        public static string enkripsi (string input)
        {
            MD5 mD5 = MD5.Create();
            byte[] bytes = mD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i]).ToString();
            }
            return sb.ToString();
        }
    }
}
