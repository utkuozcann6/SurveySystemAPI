namespace SurveySystem.Models
{
    public class SurveyOption
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
    }
}
