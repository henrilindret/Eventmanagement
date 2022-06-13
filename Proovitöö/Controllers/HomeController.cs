using Microsoft.AspNetCore.Mvc;
using Proovitöö.Models;
using System.Diagnostics;
using System.Data.SqlClient;


namespace Proovitöö.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private long ConvertDatetimeToUnixTimeStamp(DateTime date)
        {
            var dateTimeOffset = new DateTimeOffset(date);
            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }

        public IActionResult Index()
        {
            //Ürituste listi loomine avalehele. Listi järjekord olenevalt ürituse ajast.
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            var sql = "SELECT eventname, eventplace, eventdate, eventinfo, eventID FROM event ORDER BY eventdate ASC";
            using (var cmd = new SqlCommand(sql, Con))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    List<Event> Eventlist = new List<Event>();
                    while (dr.Read())
                    {
                        string eventname = (string)dr["eventname"];
                        long eventdate = (long)dr["eventdate"];
                        DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        DateTime date = start.AddSeconds(eventdate).ToLocalTime();
                        string eventinfo = (string)dr["eventinfo"];
                        int eventID = (int)dr["eventID"];
                        string eventplace = (string)dr["eventplace"];
                        Event e = new Event(eventname, date, eventplace, eventinfo, eventID);
                        Eventlist.Add(e);
                    }
                    Con.Close();
                    return View(new EventList(Eventlist));
                }
            }
            Con.Close();
            return View();
        }

        public IActionResult Eventadding()
        {
            return View();
        }


        public IActionResult Companyedit()
        {
            return View();
        }

        public IActionResult Privateedit(int ID)
        {
            ViewBag.ID = ID;
            return View();
        }




        [HttpPost]
        public IActionResult editprivate(string firstname, string surname, int identitynumber, int payment_type, string additionalinfo, int PrivateID)
        {
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            var sql = "UPDATE private_customer SET firstname = @firstname, surname = @surname, identitynumber = @identitynumber, payment_type = @payment_type, additionalinfo = @additionalinfo where ID = @ID";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@identitynumber", identitynumber);
                cmd.Parameters.AddWithValue("@payment_type", payment_type);
                cmd.Parameters.AddWithValue("@additionalinfo", additionalinfo);
                cmd.Parameters.AddWithValue("@ID", PrivateID);
                cmd.ExecuteNonQuery();
            }

            Con.Close();
            return RedirectToAction("index", "Home");
            return RedirectToAction("Eventedit", "Home", new { id = EventID });
        }

        [HttpPost]
        public IActionResult editcompany(int id)
        {
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();







            Con.Close();
            return View();
        }


        [HttpPost]
        public IActionResult eventadd(string name, string place, string time, string additionalinfo)
        {
            //Ürituse lisamise form-ilt saadud info edastamine andmebaasi.
            var parsedDate = DateTime.Parse(time);
            long timestamp = ConvertDatetimeToUnixTimeStamp(parsedDate);
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            var sql = "INSERT INTO event(eventname, eventdate, eventplace, eventinfo) VALUES(@eventname, @eventdate, @eventplace, @eventinfo)";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@eventname", name);
                cmd.Parameters.AddWithValue("@eventdate", timestamp);
                cmd.Parameters.AddWithValue("@eventplace", place);
                cmd.Parameters.AddWithValue("@eventinfo", additionalinfo);
                cmd.ExecuteNonQuery();
            }
            Con.Close();
            //Peale form-i ära saatmist, tagasi avalehele.
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public IActionResult deleteevent(string id)
        {
            //andmebaasiga ühenduse loomine
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            //andmebaasist kustutamine olenevalt antud ID-st.
            var sql = "DELETE FROM event WHERE eventID = @eventID";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@eventID", id);
                cmd.ExecuteNonQuery();
            }
            Con.Close();
            return RedirectToAction("index", "Home");
        }


        // Eraisiku kustutamine
        [HttpGet]
        public IActionResult deleteprivate(string id)
        {
            //andmebaasiga ühenduse loomine
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            //andmebaasist kustutamine olenevalt antud ID-st.
            var sql = "DELETE FROM private_customer WHERE ID = @ID";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
            Con.Close();
            return RedirectToAction("index", "Home");
        }
        //Ärikliendi/firma kustutamine
        [HttpGet]
        public IActionResult deletecompany(string id)
        {
            //andmebaasiga ühenduse loomine
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            //andmebaasist kustutamine olenevalt antud ID-st.
            var sql = "DELETE FROM company_customer WHERE ID = @ID";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
            Con.Close();
            return RedirectToAction("index", "Home");
        }



        [HttpGet]
        public IActionResult Eventedit(int id)
        {
            //andmebaasiga ühenduse loomine
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Event e = new Event("testname", DateTime.Now, "testplace", "testinfo", 0);
            Con.Open();
            //valib tabelist andmed, mis seostuvad saadetud ID-ga
            var sql = "SELECT eventname, eventplace, eventdate, eventinfo FROM event WHERE eventID = @eventID";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@eventID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    string eventname = (string)dr["eventname"];
                    long eventdate = (long)dr["eventdate"];
                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddSeconds(eventdate).ToLocalTime();
                    string eventinfo = (string)dr["eventinfo"];
                    string eventplace = (string)dr["eventplace"];
                    e = new Event(eventname, date, eventplace, eventinfo, id);
                }
            }
            Con.Close();
            Con.Open();
            //eraisiku ning firma listi loomine.
            List<Companycustomer> complist = new List<Companycustomer>();
            List<Privatecustomer> privlist = new List<Privatecustomer>();
            //andmete võtmine andmebaasist (ühendamine kahe erineva tabeliga)
            sql = "SELECT [yritused].[dbo].[company_customer].[ID], [yritused].[dbo].[company_customer].[name], [yritused].[dbo].[company_customer].[code], [yritused].[dbo].[company_customer].[participants], [yritused].[dbo].[company_customer].[payment_type], [yritused].[dbo].[company_customer].[additionalinfo], [yritused].[dbo].[company_customer].[EventID], [yritused].[dbo].[payment_types].[name] as Pname FROM[yritused].[dbo].[company_customer] INNER JOIN payment_types ON company_customer.payment_type = [yritused].[dbo].[payment_types].[ID] WHERE EventID = @eventid ORDER BY[yritused].[dbo].[company_customer].[ID]";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@eventid", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int customer_id = (int)dr["ID"];
                    string name = (string)dr["name"];
                    long code = (long)dr["code"];
                    int participants = (int)dr["participants"];
                    string payment_type = (string)dr["PName"];
                    string additionalinfo = (string)dr["additionalinfo"];
                    var comp = new Companycustomer(customer_id, name, code, participants, payment_type, additionalinfo);
                    complist.Add(comp);
                }

            }
            Con.Close();
            Con.Open();
            //andmete võtmine andmebaasist (ühendamine kahe erineva tabeliga)
            sql = "SELECT [yritused].[dbo].[private_customer].[ID], [yritused].[dbo].[private_customer].[firstname], [yritused].[dbo].[private_customer].[surname], [yritused].[dbo].[private_customer].[identitynumber], [yritused].[dbo].[private_customer].[payment_type], [yritused].[dbo].[private_customer].[additionalinfo], [yritused].[dbo].[private_customer].[EventID], [yritused].[dbo].[payment_types].[name] as PName FROM[yritused].[dbo].[private_customer] INNER JOIN payment_types ON private_customer.payment_type = [yritused].[dbo].[payment_types].[ID] WHERE EventID = @eventid ORDER BY [yritused].[dbo].[private_customer].[ID]";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@eventid", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int priv_id = (int)dr["ID"];
                    string firstname = (string)dr["firstname"];
                    string surname = (string)dr["surname"];
                    long identitynumber = (long)dr["identitynumber"];
                    string payment_type = (string)dr["PName"];
                    string additionalinfo = (string)dr["additionalinfo"];
                    var priv = new Privatecustomer(priv_id, firstname, surname, identitynumber, payment_type, additionalinfo);
                    privlist.Add(priv);
                }
            }
            Con.Close();
            return View(new ParticipantsModel(e, complist, privlist));
        }

        [HttpPost]
        public IActionResult addprivate(string firstname, string surname, int identitynumber, int payment_type, string additionalinfo, int EventID)
        {
            //andmebaasiga ühenduse loomine
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            //andmebaasi html-i formist saadud andmete lisamine.
            var sql = "INSERT INTO private_customer(firstname, surname, identitynumber, payment_type, additionalinfo, EventID) VALUES(@firstname, @surname, @identitynumber, @payment_type, @additionalinfo, @EventID)";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@identitynumber", identitynumber);
                cmd.Parameters.AddWithValue("@payment_type", payment_type);
                cmd.Parameters.AddWithValue("@additionalinfo", additionalinfo);
                cmd.Parameters.AddWithValue("@EventID", EventID);
                cmd.ExecuteNonQuery();
            }
            //andmebaasiga ühenduse kinni panek
            Con.Close();
            // peale andmete sisestamist suunata tagasi õigele ürituse lehele. olenevalt ID-st
            return RedirectToAction("Eventedit", "Home", new { id = EventID });
        }



        [HttpPost]
        public IActionResult addcompany(string name, int code, int participants, int payment_type, string additionalinfo, int EventID)
        {
            //andmebaasiga ühenduse loomine
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            //andmebaasi html-i formist saadud andmete lisamine.
            var sql = "INSERT INTO company_customer(name, code, participants, payment_type, additionalinfo, EventID) VALUES(@name, @code, @participants, @payment_type, @additionalinfo, @EventID)";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@participants", participants);
                cmd.Parameters.AddWithValue("@payment_type", payment_type);
                cmd.Parameters.AddWithValue("@additionalinfo", additionalinfo);
                cmd.Parameters.AddWithValue("@EventID", EventID);
                cmd.ExecuteNonQuery();
            }
            //andmebaasiga ühenduse kinni panek
            Con.Close();
            // peale andmete sisestamist suunata tagasi õigele ürituse lehele. olenevalt ID-st
            return RedirectToAction("Eventedit", "Home", new { id = EventID });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}