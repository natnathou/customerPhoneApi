using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using customerPhoneApi.Dtos;

namespace customerPhoneApi.utility
{
    public class ObjectValidation
    {
        public void userIsValid(UserDto user)
        {
            var errorMessagesList = new List<string> { };

            if (user.Username == null)
                errorMessagesList.Add("Username cannot be empty");
            if (user.Name == null)
                errorMessagesList.Add("Name cannot be empty");

            string pattern = null;
            pattern = "^[0-9]{10}$";

            if (!Regex.IsMatch(user.PhoneNumber, pattern))
            {
                errorMessagesList.Add("Invalid Phone");
            }
            try
            {
                MailAddress m = new MailAddress(user.Email);

            }
            catch (System.Exception)
            {
                errorMessagesList.Add("Invalid Format Email");

            }
            pattern = "^[a-zA-Z]{3,}$";
            if (!Regex.IsMatch(user.Password, pattern))
            {
                errorMessagesList.Add("Invalid format password");

            }
            if (errorMessagesList.Count() != 0)
            {
                string errorMessages = string.Join(",", errorMessagesList);
                throw new Exception(errorMessages);
            }

        }

    }
}