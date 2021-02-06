using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace customerPhoneApi.models
{
  public class User
  {
    private string _phoneNumber;
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public string Address { get; set; }
    public DateTime DateCreation { get; set; }

    public string PhoneNumber
    {

      get
      {

        return _phoneNumber;
      }
      set
      {
        string pattern = null;
        pattern = "^\\+?(972|0)(\\-)?0?(([23489]{1}\\d{7})|[5]{1}\\d{8})$";

        if (Regex.IsMatch(value, pattern))
        {
          _phoneNumber = value;
        }
        else
        {
          throw new Exception("Invalid Phone");
        }
      }
    }

  }
}