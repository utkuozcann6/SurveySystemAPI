namespace SurveySystem.DTOs
{
    public class OptionResultDto
    {
        public int Id { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }
}
