using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerForChat
{
  public  class ServerWork
    {
        private List<Socket> sockets = new List<Socket>();


       

        public Task ClientJoin(int sokIndx)
        {
            return Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        int bytes;
                        byte[] buffer = new byte[1024];
                        StringBuilder stringBuilder = new StringBuilder();

                        do
                        {
                            bytes = sockets[sokIndx].Receive(buffer);
                            stringBuilder.Append(Encoding.Default.GetString(buffer));
                        }
                        while (sockets[sokIndx].Available > 0);

                        Message newMessage = JsonConvert.DeserializeObject<Message>(stringBuilder.ToString());
                        if (newMessage.Letter == "start")
                        {
                            Console.WriteLine("Присоединился " + newMessage.Name);
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                if (i != sokIndx)
                                {
                                    sockets[i].Send(Encoding.Default.GetBytes("Присоединился " + newMessage.Name));
                                }
                            }
                        }
                        else if (newMessage.Letter == "4")
                        {
                            Console.WriteLine("Нас покинул " + newMessage.Name);
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                if (i != sokIndx)
                                {
                                    sockets[i].Send(Encoding.Default.GetBytes("Нас покинул " + newMessage.Name));
                                }
                            }
                            sockets[sokIndx].Shutdown(SocketShutdown.Both);
                        }
                        else
                        {
                            Console.WriteLine("Пользователь " + newMessage.Name+" отправил сообщение");
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                if (i != sokIndx)
                                {
                                    sockets[i].Send(Encoding.Default.GetBytes(newMessage.Name + ": " + newMessage.Letter));
                                }
                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        public void BeginToDo(Socket sok)
        {
            try
            {
                Console.WriteLine("Сервер работает");


                sok.Listen(3);
                while (true)
                {
                    if (sockets.Count < 3)
                    {

                        sockets.Add(sok.Accept());
                        int socketIndex = sockets.Count - 1;

                        ClientJoin(socketIndex);
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sok.Close();
                for (int i = 0; i < sockets.Count; i++)
                {
                    sockets[i].Shutdown(SocketShutdown.Both);
                }
            }
        }
    }
}
