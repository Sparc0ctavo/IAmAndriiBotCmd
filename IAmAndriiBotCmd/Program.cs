using System.Text;

namespace IAmAndriiBotCmd
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;


            IAmAndriiBot bot = new IAmAndriiBot();

            bot.Start();
        }
    }
}
