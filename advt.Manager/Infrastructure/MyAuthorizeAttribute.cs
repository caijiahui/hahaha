namespace System.Web.Mvc
{
    using System;
    using System.Web;
    using advt.Manager;
    using advt.Entity;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {

        private Status.RoleStatus m_role;//允许的用户角色
        /// <summary>
        /// 允许的用户角色
        /// </summary>
        public Status.RoleStatus Role
        {
            get { return m_role; }
            set { m_role = value; }
        }

        private bool m_Validata_InUserGroup;

        /// <summary>
        /// 验证页面
        /// </summary>
        public bool Validata_InUserGroup
        {
            get { return m_Validata_InUserGroup; }
            set { m_Validata_InUserGroup = value; }
        }

        /// <summary>
        /// 验证指向的 Area
        /// </summary>
        public string AuthorizeArea { get; set; }

        /// <summary>
        /// 验证指向的 Controller
        /// </summary>
        public string AuthorizeController { get; set; }

        /// <summary>
        /// 必须PageAuthorize = true
        /// </summary>
        public string AuthorizeAction { get; set; }

        bool authorizeflag = true;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //base.AuthorizeCore(httpContext); //基础信息
            //if (Login.Isadmin)
            //{
            //    authorizeflag = true;
            //    return authorizeflag;
            //}
            //if (authorizeflag && (int)m_role != 0) //允许的用户角色
            //{
            //    if (((Login.UserContext.roles ?? 0) & (int)m_role) == 0)
            //        authorizeflag = false;
            //}

            //if (authorizeflag && Validata_InUserGroup)
            //{
            //    int usergroupid = Login.UserContext.usergroupid ?? 0;

            //    authorizeflag = Login.Isadmin || Login.IsAuthenticated_Page(usergroupid, this.AuthorizeArea, this.AuthorizeAction, this.AuthorizeController);
            //}
            var username = "";
            if (Login.UserContext != null)
            {

                username = Login.UserContext.username;
            }

            var user = advt.Data.advt_users.Get_advt_users(new { username = username });
            var c = httpContext.Request.Cookies["ALock"].Value.ToString();
            if (user != null)
            {
                if (httpContext.Request.Cookies["ALock"].Value.ToString() != user.msn) //登录验证
                {
                    ////FormsAuthentication.SignOut();
                    XUtils.ClearCookie();
                    advt.Manager.Login.ClearSession();
                    authorizeflag = false;
                }
                else
                {
                    if (Login.Isadmin)
                    {
                        authorizeflag = true;
                        //return authorizeflag;
                    }
                    if (authorizeflag && (int)m_role != 0) //允许的用户角色
                    {
                        if (((Login.UserContext.roles ?? 0) & (int)m_role) == 0)
                            authorizeflag = false;
                    }

                    if (authorizeflag && Validata_InUserGroup)
                    {
                        int usergroupid = Login.UserContext.usergroupid ?? 0;

                        authorizeflag = Login.Isadmin || Login.IsAuthenticated_Page(usergroupid, this.AuthorizeArea, this.AuthorizeAction, this.AuthorizeController);
                    }
                }
                var roles = advt.Data.ExamRolePartDetail.Get_All_ExamRolePartDetail(new { Controller = this.AuthorizeController, Action = this.AuthorizeAction });
                if (roles.Count != 0)
                {
                    var data = advt.Data.ExamRolePart.Get_All_ExamRolPartDetail_Sort(username, this.AuthorizeAction, this.AuthorizeController);
                    if (data.Count == 0)
                    {
                        authorizeflag = false;
                    }
                }
            }




            //else if (Login.UserContext.msn == null) //登录验证
            //{
            //    authorizeflag = false;
            //}



            return authorizeflag;

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (String.IsNullOrEmpty(this.AuthorizeArea))
                this.AuthorizeArea = filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"].ToString();

            if (String.IsNullOrEmpty(this.AuthorizeController))
                this.AuthorizeController = filterContext.RouteData.Values["controller"] == null ? "" : filterContext.RouteData.Values["controller"].ToString();

            if (String.IsNullOrEmpty(this.AuthorizeAction))
                this.AuthorizeAction = filterContext.RouteData.Values["action"] == null ? "" : filterContext.RouteData.Values["action"].ToString();


            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (!authorizeflag && Validata_InUserGroup)
                filterContext.Result = new RedirectResult("~/Account/Login");
            if(authorizeflag==false)
                filterContext.Result = new RedirectResult("~/Account/Login");
            if (filterContext.HttpContext.Response.StatusCode == 403)
                filterContext.Result = new RedirectResult("~/Home/Err403");

            filterContext.Result = new RedirectResult("~/Account/Login");
        }

        
    }
}