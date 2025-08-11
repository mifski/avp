using System;
using System.IO;
using System.Linq;

namespace HROnboardingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HR Onboarding System - Employee List Generator\n==============================================");
            
            var employees = ReadHRMasterList();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employee records found.");
                return;
            }
            
            Console.WriteLine($"Successfully loaded {employees.Count} employee records.");
            GenerateFiles(employees);
            
            Console.WriteLine("\nAll department files generated successfully!\nFiles created:\n- CorporateAdmin.txt\n- Procurement.txt\n- ITDepartment.txt\n\nPress any key to exit...");
            Console.ReadKey();
        }
        static System.Collections.Generic.List<Employee> ReadHRMasterList()
        {
            var employees = new System.Collections.Generic.List<Employee>();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "HRMasterlist.txt");
            
            if (!File.Exists(filePath))
                return employees;
            
            foreach (string line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                var parts = line.Split('|');
                if (parts.Length != 9)
                    continue;
                
                try
                {
                    employees.Add(new Employee(
                        parts[0].Trim(),
                        parts[1].Trim(),
                        parts[2].Trim(),
                        DateTime.ParseExact(parts[3].Trim(), "dd/MM/yyyy", null),
                        parts[4].Trim(),
                        parts[5].Trim(),
                        parts[6].Trim(),
                        parts[7].Trim(),
                        double.Parse(parts[8].Trim())
                    ));
                }
                catch
                {
                    // Skip invalid entries
                }
            }
            
            return employees;
        }
        static void GenerateFiles(System.Collections.Generic.List<Employee> employees)
        {
            var departments = new[]
            {
                ("CorporateAdmin", "Corporate"),
                ("Procurement", "Procurement"),
                ("ITDepartment", "IT")
            };
            
            foreach (var (fileName, departmentName) in departments)
            {
                using var writer = new StreamWriter(Path.Combine("..", fileName + ".txt"));
                employees.ForEach(emp => writer.WriteLine(fileName switch
                {
                    "CorporateAdmin" => emp.FormatForCorpAdmin(),
                    "Procurement" => emp.FormatForProcurement(),
                    _ => emp.FormatForITDepartment()
                }));
            }
        }
    }
}
