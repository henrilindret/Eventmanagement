namespace Proovitöö.Models
{
    public class Companycustomer
    {
        public int ID { get; set; }
        public string name { get; set; }
        public long code { get; set; }
        public int participants { get; set; }
        public string payment_type { get; set; }
        public string additionalinfo { get; set; }

        public Companycustomer(int ID, string name, long code, int participants, string payment_type, string additionalinfo)
        {
            this.ID = ID;
            this.name = name;
            this.code = code;
            this.participants = participants;
            this.payment_type = payment_type;
            this.additionalinfo = additionalinfo;
        }
    }
}
