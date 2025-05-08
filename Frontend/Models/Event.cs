namespace Frontend.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<User> Participants { get; set; }
        public DateTime Date { get; set; }


    }
}
