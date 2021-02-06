using System.ComponentModel.DataAnnotations;

namespace customerPhoneApi.Dtos
{
  public class GetUserDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
  }
}