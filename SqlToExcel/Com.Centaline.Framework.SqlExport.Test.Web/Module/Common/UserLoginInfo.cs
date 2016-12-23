namespace SqlToExcel.Module.Common
{
    public class UserLoginInfo
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public UserLoginInfo(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}