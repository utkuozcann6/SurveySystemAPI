namespace SurveySystem.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SurveyId { get; set; }
        public int SurveyOptionId { get; set; }
        public DateTime VotedAt { get; set; }
    }
}
