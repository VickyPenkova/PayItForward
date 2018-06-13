namespace PayItForward.Web.Infrastructure.AutoMapper
{
    using AutoMapper;
    using global::AutoMapper;

    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
