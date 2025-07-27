namespace SurveySystem.DTOs
{
    public class SurveyResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalVotes { get; set; }
        public List<OptionResultDto> Options { get; set; } = new List<OptionResultDto>();
    }
}
