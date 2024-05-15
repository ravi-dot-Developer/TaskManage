using Database.@enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class Tasks

    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }



    }
}
