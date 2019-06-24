using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace NP_HW_Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private Thread listenThread;
        private Thread sendThread;
        private bool buttonIsEnabled;
        public MainWindow()
        {
            InitializeComponent();
            buttonIsEnabled = false;
        }

        private void ConnectedButton(object sender, RoutedEventArgs e)
        {
            if (!buttonIsEnabled)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(IPAddress.Parse(ipAddress.Text), int.Parse(port.Text));
                    listenThread = new Thread(LisetenThreadProc);
                    listenThread.Start(client);
                    buttonIsEnabled = true;
                    connect.Content = "Stop";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                client.Close();
            }
        }
        public void LisetenThreadProc(object obj)
        {
            var client = obj as TcpClient;
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var reciveSize = client.Client.Receive(buffer);
                var messageAs = Encoding.UTF8.GetString(buffer, 0, reciveSize);


                Dispatcher.Invoke(() => message.AppendText($"{Encoding.UTF8.GetString(buffer, 0, reciveSize)} \n"));
            }
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            try
            {
                sendThread = new Thread(SendThreadProc);
                sendThread.Start(client);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendThreadProc(object obj)
        {
            var client = obj as TcpClient;
            string messageToServer = "";
            Dispatcher.Invoke(() => messageToServer = index.Text);
            client.Client.Send(Encoding.ASCII.GetBytes(messageToServer));
        }
    }
}
