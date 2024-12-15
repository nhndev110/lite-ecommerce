﻿namespace SV21T1020547.DomainModels
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; } = null;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public bool IsWorking { get; set; }
    }
}