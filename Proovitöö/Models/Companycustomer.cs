namespace Proovitöö.Models
{
    public class Companycustomer
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public int identitynumber { get; set; }
        public string payment_type { get; set; }
        public string additionalinfo { get; set; }

        public Companycustomer(int ID, string name, int code, int identitynumber, string payment_type, string additionalinfo)
        {
            this.ID = ID;
            this.name = name;
            this.code = code;
            this.identitynumber = identitynumber;
            this.payment_type = payment_type;
            this.additionalinfo = additionalinfo;
        }
    }
}
