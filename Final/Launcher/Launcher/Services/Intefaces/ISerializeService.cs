using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Services.Intefaces
{
    public interface ISerializeService
    {
        string Serialize<T>(object obj);
        T Deserialize<T>(string json);
    }
}
