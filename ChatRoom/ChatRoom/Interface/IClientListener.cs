using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Interface
{
    public interface IClientListener
    {
        void ReceiveData();
    }
}
