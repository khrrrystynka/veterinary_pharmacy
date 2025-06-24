namespace VetPharmacyApi.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }  // "admin" або "doctor"
}