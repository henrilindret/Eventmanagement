namespace Proovitöö.Models
{
    public class Event
    {
        public string eventname { get; set; }
        public DateTime eventdate { get; set; }
        public string eventplace { get; set; }
        public string eventinfo { get; set; }
        public int eventID { get; set; }

        public Event(string eventname, DateTime eventdate, string eventplace, string eventinfo, int eventID)
        {
            this.eventID = eventID;
            this.eventname = eventname;
            this.eventdate = eventdate;
            this.eventplace = eventplace;
            this.eventinfo = eventinfo;
        }
    }

}
