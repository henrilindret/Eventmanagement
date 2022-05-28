namespace Proovitöö.Models
{
    public class Privatecustomer
    {
        public int ID { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public long identitynumber { get; set; }
        public string payment_type { get; set; }
        public string additionalinfo { get; set; }

        public Privatecustomer(int ID, string firstname, string surname, long identitynumber, string payment_type, string additionalinfo)
        {
            this.ID = ID;
            this.firstname = firstname;
            this.surname = surname;
            this.identitynumber = identitynumber;
            this.payment_type = payment_type;
            this.additionalinfo = additionalinfo;
        }
    }
}
