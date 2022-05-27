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


        [HttpPost]
        public IActionResult eventadd(string name, string place, string time, string additionalinfo)
        {
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
            return RedirectToAction("index", "Home");
        }


        [HttpGet]
        public IActionResult deleteevent(string id)
        {
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            var sql = "DELETE FROM event WHERE eventID = @eventID";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@eventID", id);
                cmd.ExecuteNonQuery();
            }


            Con.Close();
            return RedirectToAction("index", "Home");
        }


        [HttpGet]
        public IActionResult Eventedit(int id)
        {
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Event e;
            Con.Open();
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
            return View();

        }

        [HttpPost]

        public IActionResult addprivate(string firstname, string surname, int identitynumber, int payment_type, string additionalinfo)
        {
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();

            var sql = "INSERT INTO private_customer(firstname, surname, identitynumber, payment_type, additionalinfo) VALUES(@firstname, @surname, @identitynumber, @payment_type, @additionalinfo)";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@identitiynumber", identitynumber);
                cmd.Parameters.AddWithValue("@payment_type", payment_type);
                cmd.Parameters.AddWithValue("@additionalinfo", additionalinfo);
                cmd.ExecuteNonQuery();
            }

            Con.Close();
            return View();


        }



        [HttpPost]
        public IActionResult addcompany(string name, int code, int people, int payment_type, string additionalinfo)
        {
            SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-EBLLC22\SQLEXPRESS01;Initial Catalog=yritused;Integrated Security=True");
            Con.Open();
            var sql = "INSERT INTO company_customer(name, code, participants, payment_type, additionalinfo) VALUES(@name, @code, @participants, @payment_type, @additionalinfo)";
            using (var cmd = new SqlCommand(sql, Con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@participants", people);
                cmd.Parameters.AddWithValue("@payment_type", payment_type);
                cmd.Parameters.AddWithValue("@eventinfo", additionalinfo);
                cmd.ExecuteNonQuery();
            }
            Con.Close();
            return View();

        }
















        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}