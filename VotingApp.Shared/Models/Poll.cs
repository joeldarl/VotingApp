using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

namespace VotingApp.Shared.Models
{
    public class Poll
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PollId { get; set; }

        public string UrlCode { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Question is too long.")]
        public string Question { get; set; }

        [EnsureMinimumElements(2, ErrorMessage = "At least two options are required.")]
        public virtual IList<Option> Options { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }

    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList<Option>;
            if (list != null)
            {
                return list.Count(p => !String.IsNullOrEmpty(p.Body)) >= _minElements;
            }
            return false;
        }
    }
}