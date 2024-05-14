using Admin.Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public string Studio { get; set; }
        public string Poster { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public DateTime ReleaseDate { get; set; } = new(1990, 1, 1);
        public string Category { get; set; }
    }
}