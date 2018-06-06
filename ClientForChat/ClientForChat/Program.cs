using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientForChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите ваш ник");
            string name = Console.ReadLine();
            ClientWork chat = new ClientWork(name);

            chat.SendMessage("start");
            chat.GetMessage();
            Console.WriteLine("Введите сообщение или 4 для выхода");
            while (true)
            {
                string message = Console.ReadLine();
                if (message == "4")
                {
                    chat.CloseConnect();
                    break;
                }
                else
                {
                    chat.ThrowLetter(message);
                }
            }
        }
    }
}
