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

namespace NoticiasHilos_Cliente
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread thread;
        private UdpClient udpClient;
        private string IP_ADDR = "224.2.2.3";
        private int PORT = 8888;
        private string noticia;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click_btn_iniciar(object sender, RoutedEventArgs e)
        {
            udpClient = new UdpClient(PORT);
            udpClient.JoinMulticastGroup(IPAddress.Parse(IP_ADDR));

            thread = new Thread(new ThreadStart(EscucharNoticias));
            thread.Start();
        }   

        private void EscucharNoticias()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    lb_noticias.Content = "¡Bienvenido! Esto es lo ultimo en noticias.";
                });
                
                while (true)
                {
                    var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var data = udpClient.Receive(ref ipEndPoint);

                    Console.WriteLine(Encoding.Default.GetString(data));

                    noticia = Encoding.Default.GetString(data);

                    this.Dispatcher.Invoke(() =>
                    {
                        sp_noticias.Children.Add(CrearNoticia(noticia));
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private UIElement CrearNoticia(string noticia)
        {
            StackPanel stackP = new StackPanel();
            Label label = new Label();

            //Configurar el label
            label.Content = noticia;
            label.HorizontalAlignment = HorizontalAlignment.Center;

            stackP.Children.Add(label);
            stackP.Background = Brushes.Azure;

            return stackP;
        }

        private void Click_btn_salir(object sender, RoutedEventArgs e)
        {
            thread.Abort();

            this.Dispatcher.Invoke(() => {
                sp_noticias.Children.Clear();
                lb_noticias.Content = "Adios!";
            });

        }
    }
}
