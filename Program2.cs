using System;
using TeacherApp;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1. Створити студента");
            Console.WriteLine("2. Створити викладача");
            Console.WriteLine("3. Показати всіх студентів");
            Console.WriteLine("4. Показати всіх викладачів");
            Console.WriteLine("0. Вихід");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") CreateStudent();
            else if (choice == "2") CreateTeacher();
            else if (choice == "3") ShowStudents();
            else if (choice == "4") ShowTeachers();
            else if (choice == "0") break;
            else Console.WriteLine("Невірний вибір!");

            Console.WriteLine();
        }
    }

    static void CreateStudent()
    {
        Console.Write("ФИО: ");
        string fullName = Console.ReadLine();

        Console.Write("возраст: ");
        int age = int.Parse(Console.ReadLine());

        AppContext db = new AppContext();

        Student student = new Student();
        student.FullName = fullName;
        student.Age = age;

        db.Students.Add(student);
        db.SaveChanges();
        db.Dispose();

        Console.WriteLine("добавлен");
    }

    static void CreateTeacher()
    {
        Console.Write("ФИО: ");
        string fullName = Console.ReadLine();

        Console.Write("возраст: ");
        int age = int.Parse(Console.ReadLine());

        Console.Write("зарплата: ");
        decimal salary = decimal.Parse(Console.ReadLine());

        AppContext db = new AppContext();

        Teacher teacher = new Teacher();
        teacher.FullName = fullName;
        teacher.Age = age;
        teacher.Salary = salary;

        db.Teachers.Add(teacher);
        db.SaveChanges();
        db.Dispose();

        Console.WriteLine("добавлен");
    }

    static void ShowStudents()
    {
        AppContext db = new AppContext();

        var students = db.Students.ToList();
        db.Dispose();

        if (students.Count == 0)
        {
            Console.WriteLine("нету");
            return;
        }

        Console.WriteLine("ID | ФИО | Возрост");
        Console.WriteLine("---------------");

        foreach (var s in students)
        {
            Console.WriteLine(s.Id + " | " + s.FullName + " | " + s.Age);
        }
    }

    static void ShowTeachers()
    {
        AppContext db = new AppContext();

        var teachers = db.Teachers.ToList();
        db.Dispose();

        if (teachers.Count == 0)
        {
            Console.WriteLine("нету");
            return;
        }

        Console.WriteLine("ID | ФИО | Возрост | Зарплата");
        Console.WriteLine("--------------------------");

        foreach (var t in teachers)
        {
            Console.WriteLine(t.Id + " | " + t.FullName + " | " + t.Age + " | " + t.Salary);
        }
    }
}
