# Console Exam System

A lightweight, native C# console application for creating and taking practice and final exams interactively.

## ✨ Features

- Create **Practice** and **Final** exams.
- Add **True/False**, **Multiple Choice**, and **Multiple Answer** questions.
- Randomized exam selection and timed sessions.
- Clean, tabbed console interface with progress indicators.

## 🛠️ Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download)
- Windows, macOS, or Linux (with .NET CLI support)

## 🗂️ Project Structure
console-exam-system/
├── src/
│   ├── Models/
│   │   ├── Exam.cs
│   │   ├── Question.cs
│   │   └── Subject.cs
│   ├── Services/
│   │   ├── MenuService.cs
│   │   ├── ExamService.cs
│   │   └── QuestionBuilder.cs
│   └── Program.cs
├── tests/            # (optional) NUnit or xUnit tests
├── .gitignore
└── README.md


## 🚀 Getting Started

1. **Clone the repo**
   ```bash
   git clone https://github.com/Ahmed-Elhosiny/console-exam-system.git
   cd console-exam-system/src

