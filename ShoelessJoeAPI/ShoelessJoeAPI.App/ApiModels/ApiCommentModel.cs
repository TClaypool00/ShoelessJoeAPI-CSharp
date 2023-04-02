namespace ShoelessJoeAPI.App.ApiModels
{
    public class ApiCommentModel
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public string Date { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
