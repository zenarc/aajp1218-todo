using System;

namespace todo
{
    public class AppConfiguration
    {
        public string Endpoint { get; set; }
        public string Key { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
        public string Region { get; set; }
    }
}
