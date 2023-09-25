using BookGen.Update;

Console.WriteLine("Updater starting...");
using var updater = new Updater();
updater.CheckUpdate();