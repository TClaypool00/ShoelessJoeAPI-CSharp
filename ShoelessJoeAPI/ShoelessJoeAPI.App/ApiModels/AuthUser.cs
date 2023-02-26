namespace ShoelessJoeAPI.App.ApiModels
{
    public class AuthUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string PhoneNumb { get; set; }
        public string Token { get; set; }
    }
}
