using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTNTransport.Interfaces
{
    interface ILogin 
    {
        string getUsername();
        string getPassword();
        long getID();
        bool isLoggedIn();
        Task login();
        void setUsername(string username);
        void setPassword(string password);
        void setID(long id);
        void closeLoginWindow();
    }
}
