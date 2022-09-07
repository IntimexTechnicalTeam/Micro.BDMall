namespace BDMall.BLL
{
    public interface ILoginBLL:IDependency
    {

        Task<SystemResult> Login(LoginInput input);

        Task<SystemResult> AdminLogin(UserDto user);

        Task<UserDto> CheckAdminLogin(LoginInput input);
    }
}
