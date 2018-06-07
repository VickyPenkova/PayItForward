namespace PayItForward.Data.Models.Abstraction
{
    using System;

    public interface IDeletableEntry
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
