using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace MessangerClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverPoint;
        //const string serverAddress = "127.0.0.1";
        //const short serverPort = 4040;
        TcpClient client;
        ObservableCollection<MessageInfo> messages ;
        NetworkStream ns = null;
        StreamReader sr = null;
        StreamWriter sw = null;
        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
            string address = ConfigurationManager.AppSettings["ServerAddress"]!;
            short port = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverPoint = new IPEndPoint(IPAddress.Parse(address), port);
            messages = new ObservableCollection<MessageInfo>();
            this.DataContext = messages;            
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {           
            string message = msgTextBox.Text;
            sw.WriteLine(message);  
            sw.Flush(); 
        }
        private  void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(serverPoint);
                ns = client.GetStream();

                sw = new StreamWriter(ns);  
                sr = new StreamReader(ns);  
                Listen();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
          
        }
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            ns.Close();
            client.Close();
        }
        private async void Listen()
        {
 
            while (true)
            {
                //string = null  ---> value = not null
                string? message = await sr.ReadLineAsync();               
                messages.Add(new MessageInfo(message));
            }
           
        }       
    }
    public class MessageInfo
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public MessageInfo(string? text)
        {
            this.Text = text?? "";   
            Time = DateTime.Now;                
        }
        public override string ToString()
        {
            return $"{Text}  {Time.ToShortDateString()}";
        }

    }
}