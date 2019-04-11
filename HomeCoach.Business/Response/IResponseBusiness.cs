namespace HomeCoach.Business.Response
{
    using Models;

    public interface IResponseBusiness
    {
        string BuildResponse(HomeCoachData data, string intent);
        string BuildResponse(HomeCoachData data, string intent, string locale);
    }
}