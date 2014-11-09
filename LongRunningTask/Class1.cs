using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;//чисто для задержки потоков, т.е. для симуляции долгоиграющего процесса

namespace LongRunningTask
{
    public class LongRunningTaskPerformer
    {
        //Интерфейс оповещения, реализуемый клиентом, которому интересно состояние долгоиграющего процесса.
        //В нашем случае это форма запуска.
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
            //После каждого шага вызываем метод-оповещатель Notify. В случае с формой в реализации этого метода будет обновление прогресс-бара.
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
