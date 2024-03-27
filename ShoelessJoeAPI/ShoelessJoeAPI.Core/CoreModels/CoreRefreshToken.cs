namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreRefreshToken
    {
        public string RefreshToken { get; set; }
        public DateTime DateExpired { get; set; }
        public int UserId { get; set; }
    }
}
