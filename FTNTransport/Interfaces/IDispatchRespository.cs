using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTNTransport.Interfaces
{
    interface IDispatchRespository
    {
        void init();
        Task loadDataFromDatabase(ILogin loginWindow);
    }
}
