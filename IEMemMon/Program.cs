using System;
using System.Threading;
using System.Windows.Forms;

namespace IEMemMon
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            string mutexName = "IEMemMon";
            bool createdNew;
            Mutex mutex = new Mutex(true, mutexName, out createdNew);

            if (createdNew == false)
            {
                MessageBox.Show("多重起動はできません。");
                mutex.Close();
                return;
            }

            try
            {
                TaskTrayForm form = new TaskTrayForm();
                Application.Run();
            }
            finally
            {
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }
    }
}
