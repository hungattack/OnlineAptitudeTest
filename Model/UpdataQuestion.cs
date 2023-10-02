namespace OnlineAptitudeTest.Model
{
    public class UpdataQuestion
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string PartId { get; set; }
        public string OccupationId { get; set; }
        public string QuestionName { get; set; }
        public string? AnswerArray { get; set; }
        public string? Answer { get; set; }
        public int Point { get; set; }

    }
}
