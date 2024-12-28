using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseEnroll.Models.VM
{
    public class TraineerVM
    {
        public TraineerVM() 
        { 
            this.CourseList = new List<int>();
        }
        public int TraineeId { get; set; }
        public string TraineeName { get; set; }
        public int  Age { get; set; }
        public DateTime DOB { get; set; }
        public string MorningShift { get; set;}
        public string Picture { get; set; } 
        public HttpPostedFileBase PictureFile { get; set; }
        public List<int> CourseList { get; set; }

    }
}