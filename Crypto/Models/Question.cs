#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class Question
    {
        public int Id { get; set; }
        public string QuestionDescription { get; set; }
        public DateTime Date { get; set; }
        public int? UId { get; set; }

        public virtual Login User { get; set; }

    }
}