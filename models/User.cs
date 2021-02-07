using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace customerPhoneApi.models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateCreation { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

}
