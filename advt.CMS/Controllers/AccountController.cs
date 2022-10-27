using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using advt.Entity;
using advt.Manager;
using System.Text.RegularExpressions;
using advt.Common;
using System.Text;
using FormsAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using advt.CMS.Models.ExamModel;
using advt.Entity.Global;
using System.Net;
using System.IO;

namespace advt.Web.Controllers
{
    public class AccountController : BaseController
    {

        public AccountController()
        {
            ViewBag.active = "Account";
        }
        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl,string token,string userNo, string sys, string param)
        {
            LoginModel model = new LoginModel();
                string tokenstr = System.Web.HttpUtility.UrlEncode(token, Encoding.GetEncoding("GB2312"));
            model.token = tokenstr;
            model.userNo = userNo;
            model.sys = sys;
            model.param = param;
            Manager.Login.ClearSession();
            if (Manager.Login.ValidateUser)
            {
                alert("您已经登录，请勿重复登录!", Url.Action(string.Empty, "Home", new { Area = "" }));
            }

            return View(model) ;
        }

        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public ActionResult Login(Model.LoginModel model, string returnUrl)
        //{

        //    Service.IProvider.IAuthorizationServices service= new Service.Provider.AuthorizationServices();

        //    if (ModelState.IsValid)
        //    {
        //        Entity.advt_users user = service.Authenticate(model.UserName, model.Password);
        //        if (user != null) //验证通过
        //        {
        //            //写FormsAuthentication
        //            SetUserAuthIn(user.id.ToString(), model.Password, string.Empty, false);

        //            //写入Cookie，无需登入。
        //            XUtils.WriteUserCookie(user, model.CookieTime ?? 0, Config.BaseConfigs.Passwordkey, 1);
        //            return Json(new { IsLogin = "Pass"}, JsonRequestBehavior.AllowGet);
        //            //return RedirectToAction("index", "PEMain");
        //        }
        //    }

        //    ModelState.AddModelError("", "用户名或者密码错误!");
        //    return Json(new { IsLogin = "Fail" }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult Login(Model.LoginModel model, string returnUrl, string userNo, string token,string sys, string param)
        {
            var IsLogin = "";
            try
            {
                sys = "智能考试平台";
                param = "%2B5aScAuNL36xR3W3grHiNR62a%2BhviynjDoUJYbN%2BACTDJRVyr4peJhRGD%2F%2BrApcW1vIocH6BbHMAq%2FFVMDYh1g%3D%3D";
                Entity.advt_users users = new advt_users();
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userNo))
                {
                    var _httpClient = new HttpClient();
                    var URL = $"https://akmclearning.advantech.com.cn/SSO/ValidateUserAuth?token={token}&userNo={userNo}";
                    var resultObj = _httpClient.PostAsync(URL, null).Result.Content.ReadAsStringAsync();
                    var result = "";
                    if (resultObj == null)
                    {
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "" });
                    }
                    else
                    {
                        result = resultObj.Result;
                    }
                    if (result != null)
                    {
                        if (result == "true")
                        {
                            var uname = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { UserCode = userNo });
                            if (uname != null)
                            {
                                Service.IProvider.IAuthorizationServices service = new Service.Provider.AuthorizationServices();
                                users = service.Authenticate(uname.EamilUsername, "123");
                            }
                            else
                            {
                                IsLogin = "用户名不存在考试平台系统内";
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(token))
                {
                    var _httpClient = new HttpClient();
                    var URL = $" http://job.advantech.com.cn/HRConsole/SSO/ValidateUserFromExam?token={token}";
                    var resultObj = _httpClient.PostAsync(URL, null).Result.Content.ReadAsStringAsync();
                    var result = "";
                    if (resultObj == null)
                    {
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "" });
                    }
                    else
                    {
                        result = resultObj.Result;
                    }
                    if (!string.IsNullOrEmpty(result))
                    {
                        var uname = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { UserCode = result });
                        if (uname != null)
                        {
                            Service.IProvider.IAuthorizationServices service = new Service.Provider.AuthorizationServices();
                            users = service.Authenticate(uname.EamilUsername, "123");
                        }
                        else
                        {
                            IsLogin = "用户名不存在考试平台系统内";
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(sys) && !string.IsNullOrEmpty(param))
                {
                    string postUrl = "http://172.21.203.23:8089/WebCenter/Api/WebApi/ValidateLoginEncode";//正式
                    var postDataStr = new WebCenter()
                    {
                        System = sys,
                        Param = param
                    };

                    string returnMsg = HttpPostString(postUrl, Newtonsoft.Json.JsonConvert.SerializeObject(postDataStr));
                    var sdd = Newtonsoft.Json.JsonConvert.DeserializeObject<WebCenter>(returnMsg);
                    if (sdd.Authorized)
                    {
                        var uname = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { UserCode = sdd.UserMsg.OPID });
                        if (uname != null)
                        {
                            Service.IProvider.IAuthorizationServices service = new Service.Provider.AuthorizationServices();
                            users = service.Authenticate(uname.EamilUsername, "123");
                        }
                        else
                        {
                            IsLogin = "用户名不存在考试平台系统内";
                        }
                    }
                }
                else
                {
                    string[] SplitAccount = new string[] { };
                    var username = "";

                    Regex RegEmail = new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
                    Match m = RegEmail.Match(model.UserName);
                    //工号
                    var wuser = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { UserCode = model.UserName });
                    var cuser = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = model.UserName });
                    if (wuser != null)
                    {
                        Service.IProvider.IAuthorizationServices services = new Service.Provider.AuthorizationServices();
                        users = services.EmailAuthenticate(wuser.CommpanyEmail, model.Password);
                        if (users != null) //验证通过
                        {
                            username = wuser.EamilUsername;
                        }
                        else
                        {
                            IsLogin = "EZ账号登陆不成功";
                        }
                    }
                    else
                    {

                        if (cuser != null)
                        {
                            var acc = "acn\\" + cuser.EamilUsername.Trim();
                            SplitAccount = acc.Split('\\');
                            username = cuser.EamilUsername;
                        }
                        if (cuser != null)
                        {
                            if (SplitAccount.Length > 1)
                            {
                                String adPath = ""; //Fully-qualified Domain Name
                                switch (SplitAccount[0].ToLower().Trim())
                                {
                                    case "acn":
                                        adPath = "LDAP://acn.advantech.corp"; //acn
                                        break;
                                    case "aeu":
                                        adPath = "LDAP://aeu.advantech.corp"; //advantech
                                        break;
                                    case "aus":
                                        adPath = "LDAP://aus.advantech.corp"; //advantech
                                        break;
                                    case "advantech":
                                        adPath = "LDAP://advantech.corp";//advantech
                                        break;
                                    default:
                                        adPath = "LDAP://acn.advantech.corp"; //acn
                                        break;
                                }
                                LdapAuthentication adAuth = new LdapAuthentication(adPath);
                                string password = model.Password.Trim();

                                if (true == adAuth.IsAuthenticated(SplitAccount[0], SplitAccount[1], model.Password))
                                {
                                    Service.IProvider.IAuthorizationServices service = new Service.Provider.AuthorizationServices();
                                    users = service.Authenticate(username, model.Password);

                                }
                                else
                                {
                                    IsLogin = "用户名/账号不正确";
                                }

                            }
                        }
                    }

                    if (wuser == null && cuser == null)
                    {
                        IsLogin = "用户名/工号不存在";
                    }
                }
                if (string.IsNullOrEmpty(IsLogin) && !string.IsNullOrEmpty(users.username))
                {
                    SetUserAuthIn(users.username.ToString(), users.password, string.Empty, false);
                    //写入Cookie，无需登入。

                    var LF = Guid.NewGuid().ToString();
                    //写内存
                    Manager.Login.Lock_Flag = LF;
                    //写本地
                    Utils.WriteCookie("ALock", LF);
                    users.msn = LF;
                    advt.Data.advt_users.Update_advt_users(users, null, new string[] { "id" });
                    XUtils.WriteUserCookie(users, model.CookieTime ?? 0, Config.BaseConfigs.Passwordkey, 1);
                    IsLogin = "Pass";
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsLogin= ex.Message }, JsonRequestBehavior.AllowGet);
                throw;
            }
            //ModelState.AddModelError("", "用户名或者密码错误!");
            return Json(new { IsLogin  }, JsonRequestBehavior.AllowGet);
          
        }
        public static string HttpPostString(string url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();

            return retString;
        }
        [MyAuthorize]
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            //this.UserContext.msn = "";
            //Data.advt_users.Update_advt_users(this.UserContext, null, new string[] { "id" });
            XUtils.ClearCookie();
            Manager.Login.ClearSession();
            return RedirectToAction("Login", "Account");
        }


        //
        // GET: /Account/Register

        public ActionResult Register(int? id)
        {
            return RedirectToAction("Login", "Account");
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(int? id, Model.RegisterModel model)
        {
            if (Regex.IsMatch(model.UserName, "test\\d\\d"))
            {
                msg("抱歉，本系统内测，系统暂时不提供注册功能！有问题请QQ联系吴总。");
                return View();
            }

            if (ModelState.IsValid)
            {
                if (!BLL.Login.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("UserName", "用户名必须是数字,字母或者下划线组成!");
                }
            }
            if (ModelState.IsValid)
            {
                if (model.UserName.Length < 5)
                {
                    ModelState.AddModelError("UserName", "用户名长度必须不小于六位数!");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.UserName.Length < 2)
                    {
                        ModelState.AddModelError("UserName", "用户名最小长度为2!");
                    }

                    if (ModelState.IsValid && !Regex.IsMatch(model.UserName, "^\\w+$"))
                    {
                        ModelState.AddModelError("UserName", "用户名必须由：\"数字,字母,下划线,或汉子,\"组合成!");
                    }

                    if (ModelState.IsValid && model.Password.Length < 6)
                    {
                        ModelState.AddModelError("Password", "密码不能小于六位!");
                    }

                    if (ModelState.IsValid)
                    {
                        Entity.advt_users advt_usersInfo = Data.advt_users.Get_advt_users(new { username = model.UserName });
                        if (advt_usersInfo != null)
                        {
                            ModelState.AddModelError("UserName", string.Format("用户名\"{0}\"已经存在!", model.UserName));
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        Entity.advt_users advt_users = new Entity.advt_users();
                        advt_users.username = model.UserName;
                        advt_users.password = model.Password;
                        advt_users.email = model.email;
                        advt_users.sex = model.sex;
                        //if (id.GetValueOrDefault(0) > 0)
                        //{
                        //    Entity.advt_users _wbusers = Data.advt_users.Get_advt_users(id ?? 0);
                        //}
                        //int rst = MyWebSecurity.CreateUserAndAccount(advt_users);
                        //if (rst > 0)
                        //{
                        //    MyWebSecurity.Login(model.UserName, model.Password);
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError("UserName", string.Format("用户名\"{0}\"创建失败!请联系管理员。", model.UserName));
                        //}
                    }

                    if (ModelState.IsValid)
                    {
                        #region 发送欢迎信息
                        //TODO:发送站点消息。
                        #endregion

                        return RedirectToAction("Login", "Account");
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View("Login", model);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已经存在,请填写不同的用户名.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }


        //
        // GET: /Account/Userinfo
        [MyAuthorize]
        public ActionResult Userinfo(int? id, string returnurl, string ephone, string eemail)
        {
            if (!string.IsNullOrWhiteSpace(ephone)) ModelState.AddModelError("mobile", "请修改您的手机号码!");
            if (!string.IsNullOrWhiteSpace(eemail)) ModelState.AddModelError("email", "请修改您的邮箱地址!");
            List<Entity.advt_user_group> ladvt_user_group = BLL.Login.GetAllUserGroup();
            ViewBag.ladvt_user_group = ladvt_user_group;

            Entity.advt_users info = new advt_users();
            if (UserContext.roles == (int)Entity.Status.RoleStatus.Admin)
            {
                if (id != null)
                    info = Data.advt_users.Get_advt_users(id ?? 0);
                else
                    info = UserContext;
            }
            else
                info = UserContext;

            ViewBag.id = info.id;

            if (info != null)
            {
                info.nickname = (info.nickname ?? string.Empty).Trim();
                info.phone = (info.phone ?? string.Empty).Trim();
                info.qq = (info.qq ?? string.Empty).Trim();
                info.msn = (info.msn ?? string.Empty).Trim();
                info.email = (info.email ?? string.Empty).Trim();
            }
            return View(info);
        }

        //
        // GET: /Account/Userinfo
        [HttpPost]
        [MyAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Userinfo(Entity.advt_users info, string returnurl)
        {
            List<Entity.advt_user_group> ladvt_user_group = BLL.Login.GetAllUserGroup();
            ViewBag.ladvt_user_group = ladvt_user_group;

            Entity.advt_users tp_info = new advt_users();

            if (ModelState.IsValid)
            {
                if (UserContext.roles == (int)Entity.Status.RoleStatus.Admin)
                {
                }
                else
                {
                    if (info.id != UserContext.id)
                    {
                        alert("非法操作!", Url.Action("Userinfo", "Account", new { Area = "" }));
                        ModelState.AddModelError("", "非法操作!");
                    }
                }

                if (ModelState.IsValid && !string.IsNullOrWhiteSpace(info.nickname) && !BLL.Login.CheckUserName(info.nickname))
                {
                    ModelState.AddModelError("nickname", "昵称 格式不正确!");
                }

                if (ModelState.IsValid && string.IsNullOrWhiteSpace(info.firstname))
                {
                    ModelState.AddModelError("firstname", "姓 不能为空!");
                }

                if (ModelState.IsValid && !BLL.Login.CheckUserName(info.firstname))
                {
                    ModelState.AddModelError("firstname", "姓 格式不正确!");
                }

                if (ModelState.IsValid && string.IsNullOrWhiteSpace(info.lastname))
                {
                    ModelState.AddModelError("lastname", "名字 不能为空!");
                }

                if (ModelState.IsValid && !BLL.Login.CheckUserName(info.lastname))
                {
                    ModelState.AddModelError("lastname", "名字 格式不正确!");
                }

                if (ModelState.IsValid && Utils.IsInt(((Entity.Status.Sex)info.sex).ToString()))
                {
                    ModelState.AddModelError("sex", "性别填写有误!");
                }

                if (ModelState.IsValid && !string.IsNullOrWhiteSpace(info.phone) && !BLL.Login.CheckPhone(info.phone))
                {
                    ModelState.AddModelError("phone", "电话 格式不正确!");
                }
                
                if (ModelState.IsValid && !string.IsNullOrWhiteSpace(info.email) && !Common.Utils.IsValidEmail(info.email))
                {
                    ModelState.AddModelError("email", "e-Mail 格式不正确!");
                }

                tp_info = Data.advt_users.Get_advt_users(info.id);

                if (ModelState.IsValid && tp_info == null)
                {
                    ModelState.AddModelError("", "系统错误，请重试!");
                }

                if (ModelState.IsValid)
                {
                    int rst = 0;
                    try
                    {
                        tp_info.nickname = info.nickname;
                        tp_info.phone = info.phone;
                        tp_info.qq = info.qq;
                        tp_info.msn = info.msn;
                        tp_info.email = info.email;
                        tp_info.description = info.description;
                        tp_info.firstname = info.firstname;
                        tp_info.lastname = info.lastname;
                        tp_info.sex = info.sex;

                        if (UserContext.roles == (int)Entity.Status.RoleStatus.Admin)
                        {
                            tp_info.status = info.status;
                            tp_info.usergroupid = info.usergroupid;
                        }

                        rst = Data.advt_users.Update_advt_users(tp_info, null, new string[] { "id" });
                        if (rst == 1)
                        {
                            if (info.id == UserContext.id) Manager.Login.UserContext = null;
                            right("修改成功！");
                            if (!string.IsNullOrEmpty(returnurl)) return Redirect(returnurl);
                        }
                        else
                            alert("插入失败！");
                    }
                    catch
                    {
                        alert("系统错误，请联系管理员！");
                    }
                }
            }

            return View(tp_info);
        }


        [MyAuthorize]
        public ActionResult ChangePassword(int? id)
        {
            Entity.advt_users info = new advt_users();
            if (UserContext.roles == (int)Entity.Status.RoleStatus.Admin)
            {
                if (id != null)
                    info = Data.advt_users.Get_advt_users(id ?? 0);
                else
                    info = UserContext;
            }
            else
                info = UserContext;

            ViewBag.Userinfo = info;
            ViewBag.id = info.id;
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [MyAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(int? id, Model.LocalPasswordModel model)
        {
            Entity.advt_users info = new advt_users();
            if (UserContext.roles == (int)Entity.Status.RoleStatus.Admin)
            {
                if (id != null)
                    info = Data.advt_users.Get_advt_users(id ?? 0);
                else
                    info = UserContext;
            }
            else
                info = UserContext;

            if (info == null)
            {
                ModelState.AddModelError("", "系统错误!");
            }

            if (ModelState.IsValid)
            {
                ViewBag.Userinfo = info;
                ViewBag.id = info.id;

                bool changePasswordSucceeded = false;
                try
                {

                    if (ModelState.IsValid)
                    {
                        string pwd = Utils.MD5(model.OldPassword);

                        if (UserContext.roles == (int)Entity.Status.RoleStatus.Admin || info.password == pwd)
                        {
                            info.password = Utils.MD5(model.NewPassword);

                            int result_i = Data.advt_users.Update_advt_users(info, null, new string[] { "id" });
                            changePasswordSucceeded = result_i == 1;
                        }
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    XUtils.WriteUserCookie(info, 0, Config.BaseConfigs.Passwordkey, 1);
                    right("修改密码成功!");
                }
                else
                {
                    ModelState.AddModelError("", "当前的密码是不正确的或新的密码是无效的!");
                }
            }

            return View(model);
        }




        /// <summary>
        /// 根据用户名密码登陆，并将用户信息保存到Cookies
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="cookieDomain"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        public int SetUserAuthIn(string loginName, string password, string cookieDomain, bool RememberMe)
        {
            UserLogin(loginName, string.Empty, RememberMe, Config.BaseConfigs.Cookiedomain);
            return 1;
        }


        public static void UserLogin(string loginName, string UserData, bool RememberMe, string cookieDomain)
        {
            //设置用户的 cookie 的值
            FormsAuthentication.SetAuthCookie(loginName, RememberMe);

            //获取用户的 cookie 
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(loginName, false, FormsAuthentication.FormsCookiePath);

            //给用户的 cookie 的值加上 cookie 的域 和 过期日期
            //向客户端重写同名的 用户 cookie
            FormsAuthenticationTicket oldTicket = FormsAuthentication.Decrypt(cookie.Value);
            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                1,
                oldTicket.Name,
                oldTicket.IssueDate,
                oldTicket.Expiration,
                oldTicket.IsPersistent,
                UserData == string.Empty ? oldTicket.UserData : UserData,
                FormsAuthentication.FormsCookiePath);
            cookie.Domain = cookieDomain;
            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        [MyAuthorize]
        public ActionResult GetUser()
        {
            var username = this.UserNameContext;
            var type = string.Empty;
            var LRole = Data.ExamRolePart.Get_All_ExamRolPartDetail_Sort(username,"","").Select(y => y.PartName).ToList();
            return Json(new { admin = username, LRole = LRole },JsonRequestBehavior.AllowGet);
        }
    }
}
