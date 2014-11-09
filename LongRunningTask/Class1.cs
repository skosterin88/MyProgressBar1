using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;//we need this just to simulate a long-running process

namespace LongRunningTask
{
    public class LongRunningTaskPerformer
    {
        //Notification interface which is implemented by client interested in the long-running process state.
        //In our case, the launch form is such a client.
        private readonly INotifyProgress _notifier;

        public LongRunningTaskPerformer(INotifyProgress notifier)
        {
            if (notifier != null)
            {
                _notifier = notifier;
            }
        }

        public void Run()
        {
            //After each step, call the Notify method. 
            //As the Notify method is called, the progress bar on the form is updated.
            DoStep1();
            _notifier.Notify(33);
            DoStep2();
            _notifier.Notify(66);
            DoStep3();
            _notifier.Notify(100);
        }

        private void DoStep1()
        {
            Debug.WriteLine("Step 1");
            Thread.Sleep(12000);
            Debug.WriteLine("Step 1 completed!");
        }

        private void DoStep2()
        {
            Debug.WriteLine("Step 2");
            Thread.Sleep(12000);
            Debug.WriteLine("Step 2 completed!");
        }

        private void DoStep3()
        {
            Debug.WriteLine("Step 3");
            Thread.Sleep(12000);
            Debug.WriteLine("Step 3 completed! Work done.");
        }
    }
}
