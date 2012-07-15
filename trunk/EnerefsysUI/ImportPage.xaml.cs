using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using BLL;
using System.Net;
using System.Management;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace ControlApp
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        public ImportPage()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string s = "", mac = "";

            string hostInfo = Dns.GetHostName();

            System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;

            for (int i = 0; i < addressList.Length; i++)
            {

                s += addressList[i].ToString();
            }
            ManagementClass mc;

            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            string te="";
            foreach (ManagementObject mo in moc)
            {

                if (mo["IPEnabled"].ToString() == "True")

                    mac = mo["MacAddress"].ToString();

                te+=mac;

            }
            MessageBox.Show(te);

            MessageBox.Show(MachineCode.GetCpuInfo());
            //  button1.Enabled=false;            button2.Focus(); 

        }

        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
       
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            progressBar.Maximum = 10;
            progressBar.Value = 0;

            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(progressBar.SetValue);

            for (int i = 0; i < 10; i++)
            {
                Dispatcher.Invoke(updatePbDelegate, DispatcherPriority.Background,new object[] {ProgressBar.ValueProperty,Convert.ToDouble( i + 1) });
            }  

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel|*.xls|Excel|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
                List<string> sheets = Utility.GetSheetNames(openFileDialog.FileName);

                int sheetsCount = sheets.Count();
                //int iStep = 0;
                progressBar.Maximum = sheetsCount;
                progressBar.Value = 0;

                EngineParam ep = new EngineParam(progressBar,engineType.Text, openFileDialog.FileName);

                DealData(ep);
            }
            
        }


        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel|*.xls|Excel|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
                List<string> sheets = Utility.GetSheetNames(openFileDialog.FileName);

                int sheetsCount = sheets.Count();
                progressBarSan.Maximum = sheetsCount;
                progressBarSan.Value = 0;
                EngineParam ep = new EngineParam(progressBarSan,"", openFileDialog.FileName);

                DealData(ep);
            }
        }

        private void DealData(EngineParam engineParam)
        {
            BackgroundWorker mWorker = new BackgroundWorker();
            mWorker.WorkerReportsProgress = true;
            mWorker.WorkerSupportsCancellation = true;
            if (engineParam.ProgressBar == progressBar)
            {
                mWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
                mWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                mWorker.RunWorkerAsync(engineParam);
                completed.Text = "处理中...";
            }
            else if (engineParam.ProgressBar == progressBarSan)
            {
                mWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWorkSan);
                mWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(worker_ProgressChangedSan);
                mWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompletedSan);
                mWorker.RunWorkerAsync(engineParam);
                completedSan.Text = "处理中...";
            }
            else if (engineParam.ProgressBar == progressBarShui)
            {
                mWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork);
                mWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(worker_ProgressChanged);
                mWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                mWorker.RunWorkerAsync(engineParam);
                completed.Text = "处理中...";
            }
            
        }
        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            EngineParam ep = (EngineParam)e.Argument;
            string fileName = ep.FileName;
            ProgressBar progressBar = ep.ProgressBar;
            string engineType = ep.EngineType;

            //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
            List<string> sheets = Utility.GetSheetNames(fileName);
            //删除所有数据
            int rc = EngineManager.DeleteByType(engineType);

            int iStep = 0;
            foreach (var sheet in sheets)
            {
                Fit.Test test = new Fit.Test();
                MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.MultiPolyfit(fileName, sheet);
                MathWorks.MATLAB.NET.Arrays.MWNumericArray mmArray = mArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                Array array = mmArray.ToArray();
                int ret = EngineManager.Insert((array.GetValue(0, 0)), array.GetValue(1, 0), array.GetValue(2, 0), array.GetValue(3, 0), array.GetValue(4, 0), array.GetValue(5, 0), sheet,engineType);
                if (ret == 1)
                {
                    iStep++;
                    worker.ReportProgress(iStep);
                }
            }
        }

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            message.Text = progressBar.Value + "/" + progressBar.Maximum;
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            completed.Text = "处理完毕";
            // Stop Progressbar updatation  
        }




        private void worker_DoWorkSan(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            EngineParam ep = (EngineParam)e.Argument;
            string fileName = ep.FileName;

            //得到excel中的所有sheet名字，然后循环得到数据模拟表达式,sheet名代表水泵类型
            List<string> sheets = Utility.GetSheetNames(fileName);
            PumpManager.DeletePump();
            int iStep = 0;
            foreach (var sheet in sheets)
            {
                Fit.Test test = new Fit.Test();
           
                //根据dll得到excel中的数据，并插入数据库
                MathWorks.MATLAB.NET.Arrays.MWArray mwArray = test.GetFND(fileName, sheet);
                MathWorks.MATLAB.NET.Arrays.MWNumericArray mmwArray = mwArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                Array warray = mmwArray.ToArray();
                double[,] cc=(double[,])warray;

                int j_count = warray.Length / 3;

                for (int j = 0; j < j_count; j++)
                {
                    PumpManager.InsertPumpInfo(warray.GetValue(j,0),warray.GetValue(j,1),sheet,warray.GetValue(j,2));
                }

                //根据dll得到拟合出来的二次项系数
                MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.SingleDimensionPolyFit(fileName, sheet);
                MathWorks.MATLAB.NET.Arrays.MWNumericArray mmArray = mArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                Array array = mmArray.ToArray();
                int ret = PumpManager.Insert((array.GetValue(0, 0)), array.GetValue(0, 1), array.GetValue(0,2), sheet);
                if (ret == 1)
                {
                    iStep++;
                    worker.ReportProgress(iStep);
                }
            }
        }

        private void worker_ProgressChangedSan(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBarSan.Value = e.ProgressPercentage;
            messageSan.Text = progressBarSan.Value + "/" + progressBarSan.Maximum;
        }

        private void worker_RunWorkerCompletedSan(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            completedSan.Text = "处理完毕";
            // Stop Progressbar updatation  
        }





        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //淡入效果
            this.Opacity = 0;
            DoubleAnimation OpercityAnimation =
            new DoubleAnimation(0.01, 1.00, new Duration(TimeSpan.FromSeconds(1)));
            this.BeginAnimation(Page.OpacityProperty, OpercityAnimation);
        }

        
        class EngineParam
        {
            public string EngineType { get; set; }
            public string FileName { get; set; }
            public ProgressBar ProgressBar { get; set; }
            public EngineParam(ProgressBar progressbar,string engineType, string fileName)
            {
                ProgressBar = progressbar;
                EngineType = engineType;
                FileName = fileName;
            }
        }
    }
}
