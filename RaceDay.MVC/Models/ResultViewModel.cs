namespace RaceDay.MVC.Models
{
    public class ResultViewModel
    {
        public int ResultId { get; set; }
        public int EnrolmentId { get; set; }
        public EnrolmentViewModel? Enrolment { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
    }
}