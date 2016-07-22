using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knowledge_Management.Models
{
    public class Employee
    {
        public Employee(){ }
        public int Emp_Id { get; set; }
        public string Emp_fname { get; set; }
        public string Emp_lname { get; set; }
        public string Emp_pcode { get; set; }
        public int Dep_Id { get; set; }
        public string Dep_Name { get; set; }
        public int Job_Id { get; set; }
        public string Job_Name { get; set; }
        public bool data_entry { get; set; }
        public bool data_view { get; set; }
    }
}