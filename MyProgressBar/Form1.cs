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
    //Our launch form is implementing the INotifyProgress notification interface. This is necessary for form to get the status of a long-running task 
    //WHILE IT IS BEING PERFORMED. Nothing will work without the interface implementation!
    public partial class frmProgressBar : Form, INotifyProgress
    {
        //Class that launches our long-running process
        LongRunningTaskPerformer _ltp;
        //A delegate which is similar to notification method declared to INotifyProgress.
        //It is needed to provide thread-safe data transfer to the form. Without it, the program will give 
        //an exception looking like "calling progressBar from another thread" (something like that).
        delegate void NotifyCallback(int value);

        public frmProgressBar()
        {
            InitializeComponent();
        }

        //The process itself is running using a BackgroundWorker. The Run button only launches this BackgroundWorker.
        private void btnRun_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Performing our long-running task. 
            _ltp = new LongRunningTaskPerformer(this);
            _ltp.Run();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Indicating our long-running process completion and bringing the progress bar into the initial state.
            MessageBox.Show("Process completed!");
            progressBar1.Value = 0;
        }

        //The most important part here - implementation of the notification procedure.
        public void Notify(int percent)
        {
            //The implementation must be done exactly as follows so that the program works correctly,
            //without any threading exceptions (read more in MSDN: 
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
