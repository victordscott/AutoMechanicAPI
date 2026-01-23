using System.Net.Mail;
using System.Text.RegularExpressions;

namespace AutoMechanic.DataAccess.Helpers
{
    public static class RegexHelper
    {
        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            Regex validatePhoneNumberRegex = new Regex("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}");
            return validatePhoneNumberRegex.IsMatch(phoneNumber);
        }
    }
}
