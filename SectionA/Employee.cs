using System;

namespace HROnboardingSystem
{
    public class Employee
    {
        // Attributes
        public string Nric { get; set; }
        public string FullName { get; set; }
        public string Salutation { get; set; }
        public DateTime StartDate { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string MobileNo { get; set; }
        public string HireType { get; set; }
        public double Salary { get; set; }
        public double MonthlyPayout { get; set; } = 0.0;

        // Constructor
        public Employee(string nric, string fullName, string salutation, DateTime startDate, 
                       string designation, string department, string mobileNo, string hireType, double salary)
        {
            Nric = nric;
            FullName = fullName;
            Salutation = salutation;
            StartDate = startDate;
            Designation = designation;
            Department = department;
            MobileNo = mobileNo;
            HireType = hireType;
            Salary = salary;
            MonthlyPayout = 0.0;
        }

        // Method for Corporate Admin Department: FullName, Designation, Department
        public string FormatForCorpAdmin()
        {
            return $"{FullName}, {Designation}, {Department}";
        }

        // Method for Procurement Department: Salutation, FullName, MobileNo, Designation, Department
        public string FormatForProcurement()
        {
            return $"{Salutation}, {FullName}, {MobileNo}, {Designation}, {Department}";
        }

        // Method for IT Department: Nric, FullName, StartDate, Department, MobileNo
        public string FormatForITDepartment()
        {
            return $"{Nric}, {FullName}, {StartDate:dd/MM/yyyy}, {Department}, {MobileNo}";
        }
    }
}
