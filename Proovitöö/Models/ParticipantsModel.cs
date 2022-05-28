namespace Proovitöö.Models
{
    public class ParticipantsModel
    {
        public Event Event { get; set; }

        public List<Companycustomer> Complist { get; set; }

        public List<Privatecustomer> Privlist { get; set; }

        public ParticipantsModel(Event @event, List<Companycustomer> complist, List<Privatecustomer> privlist)
        {
            Event = @event;
            Complist = complist;
            Privlist = privlist;
        }
    }


}
