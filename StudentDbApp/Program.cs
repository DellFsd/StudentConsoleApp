using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleTables;

namespace StudentDbApp
{
    class Program
    {
        private StreamReader _studentData;
        private List<Student> _students = new();
        static void Main(string[] args)
        {

            Program _obj = new Program();
            _obj.BeginShow();
            Console.ReadLine();
        }

        public void BeginShow()
        {
            _studentData = LoadStudentDbFile();
            ReadStudentData();
            FormatScreen();
            DisplayStudentRecords(_students);

            PerformOps();
        }

        private void FormatScreen()
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
        }

        private void PerformOps()
        {
            char ip;
            do
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();
                DisplayStudentRecords(_students);
                Console.WriteLine("Select Below Operations");
                Console.WriteLine("Press 1 to Sort Records");
                Console.WriteLine("Press 2 to Search Student Record By First Name");
                Console.WriteLine("Press 0 for Exit");
                ip = Console.ReadKey().KeyChar;
                Console.WriteLine("\n Key Pressed {0}",ip);
                if (ip == '1')
                {
                    SortStudentRecord();
                }
                else if (ip == '2')
                {
                    SearchStudentRecord();
                }
                else if (ip != '0')
                {
                    Console.WriteLine("Invalid Entry");
                }

            } while (ip != '0');

        }

        private void SearchStudentRecord()
        {
            Console.WriteLine("\n Enter First Name For Search: ");
            var term = Console.ReadLine();
            if (_students.FindAll(std => std.FirstName.Trim().ToLower() == term?.Trim().ToLower()).Count > 0)
            {
                DisplayStudentRecords(_students.FindAll(std => std.FirstName.Trim().ToLower() == term?.Trim().ToLower()).ToList());
            }
            else
            {
                Console.WriteLine("\n No Records Found For entered search text: ");
            }

            Console.ReadLine();
        }

        private void SortStudentRecord()
        {
            Console.WriteLine("Press:\n F: Sort By First Name\n L: Sort By Last Name\n M: Sort By Marks\n R: Sort By Result");
            var input = Console.ReadKey().KeyChar;
            switch (Char.ToUpper(input))
            {
                case 'F':
                        Console.WriteLine("\n Student Records Sorted By First Name");
                    DisplayStudentRecords(_students.OrderBy(std => std.FirstName).ToList());
                    break;
                case 'L':
                    Console.WriteLine("\n Student Records Sorted By Last Name");
                    DisplayStudentRecords(_students.OrderBy(std => std.LastName).ToList());
                    break;
                case 'M':
                    Console.WriteLine("\n Student Records Sorted By Marks Asc");
                    DisplayStudentRecords(_students.OrderBy(std => std.Marks).ToList());
                    break;
                case 'R':
                    Console.WriteLine("\n Student Records Sorted By Result");
                    DisplayStudentRecords(_students.OrderBy(std => std.ExamPassed).ToList());
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }

            Console.ReadLine();
        }

        private void DisplayStudentRecords(List<Student> students)
        {
            ConsoleTable
                .From<Student>(students)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Configure(o => o.EnableCount = true)
                .Write(Format.Alternative);
        }

        private void ReadStudentData()
        {
            int i = 1;
            //Read the first line of text
           var line = _studentData.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                string[] words = line.Split('|');
                AddStudentRecordToList(words,i);
                //Read the next line
                line = _studentData.ReadLine();
                i++;
            }
        }

        private void AddStudentRecordToList(string[] words,int id)
        {
            Student std = new Student();
            std.Id = id;
            std.FirstName = words[0];
            std.LastName = words[1];
            std.Program = words[2];
            std.Subject = words[3];
            std.Marks = Convert.ToInt32(words[4]);
            std.ExamPassed = words[5];
            _students.Add(std);
        }

        private StreamReader LoadStudentDbFile()
        {
            StreamReader sr;
            string targetDir = @"c:\temp";
            string fileName = "StudentRecord.txt";
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            if (File.Exists(string.Concat(targetDir, @"\", fileName)))
            {
                sr = new StreamReader(string.Concat(targetDir, @"\", fileName));
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                Console.WriteLine("File not Accessible");
                throw new FileNotFoundException($"File {fileName} not present at {targetDir} path");
            }
            return sr;

        }
    }
}
