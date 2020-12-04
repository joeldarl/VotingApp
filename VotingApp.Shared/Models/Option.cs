using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VotingApp.Shared.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        [JsonIgnore]
        [ForeignKey("PollId")]
        public virtual Poll Poll { get; set; }
        public int PollId { get; set; }
        [StringLength(200, ErrorMessage = "Option is too long.")]
        public string Body { get; set; }
        public int Votes { get; set; }
    }
}