namespace Exam_System
{
    public class Program
    {
        protected const string tabs = "\t\t\t\t\t\t\t";
        protected const string stars = $"{tabs}*****************************************\n";
        public readonly static List<Exam> Pexams = [], Fexams = [];

        public static void Main()
        {
            MainMenu();

            Exam e1;
            e1=new PracticeExam();


            int examType;
            do
            {
                WriteWithIndent("Enter Exam type (or Press 3 to exit): ");
            } while (!int.TryParse(Console.ReadLine(), out examType) || examType < 1 || examType > 3);
            if (examType == 3) Exit();


            ExamMenu(examType);

            int choice;
            do
            {
                WriteWithIndent("Choose an option (or Press 4 to exit): ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4);





            if (choice == 1) // Create Exam
            {
                Thread.Sleep(300);
                Console.WriteLine($"\n\n{stars}{tabs}\tCreating a new exam form  \n{stars}");
                Thread.Sleep(300);

                Subject subject = Subject.GetSubject();
                Exam exam;

                int time;
                do
                {
                    WriteWithIndent("Enter Exam Time in minutes (15 : 45): ");
                } while (!int.TryParse(Console.ReadLine(), out time) || time < 15 || time > 45);


                int questionsNumber;
                do
                {
                    WriteWithIndent("Enter Number of Questions (1 : 100): ");
                } while (!int.TryParse(Console.ReadLine(), out questionsNumber) || questionsNumber < 1 || questionsNumber > 100);

                if (examType == 1)
                {
                    exam = new PracticeExam(time, subject, questionsNumber);
                    exam.AddQuestions(questionsNumber, "Practice");
                    Pexams.Add(exam);
                }
                else
                {
                    exam = new FinalExam(time, subject, questionsNumber);
                    exam.AddQuestions(questionsNumber, "Final");
                    Fexams.Add(exam);
                }


                Main();
                return;
            }
            else if (choice == 2) // Take Exam
            {
                Exam exam = choice == 1 ? new PracticeExam() : new FinalExam();
                exam.TakeExam(examType);
            }
            else if (choice == 3) Main();
            else Exit();
        }

        static void MainMenu()
        {
            Thread.Sleep(200);
            Console.WriteLine($"\n{stars}{tabs}\tExamination System Menu\t\t\n{stars}");
            Console.WriteLine($"{tabs}1) Practice Exam\n{tabs}2) Final Exam\n{tabs}3) Exit\n");
        }
        static void ExamMenu(int examType)
        {
            if (examType == 1)
            {
                Thread.Sleep(200);
                Console.WriteLine($"\n{stars}{tabs}\tPractice Exam Menu\t\t\n{stars}");
                Console.WriteLine($"{tabs}1) Create Practice Exam\n{tabs}2) Take Practice Exam\n{tabs}3) Back to Main Menu\n{tabs}4)Exit\n");
            }
            else if (examType == 2)
            {
                Thread.Sleep(200);
                Console.WriteLine($"\n{stars}{tabs}\tFinal Exam Menu\t\t\t\n{stars}");
                Console.WriteLine($"{tabs}1) Create Final Exam\n{tabs}2) Take Final Exam\n{tabs}3) Back to Main Menu\n{tabs}4)Exit\n");
            }
        }

        static void Exit()
        {
            WriteWithIndent("Exiting the program...Goodbye\n");
            Thread.Sleep(2000);
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
            WriteWithIndent("Done!\n\n\n", 9);
            Thread.Sleep(1000);
        }
        public static void WriteWithIndent(string text, int indentLevel = 7)
            => Console.Write("\n" + new string('\t', indentLevel) + text);
        

    }
}

