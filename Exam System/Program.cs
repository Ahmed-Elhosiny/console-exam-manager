namespace Exam_System
{
    public class Program
    {
        protected const string tabs = "\t\t\t\t\t\t\t";
        protected const string stars = $"{tabs}*****************************************\n";
        public static List<Exam> exams = [];
        public static void Main()
        {
            MainMenu();
            int role;
            do
            {
                WriteWithIndent("Choose your role (or Press 3 to exit): ");
            } while (!int.TryParse(Console.ReadLine(), out role) || role < 1 || role > 3);

            Console.Clear();


            if (role == 1)
            {
                InstructorMenu();
                int choice;
                do
                {
                    WriteWithIndent("Choose an option (or Press 3 to exit): ");
                } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3);
                if (choice == 1)
                {
                    Console.Clear();
                    Thread.Sleep(300);
                    Console.WriteLine($"\n\n{stars}{tabs}\tCreating a new exam form  \n{stars}");
                    Thread.Sleep(300);

                    string subjectName;
                    do
                    {
                        Console.Write($"\n{tabs}Enter Subject Name: ");
                        subjectName = Console.ReadLine();
                    } while (string.IsNullOrWhiteSpace(subjectName));

                    string subjectInstructor;
                    do
                    {
                        Console.Write($"{tabs}Enter Subject Instructor: ");
                        subjectInstructor = Console.ReadLine();
                    } while (string.IsNullOrWhiteSpace(subjectInstructor));

                    string subjectCode;
                    do
                    {
                        Console.Write($"{tabs}Enter Subject Code: ");
                        subjectCode = Console.ReadLine();
                    } while (string.IsNullOrWhiteSpace(subjectCode));

                    Subject subject = new(subjectName, subjectInstructor, subjectCode);

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

                    Exam exam = new(time, subject, questionsNumber);

                    Thread.Sleep(1000);

                    // Add questions
                    for (int i = 0; i < questionsNumber; ++i)
                    {
                        Console.WriteLine($"\n\n{tabs}Question {i + 1}:\n{stars}\n");

                        string header;
                        do
                        {
                            WriteWithIndent("Enter Question Header: ");
                        } while (string.IsNullOrWhiteSpace(header = Console.ReadLine()));

                        string body;
                        do
                        {
                            WriteWithIndent("Enter Question Body: ");
                        } while (string.IsNullOrWhiteSpace(body = Console.ReadLine()));

                        int marks;
                        do
                        {
                            WriteWithIndent("Enter Question Marks (1 : 3): ");
                        } while (!int.TryParse(Console.ReadLine(), out marks) || marks < 1 || marks > 3);
                        Thread.Sleep(500);

                        Console.WriteLine($"\n{tabs}1) True or False Question\n{tabs}2) Choose One Question\n{tabs}3) Choose All Question\n");
                        Console.Write($"\n{tabs}Enter Question Type (1 : 3): ");
                        int questionType;
                        do
                        {
                            if (!int.TryParse(Console.ReadLine(), out questionType) || questionType < 1 || questionType > 3)
                                Console.Write($"{tabs}Please choose number from 1 to 3: ");
                            else break;
                        } while (true);

                        switch (questionType)
                        {
                            case 1:
                                exam.AddTrueOrFalseQ(body, header, marks);
                                break;
                            case 2:
                                exam.AddCOQ(body, header, marks);
                                break;

                            case 3:
                                exam.AddCAQ(body, header, marks);
                                break;
                        }


                    }
                    exams.Add(exam);
                    Main();
                    return;


                }
                else if (choice == 2)
                {
                    Main();
                    return;
                }
                else Exit();

            }
            else if (role == 2)
            {
                StudentMenu();
                int choice;
                do
                {
                    WriteWithIndent("Choose an option (or Press 4 to exit): ");
                } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4);

                if (choice == 3)
                {
                    Main();
                    return;
                }
                else if (choice == 4) Exit();


                if(exams.Count == 0)
                {
                    WriteWithIndent("No exams available. Please ask your instructor to create an exam.\n");
                    Thread.Sleep(2000);
                    Main();
                    return;
                }
                TakeExam(exams[0], choice);
            }
            else Exit();







        }

        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine($"\n{stars}{tabs}\tWelcome to the Exam System\t\t\n{stars}");
            WriteWithIndent($"1) Instructor\n{tabs}2) Student\n{tabs}3) Exit\n");
            Thread.Sleep(200);
        }
        static void InstructorMenu()
        {
            Thread.Sleep(200);
            Console.WriteLine($"\n{stars}{tabs}\tInstructor Menu\t\t\n{stars}");
            Console.WriteLine($"{tabs}1) Create Exam\n{tabs}2) Back to Main Menu\n{tabs}3) Exit\n");
        }
        static void StudentMenu()
        {
            Console.Clear();
            Thread.Sleep(200);
            Console.WriteLine($"\n{stars}{tabs}\tStudent Menu\t\t\n{stars}");
            Console.WriteLine($"{tabs}1) Take Practice Exam\n{tabs}2) Take Final Exam\n{tabs}3) Back to Main Menu\n{tabs}4) Exit\n");
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
        public static void TakeExam(Exam exam, int type)
        {
            Console.Clear();
            Console.WriteLine($"\n\n\n\n{stars}{exam}\n{stars}");
            int n = exam.QuestionsNumber;
            exam.Answers = [.. Enumerable.Repeat("0", n)];

            for (int i = 0; i < n; ++i)
            {
                Console.WriteLine($"{tabs}{i + 1}) {exam.Questions[i]}");
                string answer;

                if (exam.Questions[i] is TrueOrFalseQuestion)
                {
                    do
                    {
                        Console.Write($"\n{tabs}Enter your answer (1 for true and 2 for false): ");
                        answer = Console.ReadLine().Trim();

                    } while (!int.TryParse(answer, out int num) || num < 1 || num > 2);
                }
                else if (exam.Questions[i] is ChooseOneQuestion)
                {
                    do
                    {
                        Console.Write($"\n{tabs}Enter your answer (1 : 4): ");
                        answer = Console.ReadLine().Trim();
                    } while (!int.TryParse(answer, out int num) || num < 1 || num > 4);
                }
                else
                {
                    do
                    {
                        Console.Write($"\n{tabs}Enter your answer(s) (1 : 4): ");
                        answer = Console.ReadLine().Trim();
                        if (string.IsNullOrWhiteSpace(answer))
                            continue;

                        string[] answers = answer.Split(',');
                        bool valid = answers.All(ans =>
                            int.TryParse(ans.Trim(), out int num) && num >= 1 && num <= 4
                        );

                        if (valid) break;
                        Console.WriteLine($"{tabs}Invalid input. Please enter numbers between 1 and 4, separated by commas if multiple.");
                    } while (true);
                }

                exam.Answers[i] = answer;
                Console.WriteLine(stars);
            }
            if(type == 1)
            {
                PracticeExam p = new(exam.QuestionsNumber, exam.Questions);
                p.ShowExam();
            }
            else
            {
                FinalExam f = new(exam.QuestionsNumber, exam.Questions, exam.Answers);
                f.ShowExam();
            }




                Main();
        }

    }
}

