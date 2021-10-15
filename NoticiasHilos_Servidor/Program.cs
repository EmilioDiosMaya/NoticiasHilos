using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NoticiasHilos_Servidor
{
	class send
	{
		send(string mcastGroup, string port, string ttl, string rep)
		{
			String[] noticias = {"Perro corre y choca con vidrio en Tuxtla", "Niña hace casa para hormigas",
			"Salio a comprar vino y ya no vino", "Hombre agarrando señal ante transito", "GTA6 para el viernes",
			"Spiderman ¿Heroé o villano?", "Pico de Orizaba es ahora Pico de Puebla xd"};
			IPAddress ip;

			try
			{
				Console.WriteLine("Multicast envia al Grupo: {0} Port: {1} TTL: {2}", mcastGroup, port, ttl);
				ip = IPAddress.Parse(mcastGroup);

				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

				socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));

				socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, int.Parse(ttl));

				IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(mcastGroup), int.Parse(port));

				Console.WriteLine("Connecting...");

				socket.Connect(ipep);

				for (int x = 0; x < noticias.Length; x++)
				{
					Console.WriteLine("Enviando: {0}", noticias[x]);
					Thread.Sleep(10000);
					socket.Send(Encoding.ASCII.GetBytes(noticias[x]), Encoding.ASCII.GetBytes(noticias[x]).Length, SocketFlags.None);
				}

				Console.WriteLine("Closing Connection...");
				socket.Close();
			}
			catch (System.Exception e) { Console.Error.WriteLine(e.Message); }
		}

		static void Main(string[] args)
		{

			new send("224.2.2.3", "8888", "1", "2");
		}
	}
}