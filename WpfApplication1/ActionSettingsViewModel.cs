using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace WpfApplication1
{
    public enum SupportedProccess { Putty }

    class OneCommand
    {
        private Process proccess = new Process();

        public OneCommand(SupportedProccess proccessName)
        {
            var s = WpfApplication1.Properties.Settings.Default.puttyPath;
        }

        public OneCommand(string path, string arg)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            this.proccess.StartInfo.FileName = path;
            this.proccess.StartInfo.Arguments = arg;
            this.proccess.StartInfo.UseShellExecute = false;
            this.proccess.StartInfo.RedirectStandardOutput = true;
            this.proccess.StartInfo.CreateNoWindow = true;
        }

        public int TerminateCommand()
        {
            //this.p.OutputDataReceived -= p_OutputDataReceived;
            this.proccess.CloseMainWindow();
            this.proccess.Kill();
            if (!this.proccess.HasExited)
            {
                MessageBox.Show("does not exied");
            }
            return proccess.ExitCode;
        }

    }

    class ActionSettingsViewModel : DependencyObject
    {
        private Process p = new Process();

        

        public ActionSettingsViewModel()
        {
            
        }

        private void read()
        {
            p.StartInfo.FileName = "ping.exe";
            p.StartInfo.Arguments = "127.0.0.1 -t";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            //razsh
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
            DependencyProperty.Register("outputString", typeof(string), typeof(ActionSettingsViewModel), new PropertyMetadata(string.Empty));


        internal void close()
        {
            this.p.OutputDataReceived -= p_OutputDataReceived;
            try
            {
                this.p.CloseMainWindow();
                p.Kill();
                if (!p.HasExited)
                {
                    MessageBox.Show("does not exied");
                }

            }
            catch(InvalidOperationException e)
            {
                return;
            }

            
        }
    }
}
