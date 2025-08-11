using System;
using System.IO;
using System.Threading.Tasks;
using HROnboardingSystem;

namespace PayrollCalculator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("HR Payroll Calculator System\n============================");
            
            var employees = ReadHRMasterList();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employee records found.");
                return;
            }
            
            Console.WriteLine($"Successfully loaded {employees.Count} employee records.\n");
            
            await ProcessPayrollAsync(employees);
            await UpdateMasterlistAsync(employees);
            
            Console.WriteLine("\nPayroll processing completed successfully!\nGenerated: HRMasterlistB.txt\n\nPress any key to exit...");
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
        static async Task ProcessPayrollAsync(System.Collections.Generic.List<Employee> employees) =>
            await Task.Run(() =>
            {
                Console.WriteLine("PAYROLL CALCULATION RESULTS\n============================");
                double totalPayroll = 0;
                
                foreach (var emp in employees)
                {
                    emp.MonthlyPayout = emp.HireType.ToLower() switch
                    {
                        "parttime" => 0.4 * emp.Salary,
                        "hourly" => 0.2 * emp.Salary,
                        _ => emp.Salary
                    };
                    
                    Console.WriteLine($"{emp.FullName} ({emp.Nric})\n{emp.Designation}\n{emp.HireType} Payout: ${emp.MonthlyPayout:F0}\n{new string('-', 40)}");
                    totalPayroll += emp.MonthlyPayout;
                }
                
                Console.WriteLine($"Total Payroll Amount: ${totalPayroll:F0} to be paid to {employees.Count} employees.\n");
            });
        static async Task UpdateMasterlistAsync(System.Collections.Generic.List<Employee> employees) =>
            await Task.Run(() =>
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "HRMasterlistB.txt");
                using var writer = new StreamWriter(filePath);
                
                foreach (var emp in employees)
                {
                    writer.WriteLine($"{emp.Nric}|{emp.FullName}|{emp.Salutation}|{emp.StartDate:dd/MM/yyyy}|{emp.Designation}|{emp.Department}|{emp.MobileNo}|{emp.HireType}|{emp.Salary}|{emp.MonthlyPayout:F2}");
                }
            });
    }
}
