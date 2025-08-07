namespace Exam_System
{
    public abstract class Exam : Program
    {
        protected const string tabs = "\t\t\t\t\t\t\t";
        protected const string stars = $"{tabs}*****************************************\n";

        int time;
        int questionsNumber;
        Subject subject;
        protected List<Question> questions;
        List<string> answers;
       

        public Exam() : this(180, new Subject(), 60) {}
        public Exam(int time, Subject subject, int questionsNumber)
        {
            Time = time;
            Subject = subject;
            Questions = [];
            Answers = [];
            QuestionsNumber = questionsNumber;
            TotalMarks = 0;
        }
        public int TotalMarks { get; private set; }
        public int Time
        {
            get => time;
            set
            {
                if (value < 15 || value > 45) throw new ArgumentOutOfRangeException("Invalid Exam Time. (exam range: 15:45 minutes)");
                time = value;
            }
        }
        public Subject Subject
        {
            get => subject;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(subject));
                subject = value;
            }
        }
        public int QuestionsNumber
        {
            get => questionsNumber;
            set
            {
                if (value < 1 || value > 100) throw new ArgumentOutOfRangeException("Invalid Questions Number. (exam range: 1:100 questions)");
                questionsNumber = value;
            }
        }
        public List<Question> Questions
        {
            get => questions;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(questions));
                questions = value;
                questionsNumber = questions.Count;
            }
        }
        public List<string> Answers
        {
            get => answers;
            set
            {
                answers = value;
            }

        }

        void AddQuestion(Question question)
        {
            Questions.Add(question);
            TotalMarks += question.Marks;
        }

        void AddTrueOrFalseQ(string body, string header, int marks)
        {
            WriteWithIndent("Enter True or False Answer: ");
            string answer;
            do
            {
                answer = Console.ReadLine().Trim().ToUpper();
                if (answer == null || answer == "" || answer != "TRUE" && answer != "FALSE" && answer != "T" && answer != "F")
                    WriteWithIndent("Please enter 'true or t' or 'false or f': ");
                else break;
            } while (true);

            AddQuestion(new TrueOrFalseQuestion(body, header, marks, answer));

        }
        void AddCOQ(string body, string header, int marks)
        {
            string[] options;
            do
            {
                WriteWithIndent("Enter Options (4 options separated by commas): ");
                options = Console.ReadLine().Split(',');

            } while (options.Length != 4);
            int correctOptionIndex;
            do
            {
                WriteWithIndent("Enter Correct Option Index (1 : 4): ");

            } while (!int.TryParse(Console.ReadLine(), out correctOptionIndex) || correctOptionIndex < 1 || correctOptionIndex > 4);

            AddQuestion(new ChooseOneQuestion(body, header, marks, options, correctOptionIndex));


        }
        void AddCAQ(string body, string header, int marks)
        {
            string[] allOptions;
            do
            {
                WriteWithIndent("Enter Options (4 options separated by commas): ");
                allOptions = Console.ReadLine().Split(',');

            } while (allOptions.Length != 4);


            string[] correctIndicesInput;
            int[] correctOptionIndices;
            bool ok;
            do
            {
                ok = true;
                do
                {
                    WriteWithIndent("Enter Correct Option Indices (comma-separated, e.g., 1,2): ");
                    correctIndicesInput = Console.ReadLine().Split(',');
                } while (correctIndicesInput.Length == 0 || correctIndicesInput.Any(i => !int.TryParse(i, out int index) || index < 1 || index > 4));
                correctOptionIndices = new int[correctIndicesInput.Length];

                for (int i = 0; i < correctIndicesInput.Length; i++)
                {
                    if (!int.TryParse(correctIndicesInput[i], out int index) || index < 1 || index > 4)
                    {
                        WriteWithIndent($"Invalid index '{correctIndicesInput[i]}'. Please enter indices between 1 and 4.\n");
                        ok = false;
                        break;
                    }
                    correctOptionIndices[i] = index;
                }
            } while (!ok);




            AddQuestion(new ChooseAllQuestion(body, header, marks, allOptions, correctOptionIndices));
        }
        public void AddQuestions(int questionsNumber, string examType)
        {
            Thread.Sleep(1000);
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
                        AddTrueOrFalseQ(body, header, marks);
                        break;
                    case 2:
                        AddCOQ(body, header, marks);
                        break;

                    case 3:
                        AddCAQ(body, header, marks);
                        break;
                }

            }
            LoadingMessage($"Creating {examType} Exam");
        }

        public abstract void ShowExam();
        public void TakeExam(int examType)
        {
            int examIdx;
            if (examType == 1)
            {
                if (Pexams.Count == 0)
                {
                    WriteWithIndent("No Practice Exams available. Come Back Later.\n");
                    Thread.Sleep(1500);
                    WriteWithIndent("Press any key to go to the main menu...");
                    Console.ReadLine();
                    return;
                }
                examIdx = new Random().Next(Pexams.Count);
            }
            else
            {
                if (Fexams.Count == 0)
                {
                    WriteWithIndent("No Final Exams available. Come Back Later.\n");
                    Thread.Sleep(1500);
                    WriteWithIndent("Press any key to go to the main menu...");
                    Console.ReadLine();
                    return;
                }
                examIdx = new Random().Next(Fexams.Count);
            }
            Exam exam = examType == 1 ? Pexams[examIdx] : Fexams[examIdx];
            int n = exam.QuestionsNumber;
            exam.Answers = new List<string>(Enumerable.Repeat("0", n));

            Console.WriteLine($"\n\n\n\n{stars}{exam}\n{stars}");
            for (int i = 0; i < n; ++i)
            {
                WriteWithIndent($"{i + 1}) {exam.Questions[i]}\n");
                string answer;

                if (exam.Questions[i] is TrueOrFalseQuestion)
                {
                    do
                    {
                        WriteWithIndent("Enter Your answer (1 for true and 2 for false): ");
                        answer = Console.ReadLine().Trim();

                    } while (!int.TryParse(answer, out int num) || num < 1 || num > 2);
                }
                else if (exam.Questions[i] is ChooseOneQuestion)
                {
                    do
                    {
                        WriteWithIndent("Enter your answer (1 : 4): ");
                        answer = Console.ReadLine().Trim();
                    } while (!int.TryParse(answer, out int num) || num < 1 || num > 4);
                }
                else
                {
                    do
                    {
                        WriteWithIndent("Enter your answer(s) (1 : 4): ");
                        answer = Console.ReadLine().Trim();
                        if (string.IsNullOrWhiteSpace(answer))
                            continue;

                        string[] answers = answer.Split(',');
                        bool valid = answers.All(ans =>
                            int.TryParse(ans.Trim(), out int num) && num >= 1 && num <= 4
                        );

                        if (valid) break;
                        WriteWithIndent("Invalid input. Please enter numbers between 1 and 4, separated by commas if multiple.");
                    } while (true);
                }

                exam.Answers[i] = answer;
                Console.WriteLine(stars);
            }
            Thread.Sleep(1000);
            WriteWithIndent("Thank You.\n");
            Thread.Sleep(1000);
            Console.WriteLine(stars);
            exam.ShowExam();
            Thread.Sleep(1000);
            WriteWithIndent("Press any key to go to the main menu...");
            Console.ReadLine();
            Main();
        }

    }
    public class PracticeExam : Exam
    {
        public PracticeExam() : this(30, new Subject(), 10) { }
        public PracticeExam(int time, Subject subject, int questionNumber) : base(time, subject, questionNumber) { }

        public override void ShowExam()
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));
            string answers = $"\n\n{tabs}Correct Answers for The Practice Exam:\n{stars}\n";

            for (int i = 0; i < questions.Count; ++i)
            {
                answers += $"{tabs}{i + 1}. Correct Answer(s): ";
                Question q = questions[i];
                if (q is ChooseOneQuestion coq)
                {
                    int idx = coq.CorrectOptionIndex;
                    //string correctOption = coq.Options[idx];
                    string correctOption = $"{idx}) {coq.Options[idx - 1]}";
                    answers += correctOption;
                }
                else if (q is ChooseAllQuestion caq)
                {
                    string correctOptions = string.Join(" & ", caq.CorrectOptionIndices.Select(idx => $"{idx}) {caq.Options[idx - 1]}"));
                    answers += correctOptions;
                }
                else if (q is TrueOrFalseQuestion tfq)
                {
                    if (tfq.Answer == "TRUE" /*|| tfq.Answer == "T"*/)
                        answers += $"1) True";
                    else
                        answers += "2) False";
                }
                answers += "\n";
            }

            Console.WriteLine(answers);
        }

        public override string ToString()
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));
            string exam = $"{tabs}Practice Exam\n{tabs}Time: {Time} minutes\n{Subject}\n{stars}\n";
            exam += $"{tabs}Full Practice Exam\n{stars}\n";
            for (int i = 0; i < questions.Count; ++i)
            {
                exam += $"{tabs}{i + 1}) {questions[i]}\n\n\n";
            }
            return exam;
        }
    }
    public class FinalExam : Exam
    {
        public FinalExam() : this(45, new Subject(), 100) { }
        public FinalExam(int time, Subject subject, int questionNumber) : base(time, subject, questionNumber) { }

        public override void ShowExam()
        {
            WriteWithIndent("Your Answers: \n\n\n");
            Console.WriteLine(stars);
            for (int i = 0; i < QuestionsNumber; ++i)
            {
                Console.WriteLine($"{tabs}{i + 1}) {Questions[i]}");
                if (Questions[i] is ChooseAllQuestion caq)
                {

                    string[] ans = Answers[i].Split(',');
                    WriteWithIndent("Your Answer(s): ");
                    Console.WriteLine(string.Join(" & ", ans.Select(s => int.TryParse(s, out int num) ? $"{num}) {caq.Options[num - 1]}" : "")));

                }
                else if (Questions[i] is ChooseOneQuestion coq)
                {
                    WriteWithIndent($"Your Answer: {Answers[i]}) {coq.Options[int.Parse(Answers[i]) - 1]}\n\n");
                }
                else
                {
                    string ans = Answers[i];
                    string t = (ans == "1") ? "TRUE" : "FALSE";
                    WriteWithIndent($"Your Answer: {ans}) {t}\n\n");
                }
            }
        }
        public override string ToString()
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));
            string exam = $"{tabs}Final Exam\n{tabs}Time: {Time} minutes\n{Subject}{stars}\n";
            exam += $"{tabs}Full Final Exam\n{stars}\n";
            for (int i = 0; i < questions.Count; ++i)
            {
                exam += $"{tabs}{i + 1}) {questions[i]}\n\n\n";
            }
            return exam;
        }
    }

}
