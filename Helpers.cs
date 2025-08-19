namespace Exam_System
{
    public static class Helpers
    {
        const string tabs = "\t\t\t\t\t\t\t";
        const string stars = $"{tabs}*****************************************\n";

        public static void Exit()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n{stars}{tabs}\tExiting Application\n{stars}");
            Console.ResetColor();

            LoadingMessage("Saving data and exiting", ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteWithIndent("Thank you for using the Exam System!");
            Console.ResetColor();
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static void LoadingMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write($"{stars}\n{tabs}\t{message}");

            for (int dot = 0; dot < 3; dot++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }

            Console.WriteLine(" Done!");
            Console.ResetColor();
            Thread.Sleep(800);
        }

        public static void WriteWithIndent(string text, int indentLevel = 7)
            => Console.Write("\n" + new string('\t', indentLevel) + text);
    }
}

