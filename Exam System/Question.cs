namespace Exam_System
{
    public abstract class Question
    {
        protected const string tabs = "\t\t\t\t\t\t\t";
        protected const string stars = $"{tabs}*****************************************\n";
        string body;
        string header;
        int marks;

        public string Body
        {
            get => body;
            set => body = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Question body cannot be null or empty.") : value;
        }
        public string Header
        {
            get => header;
            set => header = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Question header cannot be null or empty.") : value;
        }
        public int Marks
        {
            get => marks;
            set => marks = (value < 1 || value > 3) ? throw new ArgumentException("Question marks range (1 : 3).") : value;
        }

        public Question(string body, string header, int marks)
        {
            Body = body;
            Header = header;
            Marks = marks;
        }
        public Question() : this("Q Body", "Q Header", 1) { }


    }

    internal class ChooseOneQuestion : Question
    {
        public string[] options;
        int correctOptionIndex;
        public string[] Options
        {
            set => options = (value == null || value.Length != 4) ? throw new ArgumentException("Options must be exactly 4.") : value;
            get => options;
        }
        public int CorrectOptionIndex
        {
            get => correctOptionIndex;
            set => correctOptionIndex = (value < 1 || value > 4) ? throw new ArgumentException("Correct option index (1 : 4).") : value;
        }
        public ChooseOneQuestion(string body, string header, int marks, string[] options, int correctOptionIndex)
        : base(body, header, marks)
        {
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
        }
        public override string ToString()
        {
            string opts = string.Join("\n", Options.Select((o, i) => $"{tabs}{i + 1}. {o}"));
            return $"{Header}: {Body}\n{tabs}Options:\n{opts}";
        }



    }
    internal class ChooseAllQuestion : Question
    {
        string[] options;
        int[] correctOptionIndices;

        public ChooseAllQuestion(string body, string header, int marks, string[] options, int[] correctOptionIndices) : base(body, header, marks)
        {
            Options = options;
            CorrectOptionIndices = correctOptionIndices;
        }
        public string[] Options
        {
            get => options;
            set => options = (value == null || value.Length != 4) ? throw new ArgumentException("Options must be exactly 4.") : value;
        }
        public int[] CorrectOptionIndices
        {
            get => correctOptionIndices;
            set
            {
                if (value == null || value.Length == 0) throw new ArgumentException("At least one correct option is required.");
                foreach (int v in value)
                {
                    if (v < 1 || v > 4) throw new ArgumentException("Each correct option index must be between 1 and 4.");
                }

                correctOptionIndices = value;
            }
        }

        public override string ToString()
        {
            string opts = string.Join("\n", Options.Select((o, i) => $"{tabs}{i + 1}. {o}"));
            return $"{Header}: {Body}\n{tabs}Options:\n{opts}\n";
        }




    }
    internal class TrueOrFalseQuestion : Question
    {
        string answer;
        public string Answer
        {
            get => answer;
            set
            {
                value = value.Trim().ToUpper();
                if (string.IsNullOrEmpty(value) || (value != "TRUE" && value != "FALSE" && value != "T" && value != "F")) throw new ArgumentException("Invalid True of False Answer");
                if (value == "T") value = "TRUE";
                if (value == "F") value = "FALSE";
                answer = value;
            }
        }
        public TrueOrFalseQuestion(string body, string header, int marks, string answer) : base(body, header, marks)
        {
            Answer = answer;
        }

        public override string ToString() => $"{Header}: {Body}\n{tabs}Options:\n{tabs}1. TRUE\n{tabs}2. FALSE";


    }
}
