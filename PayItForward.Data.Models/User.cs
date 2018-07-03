// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PayItForward.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;

    [Table("Users")]
    public class User : IdentityUser
    {
        private readonly ICollection<Story> stories;
        private readonly ICollection<Donation> donations;

        public User()
        {
            this.donations = new HashSet<Donation>();
            this.stories = new HashSet<Story>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        public decimal AvilableMoneyAmount { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string AvatarUrl { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Story> Stories => this.stories;

        public ICollection<Donation> Donations => this.donations;
    }
}
