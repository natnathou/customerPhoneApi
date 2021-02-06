namespace customerPhoneApi.services
{
  public class ResponseService<T>
  {
    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

  }
}