namespace Exam_System
{
    public class Program
    {
        protected const string tabs = "\t\t\t\t\t\t\t";
        protected const string stars = $"{tabs}*****************************************\n";
        public static List<Exam> exams = [];
        public static void Main()
        {
            bool exitProgram = false;
            while (!exitProgram)
            {
                MainMenu();
                int role = GetUserRole();
                Console.Clear();
                bool cont;
                switch (role)
                {
                    case 1: //instructor
                        cont = HandleInstructorMenu();
                        if (!cont) exitProgram = true;
                        break;
                    case 2: //student
                        cont = HandleStudentMenu();
                        if (!cont) exitProgram = true;
                        break;
                    case 3: // exit
                        exitProgram = true;
                        break;
                    default:
                        Helpers.WriteWithIndent("Please Choose number from 1~3");
                        break;
                }
            }
            Helpers.Exit();
        }

        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine($"\n{stars}{tabs}\tWelcome to the Exam System\t\t\n{stars}");
            Helpers.WriteWithIndent($"1) Instructor\n{tabs}2) Student\n{tabs}3) Exit\n");
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


        public static void TakeExam(Exam exam, int type)
        {
            // Safety check
            if (exam == null)
            {
                Helpers.WriteWithIndent("Error: No exam selected!\n");
                Console.ReadLine();
                return;
            }

            if (exam.Questions == null || exam.Questions.Count == 0)
            {
                Helpers.WriteWithIndent("Error: This exam has no questions!\n");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine($"\n\n\n\n{stars}{exam}\n{stars}");

            int n = exam.QuestionsNumber;
            exam.Answers = [.. Enumerable.Repeat("0", n)];

            for (int i = 0; i < n; ++i)
            {
                // Safety check for each question
                if (i >= exam.Questions.Count)
                {
                    Helpers.WriteWithIndent($"Error: Question {i + 1} not found!\n");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"{tabs}{i + 1}) {exam.Questions[i]}");
                string answer = GetAnswerForQuestion(exam.Questions[i]);

                exam.Answers[i] = answer;
                Console.WriteLine(stars);
            }

            // Create appropriate exam type and show results
            try
            {
                if (type == 1)
                {
                    PracticeExam p = new(exam.QuestionsNumber, exam.Questions);
                    p.ShowExam();
                }
                else
                {
                    FinalExam f = new(exam.QuestionsNumber, exam.Questions, exam.Answers);
                    f.ShowExam();
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteWithIndent($"Error displaying exam results: {ex.Message}\n");
                Console.ReadLine();
            }
        }

        static string GetAnswerForQuestion(Question question)
        {
            string answer;

            if (question is TrueOrFalseQuestion)
            {
                do
                {
                    Console.Write($"\n{tabs}Enter your answer (1 for true and 2 for false): ");
                    answer = Console.ReadLine()?.Trim();

                } while (!int.TryParse(answer, out int num) || num < 1 || num > 2);
            }
            else if (question is ChooseOneQuestion)
            {
                do
                {
                    Console.Write($"\n{tabs}Enter your answer (1 : 4): ");
                    answer = Console.ReadLine()?.Trim();
                } while (!int.TryParse(answer, out int num) || num < 1 || num > 4);
            }
            else // ChooseAllQuestion
            {
                do
                {
                    Console.Write($"\n{tabs}Enter your answer(s) (1 : 4): ");
                    answer = Console.ReadLine()?.Trim();

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

            return answer;
        }
        static int GetUserRole()
        {
            int role;
            do
            {
                Helpers.WriteWithIndent("Choose your role (or Press 3 to exit): ");
            } while (!int.TryParse(Console.ReadLine(), out role) || role < 1 || role > 3);
            return role;
        }
        static int GetInstructorChoice()
        {
            int choice;
            do
            {
                Helpers.WriteWithIndent("Choose an option (or Press 3 to exit): ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3);
            return choice;
        }
        static bool HandleInstructorMenu()
        {
            InstructorMenu();
            int choice = GetInstructorChoice();
            switch (choice)
            {
                case 1: // Create Exam
                    CreateExam();
                    return true;

                case 2: // Back to Main Menu
                    return true;

                case 3: // Exit
                    return false;

                default:
                    return true;
            }
        }

        static bool HandleStudentMenu()
        {
            StudentMenu();
            int choice = GetStudentChoice();

            switch (choice)
            {
                case 1: // Take Practice Exam
                case 2: // Take Final Exam
                    if (exams.Count == 0)
                    {
                        Helpers.WriteWithIndent("No exams available! Please ask instructor to create an exam first.\n");
                        Console.ReadLine();
                        return true;
                    }
                    Exam exam = SelectExam();
                    TakeExam(exam, choice);
                    return true;

                case 3: // Back to Main Menu
                    return true;

                case 4: // Exit
                    return false;

                default:
                    return true;
            }
        }
        static int GetStudentChoice()
        {
            int choice;
            do
            {
                Helpers.WriteWithIndent("Choose an option (or Press 4 to exit): ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4);

            return choice;
        }

        static Exam SelectExam()
        {
            if (exams.Count == 0)
            {
                Helpers.WriteWithIndent("No exams available!\n");
                return null;
            }

            if (exams.Count == 1)
            {
                Helpers.WriteWithIndent($"Taking the only available exam: {exams[0].Subject.Name}\n");
                Console.WriteLine();
                return exams[0];
            }

            Console.Clear();
            Console.WriteLine($"\n{stars}{tabs}\tSelect an Exam\n{stars}");

            for (int i = 0; i < exams.Count; i++)
            {
                Console.WriteLine($"{tabs}{i + 1}) {exams[i].Subject.Name} - {exams[i].Subject.Code}");
                Console.WriteLine($"{tabs}   Instructor: Eng.{exams[i].Subject.Instructor}");
                Console.WriteLine($"{tabs}   Questions: {exams[i].QuestionsNumber}, Time: {exams[i].Time} minutes\n");
            }

            int choice;
            do
            {
                Helpers.WriteWithIndent($"Choose an exam (1 to {exams.Count}): ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > exams.Count);

            return exams[choice - 1];
        }

        static void CreateExam()
        {
            Console.Clear();
            Thread.Sleep(300);
            Console.WriteLine($"\n\n{stars}{tabs}\tCreating a new exam form  \n{stars}");
            Thread.Sleep(300);

            Subject subject = GetSubjectInfo();
            int time = GetExamTime();
            int questionsNumber = GetQuestionsNumber();
            Exam exam = new(time, subject, questionsNumber);
            AddQuestionsToExam(exam, questionsNumber);
            exams.Add(exam);
            Helpers.WriteWithIndent("Exam created successfully!\n");
            Thread.Sleep(1500);
            Console.ReadLine();
        }

      
        static void AddQuestionsToExam(Exam exam, int questionsNumber)
        {
            for (int i = 0; i < questionsNumber; ++i)
            {
                Console.WriteLine($"\n\n{tabs}Question {i + 1}:\n{stars}");

                string header = GetQuestionHeader();
                string body = GetQuestionBody();
                int marks = GetQuestionMarks();
                int questionType = GetQuestionType();

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
        }




         //Get Exam Data
        static Subject GetSubjectInfo()
        {
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

            return new Subject(subjectName, subjectInstructor, subjectCode);
        }
        static int GetExamTime()
        {
            int time;
            do
            {
                Helpers.WriteWithIndent("Enter Exam Time in minutes (15 : 45): ");
            } while (!int.TryParse(Console.ReadLine(), out time) || time < 15 || time > 45);

            return time;
        }

        static int GetQuestionsNumber()
        {
            int questionsNumber;
            do
            {
                Helpers.WriteWithIndent("Enter Number of Questions (1 : 100): ");
            } while (!int.TryParse(Console.ReadLine(), out questionsNumber) || questionsNumber < 1 || questionsNumber > 100);

            return questionsNumber;
        }

        static string GetQuestionHeader()
        {
            string header;
            do
            {
                Helpers.WriteWithIndent("Enter Question Header: ");
                header = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(header));

            return header;
        }

        static string GetQuestionBody()
        {
            string body;
            do
            {
                Helpers.WriteWithIndent("Enter Question Body: ");
                body = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(body));

            return body;
        }

        static int GetQuestionMarks()
        {
            int marks;
            do
            {
                Helpers.WriteWithIndent("Enter Question Marks (1 : 3): ");
            } while (!int.TryParse(Console.ReadLine(), out marks) || marks < 1 || marks > 3);

            return marks;
        }

        static int GetQuestionType()
        {
            Console.WriteLine($"\n{tabs}1) True or False Question\n{tabs}2) Choose One Question\n{tabs}3) Choose All Question\n");
            Console.Write($"\n{tabs}Enter Question Type (1 : 3): ");

            int questionType;
            do
            {
                if (!int.TryParse(Console.ReadLine(), out questionType) || questionType < 1 || questionType > 3)
                    Console.Write($"{tabs}Please choose number from 1 to 3: ");
                else break;
            } while (true);

            return questionType;
        }
    }
}

