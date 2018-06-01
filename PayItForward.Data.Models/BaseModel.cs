namespace PayItForward.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using PayItForward.Data.Models.Abstraction;

    public abstract class BaseModel : IAuditInfo, IDeletableEntry
    {
        [Key]
        public virtual string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
