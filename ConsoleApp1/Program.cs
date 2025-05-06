using System.Text.Json;
using EasySave.Jobs;

class Program
{
    static Dictionary<string, string> messages;

    static void Main(string[] args)
    {
        LoadLanguage();

        Console.WriteLine(messages["welcome"]);

        List<BackupJob> tasks = new();

        while (true)
        {
            Console.WriteLine("\n1. " + messages["create"]);
            Console.WriteLine("2. " + messages["run"]);
            Console.WriteLine("3. " + messages["runAll"]);
            Console.WriteLine("4. " + messages["exit"]);

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateTask(tasks);
                    break;
                case "4":
                    return;
            }
        }
    }

    static void LoadLanguage()
    {
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("config.json"));
        var lang = config["language"];
        var allLanguages = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("Lang/languages.json"));
        messages = allLanguages[lang];
    }

    static void CreateTask(List<BackupJob> tasks)
    {
        if (tasks.Count >= 5)
        {
            Console.WriteLine("Limite de 5 tâches atteinte.");
            return;
        }

        Console.Write("Nom de la tâche : ");
        string name = Console.ReadLine();
        Console.Write("Chemin source : ");
        string src = Console.ReadLine();
        Console.Write("Chemin cible : ");
        string tgt = Console.ReadLine();
        Console.Write("Type (full/diff) : ");
        string type = Console.ReadLine();

        tasks.Add(new BackupJob(name, src, tgt, type));
        Console.WriteLine("Tâche créée !");
    }
}