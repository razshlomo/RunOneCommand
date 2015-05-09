using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace WpfApplication1
{
    public enum SupportedProccess { Putty }

    class command
    {
        private Process proccess = new Process();

        public Process Proccess { get { return proccess; } }

        public command(SupportedProccess proccessName)
        {
            var s = WpfApplication1.Properties.Settings.Default.puttyPath;
        }

        public command(string path, string arg)
        {
            this.proccess.StartInfo.FileName = path;
            this.proccess.StartInfo.Arguments = arg;
            this.proccess.StartInfo.UseShellExecute = false;
            this.proccess.StartInfo.RedirectStandardOutput = true;
            this.proccess.StartInfo.CreateNoWindow = true;
        }
    }

    class ViewModel : DependencyObject
    {
        private Process p = new Process();

        public ViewModel()
        {
            read();
        }

        private void read()
        {


            p.StartInfo.FileName = "ping.exe";
            p.StartInfo.Arguments = "127.0.0.1 -t";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            try
            {
                p.Start();
            }
            catch (Win32Exception e)
            {
                MessageBox.Show("file not found " + Environment.NewLine + e.Message);
                return;
            }

            p.OutputDataReceived += (p_OutputDataReceived);
            p.BeginOutputReadLine();

        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.outputString += e.Data + Environment.NewLine;
            }));

        }

        public string outputString
        {
            get { return (string)GetValue(outputStringProperty); }
            set { SetValue(outputStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for outputString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty outputStringProperty =
            DependencyProperty.Register("outputString", typeof(string), typeof(ViewModel), new PropertyMetadata(string.Empty));


        internal void close()
        {
            this.p.OutputDataReceived -= p_OutputDataReceived;
            this.p.CloseMainWindow();
            
            //this.p.Close();
            // this.p.Dispose();
            
            p.Kill();
            while(!p.HasExited){
                MessageBox.Show("does not exied");
            }
            
        }
    }
}
