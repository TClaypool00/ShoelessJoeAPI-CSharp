namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreComment
    {
        public int CommentId { get; set; }

        public string CommentText { get; set; }

        public DateTime DatePosted { get; set; }

        public int UserId { get; set; }
        public CoreUser User { get; set; }

        public int PotentialBuyId { get; set; }
        public CorePotentialBuy PotentialBuy { get; set; }
    }
}
