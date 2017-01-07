using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class ProfileViewModel
    {

        public int pk_emp { get; set; }
       
        [Display(Name="First Name:")]
        public string firstName { get; set; }
       
        [Display(Name = "Last Name:")]
        public string lastName { get; set; }
       
        [Display(Name = "Password:")]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters")]
        public string passWord { get; set; }
    }
}