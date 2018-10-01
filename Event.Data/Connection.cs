namespace Event.Data
{
    using System.Configuration;

    public class Connection
    {
        public string VideoStatusConnection = ConfigurationManager.AppSettings["DefaultConnection"];
    }
}