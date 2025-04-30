namespace Frontend.Models
{
    public class EventModel
    {
        public string Id { get; set; }
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public int Participants { get; set; } = 0;

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }


    }
}
