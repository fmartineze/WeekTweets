using System;
using TinyTwitter_;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Net;


namespace WeekTWeets
{
    public class twitter_cfg
    {
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }

    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ WeekTweets (Weekly Scheduler Tweets)  || PCpractico.es (2016)        ║");
            Console.WriteLine("║ Developed by Francisco Mártinez                                      ║");
            Console.WriteLine("║ Twitter : @pcpracticoes    GitHub: https://github.com/fmartineze     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");


            bool Terror = false;

            //Dia de la semana
            string dia = "";
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday": { dia = "L"; break; }
                case "Tuesday": { dia = "M"; break; }
                case "Wednesday": { dia = "X"; break; }
                case "Thursday": { dia = "J"; break; }
                case "Friday": { dia = "V"; break; }
                case "Saturday": { dia = "S"; break; }
                case "Sunday": { dia = "D"; break; }
            }
            if (!File.Exists(@Directory.GetCurrentDirectory() + "\\twitter_cfg.json")) {
                // si no existe el archivo crea uno con los datos esenciales.
                twitter_cfg cfgfile = new twitter_cfg
                {
                    AccessToken = "Twitter AccessToken",
                    AccessSecret = "Twitter AccessSecret",
                    ConsumerKey = "Twitter ConsumerKey",
                    ConsumerSecret = "Twitter ConsumerSecret"
                };
                File.WriteAllText(@Directory.GetCurrentDirectory() + "\\twitter_cfg.json", JsonConvert.SerializeObject(cfgfile));
                Terror = true; }

            if (!File.Exists(@Directory.GetCurrentDirectory() + "\\schedule.json")) {
                // si no existe el archivo crea uno con los datos demo.
                string schedule_str = @"{
   'msgs': [
    {
      'dayweek': 'LMXJVSD',
      'time': '9:00:00',
      'msg': 'Tweet de prueba / Test Tweet',
      'last': '01/01/16',
      'count': '5'
    },
    {
      'dayweek': 'L',
      'time': '9:00:00',
      'msg': 'Tweet de prueba / Test Tweet',
      'last': '02/01/16',
      'count': '5'
    },
  ]
}";
                File.WriteAllText(@Directory.GetCurrentDirectory() + "\\schedule.json", schedule_str);

                Terror = true; }

            if (!Terror)
            {
                // Leer twitter_cfg.json para obtener las credenciales.
                JObject Json_settings = JObject.Parse(File.ReadAllText(@Directory.GetCurrentDirectory() + "\\twitter_cfg.json"));

                // Leer Schedule.json para programar los Tweets programados.
                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(File.ReadAllText(@Directory.GetCurrentDirectory() + "\\schedule.json"));
                DataTable dataTable = dataSet.Tables["msgs"];

                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["dayweek"].ToString().IndexOf(dia) != -1) // Comparar dia de la semana
                    {
                        if (DateTime.Parse(row["time"].ToString()) < DateTime.Now) // Comparar hora de ejecución (>=)
                        {
                            string fecha_last = DateTime.Parse(row["last"].ToString()).ToString("dd/MM/yy");
                            string fecha_now = DateTime.Now.ToString("dd/MM/yy");
                            if (DateTime.Parse(fecha_last) < DateTime.Parse(fecha_now)) // comparar ultima fecha que se ejecutó
                            {
                                if (Int32.Parse(row["count"].ToString()) != 0) // comparar ejecuciones pendientes
                                {
                                    Console.WriteLine("- Tweet : [" + row["dayweek"] + "|" + row["time"] + "|" + row["count"] + "] " + row["msg"]);
                                    Enviar_Tweet(row["msg"].ToString(), Json_settings["AccessToken"].ToString(), Json_settings["AccessSecret"].ToString(), Json_settings["ConsumerKey"].ToString(), Json_settings["ConsumerSecret"].ToString());
                                    row["last"] = fecha_now; // ;Modificar "last" a date/time actual.
                                    if (Int32.Parse(row["count"].ToString()) > 0) // Cambiar contador si es > 0
                                    {
                                        row["count"] = Int32.Parse(row["count"].ToString()) - 1;
                                    }
                                }
                            }
                        }
                    }
                }


                // Serialize Dataset to Json  -> http://www.newtonsoft.com/json/help/html/SerializeDataSet.htm
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                File.WriteAllText(@Directory.GetCurrentDirectory() + "\\schedule.json", json); // Grabar Cambios
            } else
            {
                Console.WriteLine("- ERROR: Archivos de configuración no encontrados (twitter_cfg.json o schedule.json).");
                Console.WriteLine("  Se han creado los siguientes archivos:");
                Console.WriteLine("  twitter_cfg.json    -> Configuración de Token (https://apps.twitter.com/)");
                Console.WriteLine("  schedule.json       -> Programador de tareas");
            }

        }
        static void Enviar_Tweet (string msg_tweet, string AT, string AS, string CK, string CS)
        {
            var oauth = new OAuthInfo
            {
                AccessToken = AT,
                AccessSecret = AS,
                ConsumerKey = CK,
                ConsumerSecret = CS
            };

            var twitter = new TinyTwitter(oauth);
            try
            {
                twitter.UpdateStatus(msg_tweet);
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("- ERROR DESCONOCIDO :"+ e);
            }

        }
    }   
}
