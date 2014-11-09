using LongRunningTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProgressBar
{
    public class NotifyProgress : INotifyProgress
    {
        public int State { get; set; }

        public void Notify(int percent)
        {
            State = percent;
        }
    }
}
