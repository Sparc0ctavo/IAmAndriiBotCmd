using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace IAmAndriiBotCmd
{
    internal class IAmAndriiBot
    {

        private bool _isRunning;
        private TelegramBotClient _client;
        private CancellationTokenSource _cts;
        private string _bot_token;

        public IAmAndriiBot()
        {
            _isRunning = true;
            _bot_token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
            _cts = new CancellationTokenSource();
            _client = new TelegramBotClient(_bot_token, cancellationToken: _cts.Token);

            var commands = new[]
            {
                new BotCommand { Command = "start", Description = "почати роботу"},
                new BotCommand { Command = "help", Description = "список всіх команд"},
                new BotCommand { Command = "presentation", Description = "презентувати автора"},
                new BotCommand { Command = "cv", Description = "резюме"},
                new BotCommand { Command = "contacts", Description = "контакти"}
            };

            _client.SetMyCommands(commands);

        }

        public void Start()
        {

            _client.OnMessage += OnMessage;

            Console.WriteLine($"Бот запущено! Натисніть CTRL+C, щоб завершити роботу.");

            while (_isRunning) { Task.Delay(1000); }
            Stop();
        }

        public void Stop()
        {
            Console.ReadKey();
            _cts.Cancel();
        }

        private async Task OnMessage(Message message, UpdateType updateType)
        {
            int scriptId = -1;
            string textRespose = GetTextResponseScripts(scriptId);

            if (message.Type == MessageType.Text && !string.IsNullOrEmpty(message.Text))
            {
                if (message.Text.StartsWith("/start"))
                {
                    scriptId = 0;
                    textRespose = GetTextResponseScripts(scriptId);
                }
                else if (message.Text.StartsWith("/help"))
                {
                    scriptId = 1;
                    textRespose = GetTextResponseScripts(scriptId);
                }
                else if (message.Text.StartsWith("/presentation"))
                {
                    scriptId = 2;
                    textRespose = GetTextResponseScripts(scriptId);
                }
                else if (message.Text.StartsWith("/cv"))
                {
                    scriptId = 3;
                    textRespose = GetTextResponseScripts(scriptId);
                }
                else if (message.Text.StartsWith("/contacts"))
                {
                    scriptId = 4;
                    textRespose = GetTextResponseScripts(scriptId);
                }

                try
                {

                    if (scriptId == 0 || scriptId == 1 || scriptId == 2)
                    {
                        await _client.SendMessage(message.Chat.Id, textRespose, parseMode: ParseMode.Markdown);
                        Console.WriteLine($"Повідомлення номер \"{scriptId}\" до {message.Chat} надіслано!");
                    }
                    else if (scriptId == 3)
                    {
                        var filePath = "https://drive.google.com/file/d/1RDIV2OyEotbMCtmOMC0toE5Krwm9cjSq/view?usp=sharing";

                        if (File.Exists(filePath))
                        {
                            await _client.SendDocument(message.Chat.Id, filePath);
                            Console.WriteLine($"Файл {filePath} успішно надіслано");
                        }
                        else { Console.WriteLine("Такого файлу не існує."); }
                    }
                    else if (scriptId == 4)
                    {

                       var keyboard = new InlineKeyboardMarkup(new[]
                       {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Мій LinkedIn", "https://www.linkedin.com/in/andrii-khrushch-b2173a354/"),
                                InlineKeyboardButton.WithUrl("Мій GitHub", "https://github.com/Sparc0ctavo"),
                                InlineKeyboardButton.WithCallbackData("+393288976830")
                            },
                            
                       });

                        await _client.SendMessage(message.Chat.Id, textRespose, parseMode: ParseMode.Markdown);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private string GetTextResponseScripts(int scriptId)
        {
            if (scriptId == 0)
            {
                return "Привіт! Я бот, створений в рамках завдання від DICEUS. Тут можна дізнатися коротку інформацію про автора. Для допомоги в навігації по меню введіть /help. Бажаю гарного \"клацання\"!\u2665";
            }
            else if (scriptId == 1)
            {
                return "Ось команди для навігації:\n/help - список всіх команд.\n/presentation - презентувати автора.\n/cv - резюме.\n/contacts - контакти.";
            }
            else if (scriptId == 2)
            {
                return "Доброго дня! Мене звати Хрущ Андрій Андрійович. Народився 17 вересня 2005 року (19 років) у місті [Тернопіль](https://uk.wikipedia.org/wiki/%D0%A2%D0%B5%D1%80%D0%BD%D0%BE%D0%BF%D1%96%D0%BB%D1%8C), Україна." +
                       " Наразі проживаю в місті [Карпі](https://uk.wikipedia.org/wiki/%D0%9A%D0%B0%D1%80%D0%BF%D1%96), Італія. " +
                       " З дитинства цікавився відео іграми, музикою, поезією та різною комп'ютерною технікою. З 15 років серйозно(наскільки міг в 15 років) взявся за програмування. Вільно(майже, хах) спілкуюсь англійською, вільно(без \"майже\") українською, та поки що на середньому рівні італійською.";
            }
            else if (scriptId == 3)
            {
                return "Завантажую резюме...";
            }
            else if (scriptId == 4)
            {
                return "Ось мої основні контакти:";
            }

            return "Під час обробки відповіді сталася помилка. Перевірте правильність написаної команди.";
        }

        private void OnCanselKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _isRunning = false;
        }

    }
}
