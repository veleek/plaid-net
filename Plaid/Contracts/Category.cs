using System.Collections.Generic;

namespace Ben.Plaid
{
    public class Category
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public List<string> Hierarchy { get; set; }
    }
}