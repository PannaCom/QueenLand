using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using QueenLand.Models;
namespace QueenLand
{
    public class Config
    {
        
        public static string domain ="http://queenland.vn";//"http://localhost:55773/";// "http://queenland.vn";
        public static int PageSize =5;
        public static string HotelImagePath = "/Images/Hotels";
        public static string NewsImagePath = "/Images/News";
        public static string JobsImagePath = "/Images/Jobs";
        public static string GolfImagePath = "/Images/golf";
        public static string SlideImagePath = "/Images/slides";
        public static string ProjectImagePath = "/Images/Projects";
        public static string ChouponImagePath = "/Images/choupon";
        public static string LogoImagePath = "/Images";
        public static int imgWidthHotelRoom = 750;
        public static int imgHeightHotelRoom = 481;
        public static int imgWidthProject = 800;
        public static int imgHeightProject = 504;
        public static int imgWidthGolf = 800;
        public static int imgHeightGolf = 504;
        public static int imgWidthNews = 300;
        public static int imgHeightNews = 200;
        public static int imgWidthBigSlide = 1700;
        public static int imgHeightBigSlide = 449;
        public static int imgWidthSmallSlide = 600;
        public static int imgHeightSmallSlide = 90;
        public static int imgWidthLogo = 635;
        public static int imgHeightLogo = 294;
        public static int imgWidthLogo2 = 1224;//for mobile
        public static int imgHeightLogo2 = 313;
        public static string imgSlideWidth = "292px";
        private static queenlandEntities db = new queenlandEntities();
        public static int datetimeid()
        {
            DateTime d1;
            try
            {
                
                d1 = DateTime.Now;//.ToUniversalTime();
                string rs = d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
                return int.Parse(rs);

            }
            catch (Exception ex)
            {
                d1 = DateTime.Now;//.ToUniversalTime();
                string rs = d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
                return int.Parse(rs);
            }
            //return DateTime.Now.Year * 365 + DateTime.Now.Month * 30 + DateTime.Now.Day;
        }
        public static int datetimeidaddday(int days)
        {
            DateTime d1;
            try
            {

                d1 = DateTime.Now.AddDays(days);//.ToUniversalTime();
                string rs = d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
                return int.Parse(rs);

            }
            catch (Exception ex)
            {
                d1 = DateTime.Now;//.ToUniversalTime();
                string rs = d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
                return int.Parse(rs);
            }
            //return DateTime.Now.Year * 365 + DateTime.Now.Month * 30 + DateTime.Now.Day;
        }
        //convert longitude latitude to geography
        public static DbGeography CreatePoint(double? latitude, double? longitude)
        {
            if (latitude == null || longitude == null) return null;
            latitude = (double)latitude;
            longitude = (double)longitude;
            return DbGeography.FromText(String.Format("POINT({1} {0})", latitude, longitude));
        }
        public static string convertToDateTimeId(string d)
        {
            DateTime d1;
            try
            {
                d1 = DateTime.Parse(d);//ToUniversalTime();
                return d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
            }
            catch (Exception ex)
            {
                d1 = DateTime.Now;
                return d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
            }
            d1 = DateTime.Now;
            return d1.Year.ToString() + d1.Month.ToString("00") + d1.Day.ToString("00");
        }
        public static string convertFromDateIdToDateString(string sDate) {
            if (sDate == null || sDate == "") return "";
            sDate = sDate.Substring(6, 2) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(0, 4);
            return sDate;
        }
        public static string convertFromDateIdToDateString2(string sDate)
        {
            if (sDate == null || sDate == "") return "";
            sDate = sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2);
            return sDate;
        }
        public static int getDateDiff(string sDate1,string sDate2)
        {
            int totaldays = 0;
            try
            {
                if (sDate1 == null || sDate1 == "") return 0;
                if (sDate2 == null || sDate2 == "") return 0;
                sDate1 = sDate1.Substring(0, 4) + "-" + sDate1.Substring(4, 2) + "-" + sDate1.Substring(6, 2);
                sDate2 = sDate2.Substring(0, 4) + "-" + sDate2.Substring(4, 2) + "-" + sDate2.Substring(6, 2);
                DateTime d1 = DateTime.Parse(sDate1);
                DateTime d2 = DateTime.Parse(sDate2);
                TimeSpan TS = new System.TimeSpan(d2.Ticks - d1.Ticks);
                totaldays = (int)TS.TotalDays;
            }
            catch (Exception ex) {
                return 0;
            }
            return totaldays;
        }
        public static string displayHotelRate(int rate) {
            string rs = "";
            for (int i = 0; i < rate; i++) {
                rs += "<i class=\"fa fa-star-o\" style=\"color:#A22EB4\"></i>";
            }
            return rs;
        }
        public static string displayTagNews(string keyword)
        {
            if (keyword.Trim().Equals("")) return "";
            string[] temp = keyword.Split(',');
            string rs="";
            for (int i = 0; i < temp.Length; i++)
            {
                if (!temp[i].Trim().Equals(""))
                {
                    rs += "<a href=\"/news/" + temp[i] + "-1\" style=\"display:block;possition:relative;\">" + temp[i] + "</a>";
                }
            }
            if (rs.Trim().Equals("")) return "";
            return rs;
        }
        //convert tieng viet thanh khong dau va them dau -
        public static string unicodeToNoMark(string input)
        {
            input = input.ToLowerInvariant().Trim();
            if (input == null) return "";
            string noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
            string unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
            string[] a_n = noMark.Split(',');
            string[] a_u = unicode.Split(',');
            for (int i = 0; i < a_n.Length; i++)
            {
                input = input.Replace(a_u[i], a_n[i]);
            }
            input = input.Replace("  ", " ");
            input = Regex.Replace(input, "[^a-zA-Z0-9% ._]", string.Empty);
            input = removeSpecialChar(input);
            input = input.Replace(" ", "-");
            input = input.Replace("--", "-");
            return input;
        }
        public static string unicodeToNoMarkCat(string input)
        {
            input = input.ToLowerInvariant().Trim();
            if (input == null) return "";
            string noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
            string unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
            string[] a_n = noMark.Split(',');
            string[] a_u = unicode.Split(',');
            for (int i = 0; i < a_n.Length; i++)
            {
                input = input.Replace(a_u[i], a_n[i]);
            }
            input = input.Replace("  ", " ");
            input = Regex.Replace(input, "[^a-zA-Z0-9% ._]", string.Empty);
            input = removeSpecialChar(input);
            input = input.Replace(" ", "");
            input = input.Replace("--", "-");
            return input;
        }
        
        public static string removeSpecialChar(string input)
        {
            input = input.Replace("-", "").Replace(":", "").Replace(",", "").Replace("_", "").Replace("'", "").Replace("\"", "").Replace(";", "").Replace("”", "").Replace(".", "").Replace("%", "");
            return input;
        }
        public static string smoothDes(string des) {
            if (des.Length >= 155) {
                des = des.Substring(0, 154)+"..";
            }
            return des;
        }
        public static string mail(string from, string to,string topic, string pass, string content) {
            try
            {
                var fromAddress = from;
                var toAddress = to;
                //Password of your gmail address
                string fromPassword = pass;
                // Passing the values and make a email formate to display
                string subject = topic;
                string body = content;
                //body += "Email: " + fromEmailAddress + "\n";
                //body += "Về việc: " + subject + "\n";
                //body += "Nội dung: \n" + content + "\n";
                // smtp settings
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromAddress);
                message.To.Add(toAddress);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;                    
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                // Passing values to smtp object
                smtp.Send(message);
            }
            catch (Exception ex) {
                return "-1";
            }
            return "ok";
        }
        public static string formatNumber(int number) {
            if (number == null || number == 0) return "";
            return number.ToString("##,#");
        }
        public static string genCode(){
            Random rnd = new Random();
            string[] temp = new string[] { "a", "b", "c", "d", "e", "f", "g","h","i", "j", "k", "l", "m", "n", "o", "p", "q", "r","s","t","u","v","w","x","y","z"};
            string[] temp2 = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            int a = rnd.Next(0,26); // creates a number between 1 and 25
            int b = rnd.Next(0,10); // creates a number between 1 and 9
            int c = rnd.Next(0,26);
            int d = rnd.Next(0, 10);
            int e = rnd.Next(0, 26);
            string rs=temp[a]+temp2[b]+temp[c]+temp2[d]+temp[e];
            return rs;
        }
        public static string getMonth(DateTime d) {
            return d.Month.ToString("00");
        }
        public static string getDate(DateTime d)
        {
            return d.Day.ToString("00");
        }
        public static string displayNewsDate(DateTime d) { 
            return "<div class=\"meta-date\"><span>"+getDate(d)+"/</span>"+getMonth(d)+"</div>";
        }
        public static string getTime(DateTime d) {
            return d.Hour.ToString("00") + ":" + d.Second.ToString("00");
        }
        public static string formatDateVn(DateTime d) {
            return d.Day.ToString("00") + "/" + d.Month.ToString("00") + "/" + d.Year.ToString();
        }
        public static string convertToDateTime(string s) {
            try
            {
                DateTime d = DateTime.Parse(s);
                return d.Month.ToString("00") + "/" + d.Day.ToString("00") + "/"+d.Year.ToString("0000") + " " + d.Hour.ToString("00") + ":" + d.Minute.ToString("00");
            }
            catch (Exception ex) {
                return s;
            }
            return s;
        }
        public static bool isWeekendDate(int date) {
            string sdate = convertFromDateIdToDateString2(date.ToString());
            try
            {
                DateTime d = DateTime.Parse(sdate);
                if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday) return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
        public static void setCookie(string field,string value){
            HttpCookie MyCookie = new HttpCookie(field);
            MyCookie.Value = value;
            MyCookie.Expires = DateTime.Now.AddDays(365);
            HttpContext.Current.Response.Cookies.Add(MyCookie);
            //Response.Cookies.Add(MyCookie);           
        }
        public static string getCookie(string v) { 
            try{
                return HttpContext.Current.Request.Cookies[v].Value.ToString();
            }catch(Exception ex){
                return "";
            }
        }
        public static string getMonthFromDateId(int dateid) {
            try
            {
                return int.Parse(dateid.ToString().Substring(4, 2)).ToString();
            }
            catch (Exception ex) {
                return DateTime.Now.Month.ToString();
            }
        }
        public static string getListProjectMenu() {
            try
            {
                var p = (from q in db.projects select q).OrderBy(o => o.no).ThenByDescending(o=>o.id).Take(20);
                var rs=p.ToList();
                string menu = "";
                for (int i = 0; i < rs.Count; i++) {
                    menu += "<li><a href=\"" + Config.domain + "/projects/" + Config.unicodeToNoMark(rs[i].name) + "-" + rs[i].id + "\" style=\"width:235px;font-weight:bold;color:#ffffff;background-color:#000080;\">" + rs[i].name.ToUpperInvariant() + "</a></li>";
                }
                return menu;
            }
            catch (Exception ex) {
                return "";
            }
        }
        public static string getProjectMenu(){
            string menuleft = "";
            try
            {
                //Lấy ra menu bên trái
                var mn = (from p in db.projects
                          join q in db.projectitems on p.id equals q.projectid
                          orderby p.id
                          select new
                          {
                              no=p.no,
                              image = p.image,
                              projectid = p.id,
                              projectname = p.name,
                              q.itemname,
                              itemid = q.id
                          }).OrderBy(o => o.no).ThenByDescending(o=>o.projectid).ToList();

                
                string link = "";
                string preMenu = "";//Mỗi Menu có nhiều Menu item khác nhau, do vậy đọc lần lượt nếu sang Menu mới thì cập nhật item
                for (int i = 0; i < mn.Count; i++)
                {
                    if (mn[i].projectname != preMenu)
                    {
                        preMenu = mn[i].projectname;
                        link = "/projects/" + Config.unicodeToNoMark(mn[i].projectname) + "-" + mn[i].projectid;
                        menuleft += "<div><a href=\"" + link + "\"><b>" + mn[i].projectname.ToUpperInvariant() + "</b></a></div>";
                    }
                    link = "/projects/" + Config.unicodeToNoMark(mn[i].itemname) + "/" + Config.unicodeToNoMark(mn[i].projectname) + "-" + mn[i].itemid;
                    menuleft += "<div>&nbsp;&nbsp;-<a href=\"" + link + "\">" + mn[i].itemname.ToUpperInvariant() + "</a></div>";
                }
            }
            catch (Exception ex) {
                return "";
            }
            return  menuleft;
        }
        public static string getHotLine() {
            try
            {
                var p = db.addresses.FirstOrDefault().hotline;
                var res = p.Split('/');
                var content = "<div ><a href=\"#\" class=\"hotlinetext\"><i class=\"fa fa-phone\"></i><font style=\"margin-top:1px;\" class=\"hotlinetext\">Hotline:</font> <B style=\"margin-top:1px;\" class=\"hotlinephone\">" + res[0] + "</b></a> </div>";
                if (res.Length >= 2) content += "<div  style=\"margin-top:8px;\"><B class=\"hotlinephone2\">" + res[1] + "</b></div>";

                return content;
            }
            catch (Exception ex) {
                return "<b>HotLine: 0904267887</b>";
            }
        }
        public static string getAddressCompany()
        {
            try
            {
                var p = db.addresses.FirstOrDefault().addresscontent;
                return p;
            }
            catch (Exception ex)
            {
                return "<b>Địa chỉ: tòa nhà tower 1, sàn GD BĐS Times City, 458 Minh Khai, Hà Nội</b>";
            }
        }
    }
}