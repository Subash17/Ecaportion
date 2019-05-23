using System;
using System.Collections.Generic;
using System.Text;

namespace Ecaportion
{
   public interface IToastInterface
    {
        void Longtime(string message);
        void Shorttime(string message);
    }
}
