namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Course()
        //{
        //    Attendances = new HashSet<Attendance>();
        //}

        public int Id { get; set; }

        [StringLength(128)]
        public string lecturerId { get; set; }

        [StringLength(255)]
        public string Name;

        [StringLength(255)]
        public string LectureName;

        [StringLength(255)]
        public string Place { get; set; }

        public DateTime? Datetime { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public List<Category> listCategory = new List<Category>();

        public bool isLogin = false;
        public bool isShowGoing = false;
        public bool isShowFollow = false;
    }
}
