namespace SurveySystem.DTOs
{
    public class CreateSurveyDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Options { get; set; } = new List<string>();
    }
}
