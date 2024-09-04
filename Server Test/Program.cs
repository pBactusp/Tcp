
Console.WriteLine("Hello, World!");

var server = new Server();

server.Start();

while (server.IsActive)
    await Task.Delay(100);