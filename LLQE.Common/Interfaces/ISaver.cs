using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLQE.Common.Interfaces
{
    public interface ISaver
    {
        bool SaveResoponse(string responseName, string responeMessage);
    }
}
