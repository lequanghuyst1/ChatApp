using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatApp.Utilities.Util
{
    public static class PolicyUtil
    {
        public static bool IsValidImageUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);

                // Get the file extension from the URL
                string fileExtension = System.IO.Path.GetExtension(uri.LocalPath);

                // List of common image file extensions
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                // Check if the file extension is in the list of image extensions
                return Array.Exists(imageExtensions, ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
            }
            catch (UriFormatException)
            {
                // Handle invalid URL format
                return false;
            }
        }

        public static bool IsVietnamesePhoneNumber(string number)
        {
            // Define a regular expression for Vietnamese phone numbers
            string pattern = @"^(03|05|07|08|09|01[2689])\d{8}$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to validate the phone number
            return regex.IsMatch(number);
        }

        public static bool ValidateEmail(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(emailRegex);
            if (regex.IsMatch(email))
                return true;
            else
                return false;
        }

        public static bool CheckPassword(string password)
        {
            if (password.Length < 6 || password.Length>18)
            {
                return false;
            }
            return password.Select(c => (byte)c).All(code => !((code < 48) | (code > 122)));
        }

        public static bool CheckUserName(string userName)
        {
            // Độ dài từ 4-16
            if (userName.Length < 4 || userName.Length > 19) return false;

            //Kí tự đầu tiên phải là chữ cái
            var fillterChar = "abcdefghijklmnopqrstuvxyzw0123456789._";
            if (fillterChar.IndexOf(userName[0]) < 0) return false;

            //Kí tự '.' không được xuất hiện liền nhau
            if (userName.IndexOf("..") >= 0) return false;

            // Ký tự '.' không được ở sau cùng
            if (userName.EndsWith(".")) return false;

            //Chuỗi hợp lệ   abcdefghijklmnopqrstuvxyzw012345678.
            fillterChar = "abcdefghijklmnopqrstuvxyzw0123456789._";
            return userName.All(t => fillterChar.IndexOf(t) >= 0);
        }
    }
}
