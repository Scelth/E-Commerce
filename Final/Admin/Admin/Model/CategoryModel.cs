using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Admin.Model
{
    public class CategoryModel
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string NewName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}