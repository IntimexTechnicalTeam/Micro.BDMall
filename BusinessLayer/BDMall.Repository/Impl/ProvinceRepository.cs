namespace BDMall.Repository
{
    public class ProvinceRepository : PublicBaseRepository, IProvinceRepository
    {
        public ProvinceRepository(IServiceProvider service) : base(service)
        {

        }

        public List<ProvinceDto> GetListByCountry(int countryId)
        {
            var result = new List<ProvinceDto>();

            var query = baseRepository.GetList<Province>().Where(d => d.CountryId == countryId && d.IsDeleted == false).ToList();
            result = AutoMapperExt.MapToList<Province, ProvinceDto>(query);

            return result;
        }
        public ProvinceDto GetById(int id)
        {
            var langs = GetSupportLanguage();
            Province item = baseRepository.GetModel<Province>(d => d.Id == id && d.IsDeleted == false);

            var dto = AutoMapperExt.MapTo<ProvinceDto>(item);
            if (dto != null)
            {
                dto.Names = LangUtil.GetMutiLang(item, "Name", langs);
            }
            else
            {
                dto = new ProvinceDto();
                dto.Names = LangUtil.GetMutiLang<Province>(null, "Name", langs);
            }

            return dto;
        }



    }
}
