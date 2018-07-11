// <copyright file="Story.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PayItForward.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Story : BaseModel<Guid>
    {
        private readonly ICollection<Donation> donations;

        public Story()
        {
            this.donations = new HashSet<Donation>();
        }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Title { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ImageUrl { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal GoalAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CollectedAmount { get; set; }

        public bool IsClosed { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Category Category { get; set; }

        public Guid CategoryId { get; set; }

        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string DocumentUrl { get; set; }

        public ICollection<Donation> Donations => this.donations;

        public bool IsRemoved { get; set; }
    }
}
