using System.Text.RegularExpressions;

namespace Lab8CSharp;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        EnsureFilesExist();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Головне меню:");
            Console.WriteLine("1. Завдання 1 (Пошук та заміна двійкових IP-адрес)");
            Console.WriteLine("2. Завдання 2 (Видалення цифр та пунктуації)");
            Console.WriteLine("3. Завдання 3 (Слова в лексикографічному порядку)");
            Console.WriteLine("4. Завдання 4 (Робота із двійковими файлами)");
            Console.WriteLine("5. Завдання 5 (Робота з файловою системою)");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть опцію: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Task1();
                    break;

                case "2":
                    Task2();
                    break;

                case "3":
                    Task3();
                    break;

                case "4":
                    Task4();
                    break;

                case "5":
                    Task5();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження..."); 
            Console.ReadKey();
        }
    }

    static void EnsureFilesExist()
    {
        if (!File.Exists("task1_in.txt"))
        {
            File.WriteAllText("task1_in.txt", "Сервер 1 має адресу 11000000.10101000.00000001.00000001, а сервер 2 - 10.0.0.11111111. Також є помилкова 1111.22.33.44.");
        }

        if (!File.Exists("task2_in.txt"))
        {
            File.WriteAllText("task2_in.txt", "Hello, World! Це тестовий рядок номер 123. Чи працює видалення пунктуації? Так, на 100%.");
        }

        if (!File.Exists("task3_in.txt"))
        {
            File.WriteAllText("task3_in.txt", "abc aeg a bz cba door efgh aab xyz hello");
        }
    }

    static void Task1()
    {
        string inputFile = "task1_in.txt";
        string outputFile = "task1_out.txt";
        string modifiedFile = "task1_modified.txt";

        string text = File.ReadAllText(inputFile);
        string pattern = @"\b[01]{1,8}\.[01]{1,8}\.[01]{1,8}\.[01]{1,8}\b";

        MatchCollection matches = Regex.Matches(text, pattern);

        Console.WriteLine($"Знайдено IP-адрес: {matches.Count}");

        List<string> foundIps = matches.Cast<Match>().Select(m => m.Value).ToList();

        foundIps.ForEach(ip => Console.WriteLine($"- {ip}"));

        File.WriteAllLines(outputFile, foundIps);
        Console.WriteLine($"Знайдені IP-адреси збережено у файл {outputFile}");

        Console.Write("\nВведіть IP-адресу для пошуку та заміни (або видалення): ");
        string targetIp = Console.ReadLine();

        Console.Write("Введіть нове значення (залиште порожнім для видалення): ");
        string replacement = Console.ReadLine();

        string newText = text.Replace(targetIp, replacement);
        File.WriteAllText(modifiedFile, newText);

        Console.WriteLine($"Результат із заміною/видаленням збережено у файл {modifiedFile}");
    }

    static void Task2()
    {
        string inputFile = "task2_in.txt";
        string outputFile = "task2_out.txt";

        string text = File.ReadAllText(inputFile);
        string result = Regex.Replace(text, @"[\d\p{P}]", "");

        File.WriteAllText(outputFile, result);

        Console.WriteLine("Початковий текст:");
        Console.WriteLine(text);
        Console.WriteLine("\nРезультат (без цифр і пунктуації):");
        Console.WriteLine(result);
        Console.WriteLine($"\nРезультат збережено у файл {outputFile}");
    }

    static void Task3()
    {
        string inputFile = "task3_in.txt";
        string outputFile = "task3_out.txt";

        string text = File.ReadAllText(inputFile);

        List<string> lexicographicalWords = Regex.Split(text.ToLower(), @"[\W_]+")
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Where(w => w.Zip(w.Skip(1), (a, b) => a <= b).All(x => x))
            .ToList();

        File.WriteAllLines(outputFile, lexicographicalWords);

        Console.WriteLine("Знайдені слова в лексикографічному порядку:");
        lexicographicalWords.ForEach(word => Console.WriteLine($"- {word}"));

        Console.WriteLine($"\nРезультат збережено у файл {outputFile}");
    }

    static void Task4()
    {
        string binFile = "task4_words.bin";
        string[] initialWords = { "яблуко", "апельсин", "ананас", "банан", "абрикос", "кавун", "акула" };

        using (BinaryWriter bw = new BinaryWriter(File.Open(binFile, FileMode.Create)))
        {
            bw.Write(initialWords.Length);
            initialWords.ToList().ForEach(word => bw.Write(word));
        }

        List<string> readWords = new List<string>();

        using (BinaryReader br = new BinaryReader(File.Open(binFile, FileMode.Open)))
        {
            int count = br.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                readWords.Add(br.ReadString());
            }
        }

        if (readWords.Any())
        {
            string lastWord = readWords.Last();
            char targetLetter = char.ToLower(lastWord[0]);

            Console.WriteLine($"Вміст файлу зчитано. Останнє слово: {lastWord}");
            Console.WriteLine($"\nСлова, що починаються на букву '{targetLetter}':");

            readWords.Where(w => char.ToLower(w[0]) == targetLetter)
                     .ToList()
                     .ForEach(word => Console.WriteLine($"- {word}"));
        }
    }

    static void Task5()
    {
        string baseDir = @"C:\temp";
        string dir1 = Path.Combine(baseDir, "Tokar1");
        string dir2 = Path.Combine(baseDir, "Tokar2");
        string dirAll = Path.Combine(baseDir, "ALL");

        try
        {
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            if (Directory.Exists(dir1))
            {
                Directory.Delete(dir1, true);
            }

            if (Directory.Exists(dir2))
            {
                Directory.Delete(dir2, true);
            }

            if (Directory.Exists(dirAll))
            {
                Directory.Delete(dirAll, true);
            }

            Directory.CreateDirectory(dir1);
            Directory.CreateDirectory(dir2);

            string t1Path = Path.Combine(dir1, "t1.txt");
            string t2Path = Path.Combine(dir1, "t2.txt");
            string t3Path = Path.Combine(dir2, "t3.txt");

            File.WriteAllText(t1Path, "Токар Іван Андрійович, 2004 року народження, місце проживання м. Запоріжжя\n");
            File.WriteAllText(t2Path, "Комар Сергій Федорович, 2000 року народження, місце проживання м. Київ\n");

            string t1Content = File.ReadAllText(t1Path);
            string t2Content = File.ReadAllText(t2Path);

            File.WriteAllText(t3Path, t1Content + t2Content);

            Action<string> printFileInfo = (filePath) =>
            {
                FileInfo fi = new FileInfo(filePath);
                Console.WriteLine($"Файл: {fi.Name} | Розмір: {fi.Length} байт | Створено: {fi.CreationTime}");
            };

            Console.WriteLine("Розгорнута інформація про створені файли:");
            printFileInfo(t1Path);
            printFileInfo(t2Path);
            printFileInfo(t3Path);

            string t2NewPath = Path.Combine(dir2, "t2.txt");
            File.Move(t2Path, t2NewPath);

            string t1NewPath = Path.Combine(dir2, "t1.txt");
            File.Copy(t1Path, t1NewPath);

            Directory.Move(dir2, dirAll);
            Directory.Delete(dir1, true);

            Console.WriteLine("\nПовна інформація про файли папки ALL:");

            new DirectoryInfo(dirAll).GetFiles()
                .ToList()
                .ForEach(fi => Console.WriteLine($"Файл: {fi.Name} | Розмір: {fi.Length} байт | Змінено: {fi.LastWriteTime} | Шлях: {fi.FullName}"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка доступу до файлової системи: {ex.Message}");
            Console.WriteLine("Переконайтеся, що диск C: існує та програма має права на запис.");
        }
    }
}


