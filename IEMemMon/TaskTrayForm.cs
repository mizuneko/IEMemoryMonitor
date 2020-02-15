using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace IEMemMon
{
    class TaskTrayForm : Form
    {
        public CycleMeasurementMemory counter = new CycleMeasurementMemory();
        NotifyIcon TaskTrayIcon = new NotifyIcon();
        readonly long alertMaxSizeMB = 600;
        readonly long scale = 1000000;  // MegaByte
        readonly string ProcessName = "iexplore";

        public TaskTrayForm()
        {
            this.ShowInTaskbar = false;
            setComponents();
        }

        private void setComponents()
        {
            var alertMaxSize = alertMaxSizeMB * scale;
            TaskTrayIcon.Icon = Properties.Resources.NormalIcon;
            TaskTrayIcon.Visible = true;

            setMenuItem();
            
            var counter = new CycleMeasurementMemory();
            Process[] ps = null;

            counter.Elapsed = (object o, MemoryEventArgs m) =>
            {
                // ツールヒントにはSum値とMax値それぞれ表示するが、アラートの閾値判定はMax値を利用する
                ps = Process.GetProcessesByName(ProcessName);
                long wsSum = 0;
                long wsMax = 0;
                if (ps.Count() > 0)
                {
                    wsSum = ps.Sum(p => p.WorkingSet64) / scale;
                    wsMax = ps.Max(p => p.WorkingSet64) / scale;
                }
                // タスクトレイアイコンには文字数制限があるので注意
                TaskTrayIcon.Text = $"Working Set({ProcessName})\nSum: {wsSum.ToString("#,0")}MB\nMax: {wsMax.ToString("#,0")}MB";

                if (alertMaxSize < wsMax)
                {
                    TaskTrayIcon.Icon = Properties.Resources.AlertIcon;
                }
                else if (wsMax <= alertMaxSize)
                {
                    TaskTrayIcon.Icon = Properties.Resources.NormalIcon;
                }
            };

            counter.Start(TimeSpan.FromMilliseconds(1000));
        }

        private void setMenuItem()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem closeItem = new ToolStripMenuItem();
            closeItem.Text = "&終了";
            closeItem.Click += CloseItem_Click;
            menu.Items.Add(closeItem);

            TaskTrayIcon.ContextMenuStrip = menu;
        }

        private void CloseItem_Click(object sender, EventArgs e)
        {
            counter.Dispose();
            TaskTrayIcon.Dispose();
            Application.Exit();
        }
    }
}
