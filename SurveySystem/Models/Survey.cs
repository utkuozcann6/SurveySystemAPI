namespace SurveySystem.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SurveyOption> Options { get; set; } = new List<SurveyOption>();
    }
}
