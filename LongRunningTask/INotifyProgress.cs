using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongRunningTask
{
    public interface INotifyProgress
    {
        void Notify(int percent);
    }
}
