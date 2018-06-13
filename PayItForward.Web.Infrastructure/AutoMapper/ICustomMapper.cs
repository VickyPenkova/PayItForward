namespace PayItForward.Web.Infrastructure.AutoMapper
{
    using global::AutoMapper;

    public interface ICustomMapper
    {
        IMapper GetMapper();
    }
}
