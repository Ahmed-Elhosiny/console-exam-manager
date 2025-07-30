namespace Exam_System
{
    public class Subject
    {
        protected const string tabs = "\t\t\t\t\t\t\t";

        protected const string stars = $"{tabs}*****************************************\n";
        string name, instructor;
        public string Name
        {
            get => name;
            set => name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Name cannot be null or empty.") : value;
        }
        public string Instructor
        {
            get => instructor;
            set => instructor = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Instructor cannot be null or empty.") : value;

        }
        public string Code { get; init; } // Make Code Immutable after initialization

        public Subject() : this("Unknown", "Unknown", "CS150") { }
        public Subject(string _name, string _instructor, string _code)
        {
            Name = _name;
            Instructor = _instructor;
            Code = string.IsNullOrWhiteSpace(_code) ? throw new ArgumentException("Code cannot be null or empty.") : _code;

        }
        public override string ToString() => $"{tabs}Subject Name: {Name}\n{tabs}Code: {Code}\n{tabs}Instructor: Eng.{Instructor}\n";

        public static Subject GetSubject()
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

            return new(subjectName, subjectInstructor, subjectCode);
        }
    }
}

