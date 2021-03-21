using System;
using System.Text;
using System.Threading;

namespace advt.Service.Provider
{
    public class AuthorizationServices : IProvider.IAuthorizationServices
    {
        private string sitename = "advt.site";

        public Entity.advt_users Login(string userName, string password, int CookieTime = 0)
        {
            return MemberProvider.MyWebSecurity.Login(userName, password, CookieTime);
        }

        public Entity.advt_users Authenticate(string userName, string password)
        {
            /*
             null:  UserName or Password is Error, Please check it!

             */
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return null;
            else
            {
                userName = userName.Trim();
            }

            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)//)
            {
                //string _TempID = null;
                Entity.advt_users user = BLL.Login.Get_User(userName);
                // 2. 存在 则找 TempID。找不到，先做Md5加密，再和Db做验证，
                string password_MD5 = getMd5Hash(userName, password);

                string ip = Common.wbRequest.GetIP();
                //MembershipWebservice.MembershipWebserviceSoapClient sso = new MembershipWebservice.MembershipWebserviceSoapClient();
                ////MembershipWebservice.MembershipWebservice sso = new MembershipWebservice.MembershipWebservice();
                //try
                //{
                //    _TempID = sso.login(userName, password, sitename, ip);
                //}
                //catch
                //{
                //    _TempID = null;
                //}
                if (user != null)
                {
                    return user;
                }
                else
                {
                    string ErrorMSG = string.Empty;

                    user = new Entity.advt_users();

                    int rst = Update_eRMA_User(user, userName, password_MD5, ref ErrorMSG);

                    if (string.IsNullOrEmpty(ErrorMSG))
                    {
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                Entity.advt_users user = BLL.Login.Get_User(userName);

                return user;
            }
            
        }

        public string getMd5Hash(string email, string input)
        {
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            email = email.Trim().ToLower();
            input = input.Trim();
            Byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(email + input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 根据用户名密码登陆，并将用户信息保存到執行緒中
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="cookieDomain"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        public int SetUserAuthIn(string loginName, string password, string cookieDomain, bool RememberMe)
        {
            // UserLogin(loginName, string.Empty, RememberMe, Config.BaseConfigs.Cookiedomain);

            return 1;
        }

        public int Update_eRMA_User(Entity.advt_users _user, string UserName, string password_MD5, ref string ErrorMSG)
        {

            string include_str = "phone,firstname,lastname,username,password,lastip,createdate,status";
            _user.username = UserName;
            _user.phone = "";
            _user.firstname = "";
            _user.lastname = "";
            _user.lastip = Common.wbRequest.GetIP();
            _user.createdate = DateTime.Now;
            _user.status = (int)Entity.Status.Normal.Enable;
            _user.password = password_MD5;

            int id = Data.advt_users.Insert_advt_users(_user, include_str.Split(','), new string[] { "id" });
            _user.id = id;

            if (id < 1) ErrorMSG = "System Error!";

            return 1;
        }
    }
}
