namespace Exam_System
{

    public class Helpers
    {
        const string tabs = "\t\t\t\t\t\t\t";
        const string stars = $"{tabs}*****************************************\n";
        public static void Exit()
        {
            LoadingMessage("Exiting the program... Goodbye");
            Environment.Exit(0);
        }
        public static void LoadingMessage(string message)
        {

            Console.Write($"{stars}\n{tabs}\t{message}");
            for (int dot = 0; dot < 3; dot++)
            {
                Thread.Sleep(500);             // half‑second per dot
                Console.Write(".");
            }
            Console.WriteLine();
            // WriteWithIndent("Done!\n\n\n", 9);
            Thread.Sleep(1000);
        }
        public static void WriteWithIndent(string text, int indentLevel = 7)
            => Console.Write("\n" + new string('\t', indentLevel) + text);

    }
}