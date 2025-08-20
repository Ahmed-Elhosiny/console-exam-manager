using System.Text.Json;

namespace Exam_System
{
    static class FileManager
    {
        private const string EXAMS_FILE = "exams.json";
        public static List<Exam> exams = [];

        public static void LoadExams()
        {
            try
            {
                if (File.Exists(EXAMS_FILE))
                {
                    string jsonString = File.ReadAllText(EXAMS_FILE);
                    var examDataList = JsonSerializer.Deserialize<List<ExamData>>(jsonString);

                    if (examDataList != null)
                    {
                        foreach (var examData in examDataList)
                        {
                            var exam = ConvertFromExamData(examData);
                            exams.Add(exam);
                        }
                        Console.WriteLine($"Loaded {exams.Count} exam(s) from file.");
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteWithIndent($"Error loading exams: {ex.Message}\n");
            }
        }
        public static void SaveExams()
        {
            try
            {
                var examDataList = new List<ExamData>();

                foreach (var exam in exams)
                {
                    var examData = ConvertToExamData(exam);
                    examDataList.Add(examData);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(examDataList, options);
                File.WriteAllText(EXAMS_FILE, jsonString);

                Console.WriteLine("Exams saved successfully!");
            }
            catch (Exception ex)
            {
                Helpers.WriteWithIndent($"Error saving exams: {ex.Message}\n");
            }
        }

        // Convert Exam to ExamData for serialization
        static ExamData ConvertToExamData(Exam exam)
        {
            var examData = new ExamData
            {
                Time = exam.Time,
                QuestionsNumber = exam.QuestionsNumber,
                TotalMarks = exam.TotalMarks,
                Subject = new SubjectData
                {
                    Name = exam.Subject.Name,
                    Instructor = exam.Subject.Instructor,
                    Code = exam.Subject.Code
                },
                Questions = new List<QuestionData>()
            };

            foreach (var question in exam.Questions)
            {
                var questionData = new QuestionData
                {
                    Header = question.Header,
                    Body = question.Body,
                    Marks = question.Marks
                };

                if (question is TrueOrFalseQuestion tfq)
                {
                    questionData.Type = "TrueOrFalse";
                    questionData.Answer = tfq.Answer;
                }
                else if (question is ChooseOneQuestion coq)
                {
                    questionData.Type = "ChooseOne";
                    questionData.Options = coq.Options;
                    questionData.CorrectOptionIndex = coq.CorrectOptionIndex;
                }
                else if (question is ChooseAllQuestion caq)
                {
                    questionData.Type = "ChooseAll";
                    questionData.Options = caq.Options;
                    questionData.CorrectOptionIndices = caq.CorrectOptionIndices;
                }

                examData.Questions.Add(questionData);
            }

            return examData;
        }

        // Convert ExamData back to Exam for loading
        static Exam ConvertFromExamData(ExamData examData)
        {
            var subject = new Subject(
                examData.Subject.Name,
                examData.Subject.Instructor,
                examData.Subject.Code
            );

            var exam = new Exam(examData.Time, subject, examData.QuestionsNumber);

            foreach (var questionData in examData.Questions)
            {
                switch (questionData.Type)
                {
                    case "TrueOrFalse":
                        var tfq = new TrueOrFalseQuestion(
                            questionData.Body,
                            questionData.Header,
                            questionData.Marks,
                            questionData.Answer
                        );
                        exam.Questions.Add(tfq);
                        break;

                    case "ChooseOne":
                        var coq = new ChooseOneQuestion(
                            questionData.Body,
                            questionData.Header,
                            questionData.Marks,
                            questionData.Options,
                            questionData.CorrectOptionIndex
                        );
                        exam.Questions.Add(coq);
                        break;

                    case "ChooseAll":
                        var caq = new ChooseAllQuestion(
                            questionData.Body,
                            questionData.Header,
                            questionData.Marks,
                            questionData.Options,
                            questionData.CorrectOptionIndices
                        );
                        exam.Questions.Add(caq);
                        break;
                }
            }

            return exam;
        }
    }

    public class ExamData
    {
        public int Time { get; set; }
        public int QuestionsNumber { get; set; }
        public SubjectData Subject { get; set; }
        public List<QuestionData> Questions { get; set; }
        public int TotalMarks { get; set; }
    }

    public class SubjectData
    {
        public string Name { get; set; }
        public string Instructor { get; set; }
        public string Code { get; set; }
    }

    public class QuestionData
    {
        public string Type { get; set; } // "TrueOrFalse", "ChooseOne", "ChooseAll"
        public string Header { get; set; }
        public string Body { get; set; }
        public int Marks { get; set; }
        public string[] Options { get; set; } // null for TrueOrFalse
        public string Answer { get; set; } // for TrueOrFalse
        public int CorrectOptionIndex { get; set; } // for ChooseOne
        public int[] CorrectOptionIndices { get; set; } // for ChooseAll
    }
}