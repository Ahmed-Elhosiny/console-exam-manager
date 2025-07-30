namespace Exam_System
{
    public abstract class Exam
    {
        protected const string tabs = "\t\t\t\t\t\t\t";
        protected const string stars = $"{tabs}*****************************************\n";
        int time;
        int questionsNumber;
        Subject subject;
        protected List<Question> questions;
        List<string> answers;

        public Exam() : this(180, new Subject(), 60) { }
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
            Console.Write($"{tabs}Enter True or False Answer: ");
            string answer;
            do
            {
                answer = Console.ReadLine().Trim().ToUpper();
                if (answer == null || answer == "" || answer != "TRUE" && answer != "FALSE" && answer != "T" && answer != "F")
                    Console.Write($"{tabs}Please enter 'true or t' or 'false or f': ");
                else break;
            } while (true);

            AddQuestion(new TrueOrFalseQuestion(body, header, marks, answer));

        }
        void AddCOQ(string body, string header, int marks)
        {
            string[] options;
            do
            {
                Console.Write($"{tabs}Enter Options (4 options separated by commas): ");
                options = Console.ReadLine().Split(',');

            } while (options.Length != 4);
            int correctOptionIndex;
            do
            {
                Console.Write($"{tabs}Enter Correct Option Index (1 : 4): ");

            } while (!int.TryParse(Console.ReadLine(), out correctOptionIndex) || correctOptionIndex < 1 || correctOptionIndex > 4);

            AddQuestion(new ChooseOneQuestion(body, header, marks, options, correctOptionIndex));


        }
        void AddCAQ(string body, string header, int marks)
        {
            string[] allOptions;
            do
            {
                Console.Write($"{tabs}Enter Options (4 options separated by commas): ");
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
                    Console.Write($"{tabs}Enter Correct Option Indices (comma-separated, e.g., 1,2): ");
                    correctIndicesInput = Console.ReadLine().Split(',');
                } while (correctIndicesInput.Length == 0 || correctIndicesInput.Any(i => !int.TryParse(i, out int index) || index < 1 || index > 4));
                correctOptionIndices = new int[correctIndicesInput.Length];

                for (int i = 0; i < correctIndicesInput.Length; i++)
                {
                    if (!int.TryParse(correctIndicesInput[i], out int index) || index < 1 || index > 4)
                    {
                        Console.WriteLine($"{tabs}Invalid index '{correctIndicesInput[i]}'. Please enter indices between 1 and 4.");
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
                string header;
                Console.WriteLine($"\n\n{tabs}Question {i + 1}:\n");
                Console.Write($"{tabs}Enter Question Header: ");
                do
                {
                    header = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(header))
                        Console.Write($"{tabs}Question header cannot be empty. Please enter a valid question header: ");
                    else break;
                } while (true);

                string body;
                Console.Write($"{tabs}Enter Question Body: ");
                do
                {
                    body = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(body))
                        Console.Write($"{tabs}Question body cannot be empty. Please enter a valid question body: ");
                    else break;

                } while (true);

                int marks;
                Console.Write($"{tabs}Enter Question Marks (1 : 3): ");
                do
                {
                    if (!int.TryParse(Console.ReadLine(), out marks) || marks < 1 || marks > 3)
                        Console.Write($"{tabs}Please enter a valid number of marks (1 : 3): ");
                    else break;
                } while (true);

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
            Program.LoadingMessage($"Creating {examType} Exam");
        }

        public abstract void ShowExam();
    }
    public class PracticeExam : Exam
    {
        public PracticeExam(int time, Subject subject, int questionNumber) : base(time, subject, questionNumber) { }

        public override void ShowExam()
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));
            string answers = $"\n\n{tabs}Correct Answers for The Practice Exam.\n{stars}\n";

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
        public FinalExam(int time, Subject subject, int questionNumber) : base(time, subject, questionNumber) { }

        public override void ShowExam()
        {
            Console.WriteLine($"{tabs}Your Answers: \n\n");

            for (int i = 0; i < QuestionsNumber; ++i)
            {
                Console.WriteLine($"{tabs}{i + 1}) {Questions[i]}");
                if (Questions[i] is ChooseAllQuestion caq)
                {

                    string[] ans = Answers[i].Split(',');
                    Console.Write($"{tabs}Your Answer(s): ");
                    Console.WriteLine(string.Join(" & ", ans.Select(s => int.TryParse(s, out int num) ? $"{num}) {caq.Options[num - 1]}" : "")));

                }
                else if (Questions[i] is ChooseOneQuestion coq)
                {
                    //int idx = coq.CorrectOptionIndex;
                    ////string correctOption = coq.Options[idx];
                    //string correctOption = $"{idx}) {coq.Options[idx - 1]}";
                    //answers += correctOption;
                    Console.WriteLine($"{tabs}Your Answer: {Answers[i]}) {coq.Options[int.Parse(Answers[i]) - 1]}\n");
                }
                else
                {
                    string ans = Answers[i];
                    string t = (ans == "1") ? "TRUE" : "FALSE";
                    Console.WriteLine($"{tabs}Your Answer: {ans}) {t}\n");
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
