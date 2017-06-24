using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace EasySweeper_3 {
    partial class MapOperations {

        private static string tesseract_read_floor_number(ref TesseractEngine tessEng, ref Bitmap winterfaceBitmap) {
            tessEng.SetVariable("tessedit_char_whitelist", " Floor0123456789:");
            var page = tessEng.Process(winterfaceBitmap);
            var str = page.GetText();
            return str;
        }

        private static string tesseract_read_keyer(ref TesseractEngine tessEng, ref Bitmap winterfaceBitmap) {
            tessEng.SetVariable("tessedit_char_whitelist", " -0123456789abcdefghijqlmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var page = tessEng.Process(winterfaceBitmap);
            return page.GetText();
        }

        private static string tesseract_read_percent_completed(ref TesseractEngine tessEng, ref Bitmap winterfaceBitmap) {
            var page = tessEng.Process(winterfaceBitmap);
            return page.GetText();
        }

        private static string tesseract_read_player(ref TesseractEngine tessEng, ref Bitmap winterfaceBitmap) {
            tessEng.SetVariable("tessedit_char_whitelist", " -0123456789abcdefghijqlmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var page = tessEng.Process(winterfaceBitmap);
            return page.GetText();
        }

        private static List<Tuple<string, string>> _regex_timer_list = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("z",(":")),
            new Tuple<string, string>("1[}]",("4")),
            new Tuple<string, string>("[?]",("7")),
            new Tuple<string, string>("k",("4")),
            new Tuple<string, string>("190",("40")),
            new Tuple<string, string>("191",("41")),
            new Tuple<string, string>("192",("42")),
            new Tuple<string, string>("193",("43")),
            new Tuple<string, string>("194",("44")),
            new Tuple<string, string>("195",("45")),
            new Tuple<string, string>("196",("46")),
            new Tuple<string, string>("197",("47")),
            new Tuple<string, string>("198",("48")),
            new Tuple<string, string>("199",("49")),
            new Tuple<string, string>("s[:]",(":45")),
            new Tuple<string, string>("qu",(":49")),
            new Tuple<string, string>("1[;]",("4")),
            new Tuple<string, string>("m",(":14"))
        };

        private static string tesseract_read_timer(ref TesseractEngine tessEng, ref Bitmap winterfaceBitmap) {
            var page = tessEng.Process(winterfaceBitmap);
            var str = page.GetText();
            foreach (var reg_match in _regex_timer_list)
            {
                str = Regex.Replace(str, reg_match.Item1, reg_match.Item2);
            }
            return str;
        }

        private static string tesseract_read_level_mod(ref TesseractEngine tessEng, ref Bitmap winterfaceBitmap) {
            var page = tessEng.Process(winterfaceBitmap);
            tessEng.SetVariable("tessedit_char_whitelist", "-+%0123456789");
            return page.GetText();
        }




    }
}
