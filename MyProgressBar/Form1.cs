using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LongRunningTask;

namespace MyProgressBar
{
    //Наша форма запуска реализует интерфейс оповещения INotifyProgress. Это надо, чтобы форма смогла получать данные о состоянии долгоиграющего процесса
    //ПО МЕРЕ его выполнения. Без реализации интерфейса ничего работать не будет!
    public partial class frmProgressBar : Form, INotifyProgress
    {
        //Класс, выполняющий наш долгоиграющий процесс
        LongRunningTaskPerformer _ltp;
        //Делегат, воспроизводящий внешний вид метода-оповещателя из интерфейса INotifyProgress.
        //Он нужен для обеспечения потокобезопасной передачи данных в форму. Если его не будет, программа вылетит
        //с исключением типа "обращение к progressBar не из его потока" (или что-то подобное).
        delegate void NotifyCallback(int value);

        public frmProgressBar()
        {
            InitializeComponent();
        }

        //Сам процесс запускается с помощью backgroundWorker-а. Кнопка Run только запускает этот backgroundWorker.
        private void btnRun_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Выполнение долгоиграющего процесса. 
            _ltp = new LongRunningTaskPerformer(this);
            _ltp.Run();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Как-то показываем, что у нас завершился наш долгоиграющий процесс и возвращаем прогресс-бар в исходное состояние.
            MessageBox.Show("Process completed!");
            progressBar1.Value = 0;
        }

        //Реализация процедуры оповещения о состоянии долгоиграющего процесса.
        public void Notify(int percent)
        {
            //Чтобы передача состояния процесса сработала корректно и программа не вылетела, 
            //надо делать в точности как здесь (за подробностями - MSDN: 
            //http://msdn.microsoft.com/query/dev11.query?appId=Dev11IDEF1&l=EN-US&k=k%28EHInvalidOperation.WinForms.IllegalCrossThreadCall%29;k%28TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5%29;k%28DevLang-csharp%29&rd=true)

            if (progressBar1.InvokeRequired)
            {
                NotifyCallback d = new NotifyCallback(Notify);
                this.Invoke(d, new object[] { percent });
            }
            else
            {
                progressBar1.Value = percent;
            }
        }
    }
}
