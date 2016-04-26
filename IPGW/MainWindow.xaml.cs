using IPGW.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPGW
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            threadOfLoadStart();

        }

        private Thread threadOfLoad = null;

        private void threadOfLoadImpl()
        {
            try
            {
                string error = null;
                string username = UConfig.get("username", ref error);
                string password = UConfig.get("password", ref error);
                if (error != null)
                {
                    setListBoxOfInfo(new string[] { error });
                    return;
                }
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return;
                }
                setTextBoxOfUser(username);
                setPasswordBoxOfPsw(password);
            }
            catch (Exception e)
            {
                setListBoxOfInfo(new string[] { e.Message });
            }
            finally
            {
                setListBoxOfInfo(new string[]{"By.","QQ","183515951","CAOLEI 作品."});
                setMainWindow(true);
            }
        }

        private void threadOfLoadStart()
        {
            this.IsEnabled = false;
            threadOfLoad = new Thread(threadOfLoadImpl);
            threadOfLoad.IsBackground = true;
            threadOfLoad.Start();
        }

        private delegate void setListBoxOfInfoGate(string[] items);

        private void setListBoxOfInfo(string[] items)
        {
            if (ListBoxOfInfo.Dispatcher.Thread != Thread.CurrentThread)
            {
                setListBoxOfInfoGate sg = new setListBoxOfInfoGate(setListBoxOfInfo);
                Dispatcher.Invoke(sg, new object[] { items });
            }else
	        {
                ListBoxOfInfo.ItemsSource = null;
                ListBoxOfInfo.ItemsSource = items;
	        }
        }

        private delegate void setTextBoxOfUserGate(string text);

        private void setTextBoxOfUser(string text)
        {
            if (TextBoxOfUser.Dispatcher.Thread != Thread.CurrentThread)
            {
                setTextBoxOfUserGate sg = new setTextBoxOfUserGate(setTextBoxOfUser);
                Dispatcher.Invoke(sg, new object[] { text });
            }
            else
            {
                TextBoxOfUser.Text = text;
            }
        }

        private delegate void setPasswordBoxOfPswGate(string text);

        private void setPasswordBoxOfPsw(string text)
        {
            if (PasswordBoxOfPsw.Dispatcher.Thread != Thread.CurrentThread)
            {
                setPasswordBoxOfPswGate sg = new setPasswordBoxOfPswGate(setPasswordBoxOfPsw);
                Dispatcher.Invoke(sg, new object[] { text });
            }
            else
            {
                PasswordBoxOfPsw.Password = text;
            }
        }

        private delegate void setButtonOfConnectGate(bool flag);

        private void setButtonOfConnect(bool flag)
        {

            if (ButtonOfConnect.Dispatcher.Thread != Thread.CurrentThread)
            {
                setButtonOfConnectGate sg = new setButtonOfConnectGate(setButtonOfConnect);
                Dispatcher.Invoke(sg, new object[] { flag});
            }
            else
            {
                ButtonOfConnect.IsEnabled = flag;
            }
        }

        private delegate void setButtonOfSaveGate(bool flag);

        private void setButtonOfSave(bool flag)
        {

            if (ButtonOfSave.Dispatcher.Thread != Thread.CurrentThread)
            {
                setButtonOfSaveGate sg = new setButtonOfSaveGate(setButtonOfSave);
                Dispatcher.Invoke(sg, new object[] { flag });
            }
            else
            {
                ButtonOfSave.IsEnabled = flag;
            }
        }

        private delegate void setButtonOfStopGate(bool flag);

        private void setButtonOfStop(bool flag)
        {

            if (ButtonOfStop.Dispatcher.Thread != Thread.CurrentThread)
            {
                setButtonOfStopGate sg = new setButtonOfStopGate(setButtonOfStop);
                Dispatcher.Invoke(sg, new object[] { flag });
            }
            else
            {
                ButtonOfStop.IsEnabled = flag;
            }
        }

        private delegate void setMainWindowGate(bool flag);

        private void setMainWindow(bool flag)
        {
            if (ButtonOfConnect.Dispatcher.Thread != Thread.CurrentThread)
            {
                setMainWindowGate sg = new setMainWindowGate(setMainWindow);
                Dispatcher.Invoke(sg, new object[] { flag });
            }
            else
            {
                this.IsEnabled = flag;
            }
        }

        private string username = null;
        private string password = null;

        private Thread threadOfConnect = null;

        private void threadOfConnectImpl()
        {
            string error = null;
            string[] lines = UWeb.connect(username, password, ref error);
            if (error != null)
            {
                setListBoxOfInfo(new string[] { error });
                setMainWindow(true);
                return;
            }
            setListBoxOfInfo(lines);
            setMainWindow(true);
        }

        private void threadOfConnectStart()
        {
            this.IsEnabled = false;
            threadOfConnect = new Thread(threadOfConnectImpl);
            threadOfConnect.IsBackground = true;
            threadOfConnect.Start();
        }

        private bool getUser()
        {
            string username = TextBoxOfUser.Text;
            string password = PasswordBoxOfPsw.Password;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                setListBoxOfInfo(new string[] { "用户名和密码不能为空" });
                return false;
            }
            this.username = username;
            this.password = password;
            return true;
        }

        private void ButtonOfConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!getUser())
            {
                return;
            }
            threadOfConnectStart();
        }

        private Thread threadOfStop = null;

        private void threadOfStopImpl()
        {
            string error = null;
            string[] lines = UWeb.logout(username, password, ref error);
            if (error != null)
            {
                setListBoxOfInfo(new string[] { error });
                setMainWindow(true);
                return;
            }
            setListBoxOfInfo(lines);
            setMainWindow(true);
        }

        private void threadOfStopStart()
        {
            this.IsEnabled = false;
            threadOfStop = new Thread(threadOfStopImpl);
            threadOfStop.IsBackground = true;
            threadOfStop.Start();
        }

        private void ButtonOfStop_Click(object sender, RoutedEventArgs e)
        {
            if (!getUser())
            {
                return;
            }
            threadOfStopStart();
        }

        private Thread threadOfSave = null;

        private void threadOfSaveImpl()
        {
            string error = null;
            UConfig.add("username", username, ref error);
            UConfig.add("password", password, ref error);
            if (error != null)
            {
                setListBoxOfInfo(new string[] { error });
                setMainWindow(true);
                return;
            }
            setListBoxOfInfo(new string[] { "保存成功" });
            setMainWindow(true);
        }

        private void threadOfSaveStart()
        {
            this.IsEnabled = false;
            threadOfSave = new Thread(threadOfSaveImpl);
            threadOfSave.IsBackground = true;
            threadOfSave.Start();
        }

        private void ButtonOfSave_Click(object sender, RoutedEventArgs e)
        {
            if (!getUser())
            {
                return;
            }
            threadOfSaveStart();
        }

    }
}
