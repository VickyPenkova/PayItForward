namespace PayItForward.Web.Infrastructure.AutoMapper
{
    using global::AutoMapper;

    public class ObjectAutoMapper : ICustomMapper
    {
        public IMapper GetMapper()
        {
            return AutoMapperConfig.Configuration.CreateMapper();
        }
    }
}
