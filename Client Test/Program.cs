
using System;

Client client = new Client();

if (await client.Connect())
{
    TsConsole.WriteLine("cool");
}

bool exit = false;

while (!exit)
{
    string message = Console.ReadLine();
    switch (message)
    {
        case "bye":
            if (client.IsConnected)
                await client.Disconnect();
            break;

        case "hi":
            if (!client.IsConnected)
                await client.Connect();
            break;

        case "gtfo":
            exit = true;
            break;

        case "stress test":
            if (client.IsConnected && message != Environment.NewLine && message != string.Empty)
            {
                TsConsole.WriteLine(await client.SendMessage(message));
                for (int i = 0; i < 1000000; i++)
                    await client.SendMessage($"{message} no.{i}");

                TsConsole.WriteLineInColor("Stress test completed", ConsoleColor.Cyan);
            }


            break;

        default:
            if (client.IsConnected && message != Environment.NewLine && message != string.Empty)
            {
                TsConsole.WriteLine(await client.SendMessage(message));
            }
            break;
    }
}

if (client.IsConnected)
    await client.Disconnect();

//Console.ReadLine();