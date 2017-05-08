using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTNTransport.Interfaces
{
    interface IOrderRepository
    {
        void init();
        Task loadLegs(Order order);
    }
}
