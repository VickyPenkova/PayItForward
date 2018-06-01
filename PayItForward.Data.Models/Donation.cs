// <copyright file="Donation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PayItForward.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Donation : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DonationId { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public Story Story { get; set; }

        public Guid StoryId { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
    }
}
