using System.ComponentModel.DataAnnotations;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }
        [Required]
        public DateTime DateExpired { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public RefreshToken()
        {
            
        }

        public RefreshToken(string refreshToken, DateTime dateExpired, int userId)
        {
            Token = refreshToken;
            DateExpired = dateExpired;
            UserId = userId;
        }
    }
}
