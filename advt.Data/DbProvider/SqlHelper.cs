using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Data.Common;
using System.Collections;
using System.Threading;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Reflection.Emit;

using advt.Common;
using advt.Config;

namespace advt.Data
{
    /// <summary>
    /// ���ݷ���������
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// ��ѯ����ͳ��
        /// </summary>
        private static int m_querycount = 0;

        /// <summary>
        /// ��ѯ����ͳ��
        /// </summary>
        public static int QueryCount
        {
            get { return m_querycount; }
            set { m_querycount = value; }
        }

        private static string m_querydetail = "";
        public static string QueryDetail
        {
            get { return m_querydetail; }
            set { m_querydetail = value; }
        }

#if DEBUG
        private void SetQueryDetail(string CommandText, DateTime dt1, DateTime dt2, DbParameter[] commandParameters)
        {
            string _querydetail = GetQueryDetail(CommandText, dt1, dt2, commandParameters);

            string _querydetail_tp = Regex.Replace(_querydetail, "<b>TIME:</b>[ .\\d]+</div>", "</div>");
            string m_querydetail_tp = Regex.Replace(m_querydetail, "<b>TIME:</b>[ .\\d]+</div>", "</div>");

            if (m_querydetail_tp.IndexOf(_querydetail_tp) > -1)
            {
                _querydetail = _querydetail.Replace("<center>", "<center style=\"color:red;\">");
            }
            m_querydetail += _querydetail;
        }
#endif

        #region ˽�б���

        private int CommandTimeout = 330;

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        protected string m_connectionstring = null;
        /// <summary>
        /// DbProviderFactoryʵ��
        /// </summary>
        private DbProviderFactory m_factory = null;
        /// <summary>
        /// ���ݽӿ�
        /// </summary>
        private static IDbProvider m_provider = null;
        /// <summary>
        /// Parameters�����ϣ��
        /// </summary>
        private Hashtable m_paramcache = Hashtable.Synchronized(new Hashtable());
        private static object lockHelper = new object();

        #endregion

#if DEBUG
        private string GetQueryDetail(string commandText, DateTime dtStart, DateTime dtEnd, DbParameter[] cmdParams)
        {
            string tr = "<tr style=\"background: rgb(255, 255, 255) none repeat scroll 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">";
            string colums = "";
            string dbtypes = "";
            string values = "";
            string delcareText = "";
            string paramdetails = "";
            if (cmdParams != null && cmdParams.Length > 0)
            {
                foreach (DbParameter param in cmdParams)
                {
                    if (param == null)
                    {
                        continue;
                    }

                    colums += "<td style=\"padding:1px 5px;border-left:1px solid #000;border-top:1px solid #000;\">" + param.ParameterName + "</td>";
                    dbtypes += "<td style=\"padding:1px 5px;border-left:1px solid #000;border-top:1px solid #000;\">" + GetDbParameterTypeAndSize(param) + "</td>";
                    values += "<td style=\"word-wrap:break-word;word-break: break-all;padding:1px 5px;border-left:1px solid #000;border-top:1px solid #000;\">" + param.Value.ToString() + "</td>";

                    delcareText += param.ParameterName + " " + GetDbParameterProperty(param) + ", <br />";

                }

                delcareText = Regex.Replace(delcareText, ", <br />$", "", RegexOptions.IgnoreCase);

                if (!delcareText.Equals(string.Empty))
                {
                    delcareText = "<div>" + "declare " + delcareText + "</div><br />";
                }
                commandText = commandText.Replace("\r\n", "<br />");
                paramdetails = string.Format("<table cellspacing=\"1\" style=\"word-wrap:break-word;word-break: break-all;border-right:1px solid #000;border-bottom:1px solid #000;background: rgb(255, 255, 255) none repeat scroll 0%; margin-top: 5px; font-size: 12px; \">{0}{1}</tr>{0}{2}</tr>{0}{3}</tr></table>", tr, colums, dbtypes, values);
            }
            return string.Format("<center><div style=\"border: 1px solid black; background:#FFF; margin: 2px; padding: 1em; text-align: left; width: 96%; clear: both;\"><div style=\"font-size: 12px; float: right; width: 100px; margin-bottom: 5px;\"><b>TIME:</b> {0}</div><div style=\"font-size: 12px;\">{1}{2}{3}</div></div><br /></center>", dtEnd.Subtract(dtStart).TotalMilliseconds / 1000, delcareText, commandText, paramdetails);
        }

        private string GetDbParameterProperty(DbParameter param)
        {
            if (param == null)
                return null;

            string property = null;
            string value = Utils.HtmlEncode(Utils.ChkSQL(param.Value.ToString()));

            switch ((int)param.DbType)
            {
                case 0: property = string.Format("VarChar({0}) = '{1}'", param.Size, value);
                    break;
                case 0x16: property = string.Format("Char({0}) = '{1}'", param.Size, value);
                    break;
                case 1: property = string.Format("Binary({0}) = {1}", param.Size, value);
                    break;
                case 3: property = string.Format("Bit = {0}", value.ToUpper() == "TRUE" ? 1 : 0);
                    break;
                case 2: property = string.Format("TinyInt = {0}", value);
                    break;
                case 4: property = string.Format("Money = {0}", value);
                    break;
                case 5: property = string.Format("Date = '{0}'", value);
                    break;
                case 6: property = string.Format("DateTime = '{0}'", value);
                    break;
                case 0x1a: property = string.Format("DateTime2({0}) = '{1}'", param.Size, value);
                    break;
                case 0x1b: property = string.Format("DateTimeOffset({0}) = '{1}'", param.Size, value);
                    break;
                case 7: property = string.Format("Decimal({0},{0}) = '{1}'", param.Size, value);
                    break;
                case 8: property = string.Format("Float = {0}", value);
                    break;
                case 9: property = string.Format("UniqueIdentifier = {0}", value);
                    break;
                case 10: property = string.Format("SmallInt = {0}", value);
                    break;
                case 11: property = string.Format("int = {0}", value);
                    break;
                case 12: property = string.Format("BigInt = {0}", value);
                    break;
                case 13: property = string.Format("sql_variant = {0}", value);
                    break;
                //case 14: property = " ";
                //    break;
                case 15: property = string.Format("Real = {0}", value);
                    break;
                case 0x10: property = string.Format("NVarChar({0}) = '{1}'", param.Size, value);
                    break;
                case 0x17: property = string.Format("NChar({0}) = '{1}'", param.Size, value);
                    break;
                case 0x11: property = string.Format("Time({0}) = '{1}'", param.Size, value);
                    break;
                //case 0x12: property = " ";
                //    break;
                //case 0x13: property = " ";
                //    break;
                //case 20: property = " ";
                //    break;
                //case 0x15: property = " ";
                //    break;
                case 0x19: property = string.Format("Xml = '{0}'", value);
                    break;

                default: property = string.Format("NVarChar(max) = '{0}'", value); ;
                    break;
            }

            return property;
        }

        private string GetDbParameterTypeAndSize(DbParameter param)
        {
            if (param == null)
                return null;

            string property = null;

            switch ((int)param.DbType)
            {
                case 0: property = string.Format("VarChar({0})", param.Size);
                    break;
                case 0x16: property = string.Format("Char({0})", param.Size);
                    break;
                case 1: property = string.Format("Binary({0})", param.Size);
                    break;
                case 3: property = "Bit";
                    break;
                case 2: property = "TinyInt";
                    break;
                case 4: property = "Money";
                    break;
                case 5: property = "Date";
                    break;
                case 6: property = "DateTime";
                    break;
                case 0x1a: property = string.Format("DateTime2({0})", param.Size);
                    break;
                case 0x1b: property = string.Format("DateTimeOffset({0})", param.Size);
                    break;
                case 7: property = string.Format("Decimal({0},{0})", param.Size);
                    break;
                case 8: property = "Float";
                    break;
                case 9: property = "UniqueIdentifier";
                    break;
                case 10: property = "SmallInt";
                    break;
                case 11: property = "int";
                    break;
                case 12: property = "BigInt";
                    break;
                case 13: property = "sql_variant";
                    break;
                //case 14: property = "";
                //    break;
                case 15: property = "Real";
                    break;
                case 0x10: property = string.Format("NVarChar({0})", param.Size);
                    break;
                case 0x17: property = string.Format("NChar({0})", param.Size);
                    break;
                case 0x11: property = string.Format("Time({0})", param.Size);
                    break;
                //case 0x12: property = "";
                //    break;
                //case 0x13: property = "";
                //    break;
                //case 20: property = "";
                //    break;
                //case 0x15: property = "";
                //    break;
                case 0x19: property = "Xml";
                    break;

                default: property = "NVarChar(max)";
                    break;
            }

            return property;
        }

        private string GetDbParameterType(DbParameter param)
        {
            if (param == null)
                return null;

            string type = null;
            string value = Utils.HtmlEncode(Utils.ChkSQL(param.Value.ToString()));

            switch ((int)param.DbType)
            {
                case 0: type = "VarChar";
                    break;
                case 0x16: type = "Char";
                    break;
                case 1: type = "Binary";
                    break;
                case 3: type = "Bit";
                    break;
                case 2: type = "TinyInt";
                    break;
                case 4: type = "Money";
                    break;
                case 5: type = "Date";
                    break;
                case 6: type = "DateTime";
                    break;
                case 0x1a: type = "DateTime2";
                    break;
                case 0x1b: type = "DateTimeOffset";
                    break;
                case 7: type = "Decimal";
                    break;
                case 8: type = "Float";
                    break;
                case 9: type = "UniqueIdentifier";
                    break;
                case 10: type = "SmallInt";
                    break;
                case 11: type = "int";
                    break;
                case 12: type = "BigInt";
                    break;
                case 13: type = "sql_variant";
                    break;
                //case 14: type = "";
                //    break;
                case 15: type = "Real";
                    break;
                case 0x10: type = "NVarChar";
                    break;
                case 0x17: type = "NChar";
                    break;
                case 0x11: type = "Time";
                    break;
                //case 0x12: type = "";
                //    break;
                //case 0x13: type = "";
                //    break;
                //case 20: type = "";
                //    break;
                //case 0x15: type = "";
                //    break;
                case 0x19: type = "Xml";
                    break;

                default: type = "NVarChar(max)";
                    break;
            }

            return type;
        }
#endif

        #region ����


        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (m_connectionstring == null)
                {
                    m_connectionstring = BaseConfigs.GetDBConnectString;
                }
                return m_connectionstring;
            }
            set
            {
                m_connectionstring = value;
            }
        }

        /// <summary>
        /// IDbProvider�ӿ�
        /// </summary>
        public static IDbProvider Provider
        {
            get
            {
                if (m_provider == null)
                {
                    lock (lockHelper)
                    {
                        if (m_provider == null)
                        {
                            try
                            {
                                m_provider = (IDbProvider)Activator.CreateInstance(Type.GetType("advt.Data.SqlserverProvider,advt.Data.Sqlserver", false, true));//BaseConfigs.GetDbType
                            }
                            catch
                            {
                                throw new Exception("����.config��Dbtype�ڵ����ݿ������Ƿ���ȷ??");
                            }
                        }
                    }
                }
                return m_provider;
            }
        }

        /// <summary>
        /// DbFactoryʵ��
        /// </summary>
        public DbProviderFactory Factory
        {
            get
            {
                if (m_factory == null)
                {
                    m_factory = Provider.Instance();
                }
                return m_factory;
            }
        }

        /// <summary>
        /// ˢ�����ݿ��ṩ��
        /// </summary>
        public void ResetDbProvider()
        {
            BaseConfigs.ResetRealConfig();
            Data.DatabaseProvider.ResetDbProvider();
            m_connectionstring = null;
            m_factory = null;
            m_provider = null;
        }

        #endregion

        #region ˽�з���

        /// <summary>
        /// ��DbParameter��������(����ֵ)�����DbCommand����.
        /// ������������κ�һ����������DBNull.Value;
        /// �ò�������ֹĬ��ֵ��ʹ��.
        /// </summary>
        /// <param name="command">������</param>
        /// <param name="commandParameters">DbParameters����</param>
        private void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // ���δ����ֵ���������,���������DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// ��DataRow���͵���ֵ���䵽DbParameter��������.
        /// </summary>
        /// <param name="commandParameters">Ҫ����ֵ��DbParameter��������</param>
        /// <param name="dataRow">��Ҫ������洢���̲�����DataRow</param>
        private void AssignParameterValues(DbParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
                return;

            int i = 0;
            // ���ò���ֵ
            foreach (DbParameter commandParameter in commandParameters)
            {
                // ������������,���������,ֻ�׳�һ���쳣.
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format("���ṩ����{0}һ����Ч������{1}.", i, commandParameter.ParameterName));
                // ��dataRow�ı��л�ȡΪ�����������������Ƶ��е�����.
                // ������ںͲ���������ͬ����,����ֵ������ǰ���ƵĲ���.
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        /// ��һ��������������DbParameter��������.
        /// </summary>
        /// <param name="commandParameters">Ҫ����ֵ��DbParameter��������</param>
        /// <param name="parameterValues">��Ҫ������洢���̲����Ķ�������</param>
        private void AssignParameterValues(DbParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
                return;

            // ȷ����������������������ƥ��,�����ƥ��,�׳�һ���쳣.
            if (commandParameters.Length != parameterValues.Length)
                throw new ArgumentException("����ֵ�����������ƥ��.");

            // ��������ֵ
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                        commandParameters[i].Value = DBNull.Value;
                    else
                        commandParameters[i].Value = paramInstance.Value;
                }
                else if (parameterValues[i] == null)
                    commandParameters[i].Value = DBNull.Value;
                else
                    commandParameters[i].Value = parameterValues[i];
            }
        }

        /// <summary>
        /// Ԥ�����û��ṩ������,���ݿ�����/����/��������/����
        /// </summary>
        /// <param name="command">Ҫ������DbCommand</param>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="transaction">һ����Ч�����������nullֵ</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢��������SQL�����ı�</param>
        /// <param name="commandParameters">�������������DbParameter��������,���û�в���Ϊ'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> ��������Ǵ򿪵�,��Ϊtrue,���������Ϊfalse.</param>
        private void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // ���������һ�����ݿ�����.
            command.Connection = connection;
            // ���������ı�(�洢��������SQL���)
            command.CommandText = commandText;

            // ��������
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // ������������.
            command.CommandType = commandType;

            // �����������
            if (commandParameters != null)
                AttachParameters(command, commandParameters);

            command.CommandTimeout = CommandTimeout;

            return;
        }

        /// <summary>
        /// ̽������ʱ�Ĵ洢����,����DbParameter��������.
        /// ��ʼ������ֵΪ DBNull.Value.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ�����</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
        /// <returns>����DbParameter��������</returns>
        private DbParameter[] DiscoverSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if (connection.State == ConnectionState.Open)
                connection.Close();

            connection.ConnectionString = GetRealConnectionString(spName);
            connection.Open();

            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;

            // ����cmdָ���Ĵ洢���̵Ĳ�����Ϣ,����䵽cmd��Parameters��������.
            Provider.DeriveParameters(cmd);
            connection.Close();
            // �������������ֵ����,���������е�ÿһ������ɾ��.
            if (!includeReturnValueParameter)
                cmd.Parameters.RemoveAt(0);

            // ������������
            DbParameter[] discoveredParameters = new DbParameter[cmd.Parameters.Count];
            // ��cmd��Parameters���������Ƶ�discoveredParameters����.
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // ��ʼ������ֵΪ DBNull.Value.
            foreach (DbParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// DbParameter�����������㿽��.
        /// </summary>
        /// <param name="originalParameters">ԭʼ��������</param>
        /// <returns>����һ��ͬ���Ĳ�������</returns>
        private DbParameter[] CloneParameters(DbParameter[] originalParameters)
        {
            DbParameter[] clonedParameters = new DbParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (DbParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion ˽�з�������

        #region ExecuteNonQuery����

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery("SELECT * FROM [table123]");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(CommandType.Text, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery("SELECT * FROM [table123]");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQuery(out int id, string commandText)
        {
            return ExecuteNonQuery(out id, CommandType.Text, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,�����ظղ��������ID
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQuery(out int id, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(out id, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.���û���ṩ����,�����ؽ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");

            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = GetRealConnectionString(commandText);//ConnectionString;
                //connection.Open();

                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }


        /// <summary>
        /// ִ��ָ�������ַ��������ظղ��������ID,���͵�DbCommand.���û���ṩ����,�����ؽ��.
        /// </summary>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQuery(out int id, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");

            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = GetRealConnectionString(commandText);//ConnectionString;
                //connection.Open();

                return ExecuteNonQuery(out id, connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ�������� 
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������������ID 
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(out int id, DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(out id, connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">T�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            // ִ��DbCommand����,�����ؽ��.
            int retval = cmd.ExecuteNonQuery();
#if DEBUG
            DateTime dt2 = DateTime.Now;
            SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
            m_querycount++;
#endif

            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">T�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(out int id, DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (Provider.GetLastIdSql().Trim() == "") throw new ArgumentNullException("GetLastIdSql is \"\"");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ������
            int retval = cmd.ExecuteNonQuery();
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Provider.GetLastIdSql();

#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            id = TypeConverter.ObjectToInt(cmd.ExecuteScalar());
#if DEBUG
            DateTime dt2 = DateTime.Now;

            SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
            m_querycount++;


            if (mustCloseConnection)
            {
                connection.Close();
            }
            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,�����������ֵ�����洢���̲���.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �ӻ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̷������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ�д������DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������/returns>
        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (DbParameter[])null);
        }


        /// <summary>
        /// ִ�д������DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������/returns>
        public int ExecuteNonQuery(out int id, DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(out id, transaction, commandType, commandText, (DbParameter[])null);
        }


        /// <summary>
        /// ִ�д������DbCommand(ָ������).
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��
#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            // ִ��DbCommand����,�����ؽ��.
            int retval = cmd.ExecuteNonQuery();
#if DEBUG
            DateTime dt2 = DateTime.Now;
            SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
            m_querycount++;
#endif

            // ���������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// ִ�д������DbCommand(ָ������).
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecuteNonQuery(out int id, DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��
            int retval = cmd.ExecuteNonQuery();
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Provider.GetLastIdSql();
            id = TypeConverter.ObjectToInt(cmd.ExecuteScalar().ToString());
            return retval;
        }

        /// <summary>
        /// ִ�д������DbCommand(ָ������ֵ).
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>������Ӱ�������</returns>
        public int ExecuteNonQuery(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // û�в���ֵ
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteNonQuery��������

        #region ExecuteCommandWithSplitter����
        /// <summary>
        /// ���к���GO����Ķ���SQL����
        /// </summary>
        /// <param name="commandText">SQL�����ַ���</param>
        /// <param name="splitter">�ָ��ַ���</param>
        public void ExecuteCommandWithSplitter(string commandText, string splitter)
        {
            int startPos = 0;

            do
            {
                int lastPos = commandText.IndexOf(splitter, startPos);
                int len = (lastPos > startPos ? lastPos : commandText.Length) - startPos;
                string query = commandText.Substring(startPos, len);

                if (query.Trim().Length > 0)
                {
                    try
                    {
                        ExecuteNonQuery(CommandType.Text, query);
                    }
                    catch { ;}
                }

                if (lastPos == -1)
                    break;
                else
                    startPos = lastPos + splitter.Length;
            } while (startPos < commandText.Length);

        }

        /// <summary>
        /// ���к���GO����Ķ���SQL����
        /// </summary>
        /// <param name="commandText">SQL�����ַ���</param>
        public void ExecuteCommandWithSplitter(string commandText)
        {
            ExecuteCommandWithSplitter(commandText, "\r\nGO\r\n");
        }
        #endregion ExecuteCommandWithSplitter��������

        #region ExecuteDataset����


        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset("SELECT * FROM [table1]");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(string commandText)
        {
            return ExecuteDataset(CommandType.Text, commandText, (DbParameter[])null);
        }


        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return ExecuteDataset(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��: 
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">SqlParamters��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");

            // �����������ݿ����Ӷ���,��������ͷŶ���.

            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                // connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, (DbParameter[])null);
        }


        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            // connection.Close();

            connection.ConnectionString = GetRealConnectionString(commandText);
            connection.Open();
            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ����DbDataAdapter��DataSet.
            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

#if DEBUG
                DateTime dt1 = DateTime.Now;
#endif
                // ���DataSet.
                da.Fill(ds);
#if DEBUG
                DateTime dt2 = DateTime.Now;

                SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
                m_querycount++;

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return ds;
            }
        }


        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ȼ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����������,ָ������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ���� DataAdapter & DataSet
            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }

        /// <summary>
        /// ִ��ָ�����������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDataset(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �ӻ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDataset���ݼ��������

        #region ExecuteReader �����Ķ���

        /// <summary>
        /// ö��,��ʶ���ݿ���������BaseDbHelper�ṩ�����ɵ������ṩ
        /// </summary>
        private enum DbConnectionOwnership
        {
            /// <summary>��BaseDbHelper�ṩ����</summary>
            Internal,
            /// <summary>�ɵ������ṩ����</summary>
            External
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
        /// </summary>
        /// <remarks>
        /// �����BaseDbHelper������,�����ӹر�DataReaderҲ���ر�.
        /// ����ǵ��ö�������,DataReader�ɵ��ö�����.
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="transaction">һ����Ч������,����Ϊ 'null'</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameters��������,���û�в�����Ϊ'null'</param>
        /// <param name="connectionOwnership">��ʶ���ݿ����Ӷ������ɵ������ṩ������BaseDbHelper�ṩ</param>
        /// <returns>���ذ����������DbDataReader</returns>
        private DbDataReader ExecuteReader(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, DbConnectionOwnership connectionOwnership)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            //connection.Close();
            connection.ConnectionString = GetRealConnectionString(commandText);
            connection.Open();

            bool mustCloseConnection = false;
            // ��������
            DbCommand cmd = Factory.CreateCommand();
            try
            {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                // ���������Ķ���
                DbDataReader dataReader;

#if DEBUG
                DateTime dt1 = DateTime.Now;
#endif
                if (connectionOwnership == DbConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
#if DEBUG
                DateTime dt2 = DateTime.Now;

                SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
                m_querycount++;
                // �������,�Ա��ٴ�ʹ��..
                bool canClear = true;
                foreach (DbParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    //cmd.Dispose();
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }



        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return ExecuteReader(commandType, commandText, (DbParameter[])null);
        }

        public DbDataReader ExecuteReader(CommandType commandType, string commandText, List<DbParameter> lcommandParameters)
        {
            return ExecuteReader(commandType, commandText, lcommandParameters.ToArray());
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">SqlParamter��������(new DbParameter("@prodid", 24))</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            DbConnection connection = null;
            try
            {
                connection = Factory.CreateConnection();
                connection.ConnectionString = ConnectionString;
                //connection.Open();

                return ExecuteReader(connection, null, commandType, commandText, commandParameters, DbConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if (connection != null) connection.Close();
                throw;
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(ConnectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(ConnectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return ExecuteReader(connection, (DbTransaction)null, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">T�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///   DbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  DbDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReader(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // û�в���ֵ
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteReader�����Ķ���

        #region ExecuteScalar ���ؽ�����еĵ�һ�е�һ��

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return ExecuteScalar(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                // connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }


        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(DbConnection connection, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return ExecuteScalar(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            //connection.Close();
            connection.ConnectionString = GetRealConnectionString(commandText);
            connection.Open();

#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��DbCommand����,�����ؽ��.
            object retval = cmd.ExecuteScalar();

            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();

#if DEBUG
            DateTime dt2 = DateTime.Now;

            SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
            m_querycount++;

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }


        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // û�в���ֵ
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            // ִ�в���Ϊ�յķ���
            return ExecuteScalar(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            // ִ��DbCommand����,�����ؽ��.
            object retval = cmd.ExecuteScalar();
#if DEBUG
            DateTime dt2 = DateTime.Now;
            SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
            m_querycount++;
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalar(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // PPull the parameters for this stored procedure from the parameter cache ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // û�в���ֵ
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteScalar

        #region FillDataset ������ݼ�
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ���������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)</param>
        //public void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        //{
        //    if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
        //    if (dataSet == null) throw new ArgumentNullException("dataSet");

        //    // �����������ݿ����Ӷ���,��������ͷŶ���.
        //    using (DbConnection connection = Factory.CreateConnection())
        //    {
        //        connection.ConnectionString = ConnectionString;
        //        connection.Open();

        //        // ����ָ�����ݿ������ַ������ط���.
        //        FillDataset(connection, commandType, commandText, dataSet, tableNames);
        //    }
        //}

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ���������ݼ�.ָ���������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        //public void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        //{
        //    if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
        //    if (dataSet == null) throw new ArgumentNullException("dataSet");
        //    // �����������ݿ����Ӷ���,��������ͷŶ���.
        //    using (DbConnection connection = Factory.CreateConnection())
        //    {
        //        connection.ConnectionString = ConnectionString;
        //        connection.Open();

        //        // ����ָ�����ݿ������ַ������ط���.
        //        FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
        //    }
        //}

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ���������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>    
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        //public void FillDataset(string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        //{
        //    if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
        //    if (dataSet == null) throw new ArgumentNullException("dataSet");
        //    // �����������ݿ����Ӷ���,��������ͷŶ���.
        //    using (DbConnection connection = Factory.CreateConnection())
        //    {
        //        connection.ConnectionString = ConnectionString;
        //        connection.Open();

        //        // ����ָ�����ݿ������ַ������ط���.
        //        FillDataset(connection, spName, dataSet, tableNames, parameterValues);
        //    }
        //}

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ���������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>    
        public void FillDataset(DbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ���������ݼ�,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        public void FillDataset(DbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ���������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        //public void FillDataset(DbConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        //{
        //    if (connection == null) throw new ArgumentNullException("connection");
        //    if (dataSet == null) throw new ArgumentNullException("dataSet");
        //    if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        //    // ����в���ֵ
        //    if ((parameterValues != null) && (parameterValues.Length > 0))
        //    {
        //        // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
        //        DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

        //        // ���洢���̲�����ֵ
        //        AssignParameterValues(commandParameters, parameterValues);

        //        // �������ط���
        //        FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
        //    }
        //    else
        //    {
        //        // û�в���ֵ
        //        FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
        //    }
        //}

        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ���������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        //public void FillDataset(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        //{
        //    FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        //}

        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ���������ݼ�,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        public void FillDataset(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ���������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        //public void FillDataset(DbTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        //{
        //    if (transaction == null) throw new ArgumentNullException("transaction");
        //    if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        //    if (dataSet == null) throw new ArgumentNullException("dataSet");
        //    if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        //    // ����в���ֵ
        //    if ((parameterValues != null) && (parameterValues.Length > 0))
        //    {
        //        // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
        //        DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

        //        // ���洢���̲�����ֵ
        //        AssignParameterValues(commandParameters, parameterValues);

        //        // �������ط���
        //        FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
        //    }
        //    else
        //    {
        //        // û�в���ֵ
        //        FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
        //    }
        //}

        /// <summary>
        /// [˽�з���][�ڲ�����]ִ��ָ�����ݿ����Ӷ���/���������,ӳ�����ݱ���������ݼ�,DataSet/TableNames/DbParameters.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        private void FillDataset(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            // ����DbCommand����,������Ԥ����
            DbCommand command = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ������
            using (DbDataAdapter dataAdapter = Factory.CreateDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                // ׷�ӱ�ӳ��
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }

                // ������ݼ�ʹ��Ĭ�ϱ�����
                dataAdapter.Fill(dataSet);

                // �������,�Ա��ٴ�ʹ��.
                command.Parameters.Clear();
            }

            if (mustCloseConnection)
                connection.Close();
        }
        #endregion


        #region ����ָ���Ĵ洢���̵Ĳ�����

        /// <summary>
        /// ����ָ���Ĵ洢���̵Ĳ�����
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ�</param>
        /// <param name="spName">�洢������</param>
        /// <returns>����DbParameter��������</returns>
        public DbParameter[] GetSpParameterSet(string spName)
        {
            return GetSpParameterSet(spName, false);
        }

        /// <summary>
        /// ����ָ���Ĵ洢���̵Ĳ�����
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="ConnectionString">һ����Ч�����ݿ������ַ�.</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
        /// <returns>����DbParameter��������</returns>
        public DbParameter[] GetSpParameterSet(string spName, bool includeReturnValueParameter)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���).
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ������ַ�</param>
        /// <param name="spName">�洢������</param>
        /// <returns>����DbParameter��������</returns>
        internal DbParameter[] GetSpParameterSet(DbConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">
        /// �Ƿ��������ֵ����
        /// </param>
        /// <returns>����DbParameter��������</returns>
        internal DbParameter[] GetSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            using (DbConnection clonedConnection = (DbConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// [˽��]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
        /// <returns>����DbParameter��������</returns>
        private DbParameter[] GetSpParameterSetInternal(DbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            DbParameter[] cachedParameters;

            cachedParameters = m_paramcache[hashKey] as DbParameter[];
            if (cachedParameters == null)
            {
                DbParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                m_paramcache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion ��������������

        #region ���ɲ���

        public static DbParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static DbParameter MakeOutParam(string ParamName, DbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            DbParameter param;

            param = Provider.MakeParam(ParamName, DbType, Size);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        #endregion ���ɲ�������

        #region ִ��ExecuteScalar,��������ַ������������

        public string ExecuteScalarToStr(CommandType commandType, string commandText)
        {
            object ec = ExecuteScalar(commandType, commandText);
            if (ec == null)
            {
                return "";
            }
            return ec.ToString();
        }


        public string ExecuteScalarToStr(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            object ec = ExecuteScalar(commandType, commandText, commandParameters);
            if (ec == null)
            {
                return "";
            }
            return ec.ToString();
        }
        #endregion


        #region ��������򷽷���Ҫ���ڶ�д���뻷����ʹ��
        /// <summary>
        /// �������ݿ���ִ��ָ�������ַ���,���͵�DbCommand.���û���ṩ����,�����ؽ��.
        /// (ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQueryInMasterDB(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQueryInMasterDB(CommandType commandType, string commandText)
        {
            return ExecuteNonQueryInMasterDB(commandType, commandText, null);
        }

        /// <summary>
        /// �������ݿ���ִ��ָ�������ַ���,���͵�DbCommand.���û���ṩ����,�����ؽ��.
        /// (ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecuteNonQueryInMasterDB(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");

            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                //connection.Open();

                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// �������ݿ���ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
        /// (ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDatasetInMasterDB(CommandType commandType, string commandText)
        {
            return ExecuteDatasetInMasterDB(commandType, commandText, null);
        }

        /// <summary>
        /// �������ݿ���ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
        /// (ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">SqlParamter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet ExecuteDatasetInMasterDB(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");

            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection())
            {
                if (connection == null) throw new ArgumentNullException("connection");

                connection.ConnectionString = ConnectionString;
                connection.Open();

                // Ԥ����
                DbCommand cmd = Factory.CreateCommand();
                bool mustCloseConnection = false;
                PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

                // ����DbDataAdapter��DataSet.
                using (DbDataAdapter da = Factory.CreateDataAdapter())
                {
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();

#if DEBUG
                    DateTime dt1 = DateTime.Now;
#endif
                    // ���DataSet.
                    da.Fill(ds);
#if DEBUG
                    DateTime dt2 = DateTime.Now;

                    SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
                    m_querycount++;

                    cmd.Parameters.Clear();

                    if (mustCloseConnection)
                        connection.Close();

                    return ds;
                }
            }
        }


        /// <summary>
        /// �������ݿ���ִ��ָ�����ݿ����Ӷ���������Ķ���.(ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// �����BaseDbHelper������,�����ӹر�DataReaderҲ���ر�.
        /// ����ǵ��ö�������,DataReader�ɵ��ö�����.
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReaderInMasterDB(CommandType commandType, string commandText)
        {
            return ExecuteReaderInMasterDB(commandType, commandText, null);
        }

        /// <summary>
        /// �������ݿ���ִ��ָ�����ݿ����Ӷ���������Ķ���.(ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// �����BaseDbHelper������,�����ӹر�DataReaderҲ���ر�.
        /// ����ǵ��ö�������,DataReader�ɵ��ö�����.
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameters��������,���û�в�����Ϊ'null'</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader ExecuteReaderInMasterDB(CommandType commandType, string commandText, DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            DbConnection connection = null;
            try
            {
                connection = Factory.CreateConnection();

                if (connection == null) throw new ArgumentNullException("connection");

                connection.ConnectionString = ConnectionString;
                connection.Open();

                bool mustCloseConnection = false;
                // ��������
                DbCommand cmd = Factory.CreateCommand();
                try
                {
                    PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

                    // ���������Ķ���
                    DbDataReader dataReader;

#if DEBUG
                    DateTime dt1 = DateTime.Now;
#endif
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

#if DEBUG
                    DateTime dt2 = DateTime.Now;

                    SetQueryDetail(cmd.CommandText, dt1, dt2, commandParameters);
#endif
                    m_querycount++;
                    // �������,�Ա��ٴ�ʹ��..
                    bool canClear = true;
                    foreach (DbParameter commandParameter in cmd.Parameters)
                    {
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }

                    if (canClear)
                    {
                        //cmd.Dispose();
                        cmd.Parameters.Clear();
                    }

                    return dataReader;
                }
                catch
                {
                    if (mustCloseConnection)
                        connection.Close();
                    throw;
                }
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if (connection != null) connection.Close();
                throw;
            }
        }

        /// <summary>
        /// �������ݿ���ִ��ָ�����ݿ����Ӷ��������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// (ע:�����ݿ����Ӽ�ΪRi.config�ļ���Dbconnectstring�����ӵ�ַ)
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalarInMasterDB(CommandType commandType, string commandText)
        {
            return ExecuteScalarInMasterDB(commandType, commandText, null);
        }

        /// <summary>
        /// �������ݿ���ִ��ָ�����ݿ����Ӷ��������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������SqlParamter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object ExecuteScalarInMasterDB(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection())
            {
                if (connection == null) throw new ArgumentNullException("connection");

                connection.ConnectionString = ConnectionString;
                connection.Open();

                // ����DbCommand����,������Ԥ����
                DbCommand cmd = Factory.CreateCommand();

                bool mustCloseConnection = false;
                PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

                // ִ��DbCommand����,�����ؽ��.
                object retval = cmd.ExecuteScalar();

                // �������,�Ա��ٴ�ʹ��.
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return retval;
            }
        }

        /// <summary>
        /// �Ƿ�ʹ�ÿ������ݿ�
        /// </summary>
        /// <param name="commandText">��ѯ</param>
        /// <returns></returns>
        private bool UserSnapDatabase(string commandText)
        {
            //// ����ϴ�ˢ��cookie���С��5����, ��ˢ�����ݿ����ʱ��
            //if (commandText.StartsWith(BaseConfigs.GetTablePrefix + "create"))
            //{
            //    Utils.WriteCookie("JumpAfterWrite", Environment.TickCount.ToString());
            //    return false;
            //}
            //else if (!String.IsNullOrEmpty(Utils.GetCookie("JumpAfterWrite")) && (Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("JumpAfterWrite"), Environment.TickCount)) < DbSnapConfigs.GetConfig().WriteWaitTime * 1000)
            //    return false;
            //else if (!commandText.StartsWith(BaseConfigs.GetTablePrefix + "get"))
            //    return false;

            return true;
        }


        /// <summary>
        /// ��ȡʹ�õ����ݿ�(�����)���Ӵ�
        /// </summary>
        /// <param name="commandText">�洢��������SQL�����ı�</param>
        /// <returns></returns>
        public string GetRealConnectionString(string commandText)
        {
            //if (DbSnapConfigs.GetConfig() != null && DbSnapConfigs.GetConfig().AppDbSnap)
            //{
            //    commandText = commandText.Trim().ToLower();
            //    if (commandText.StartsWith("select") || ((commandText.StartsWith(BaseConfigs.GetTablePrefix) && UserSnapDatabase(commandText))))
            //    {
            //        DbSnapInfo dbSnapInfo = GetLoadBalanceScheduling.GetConnectDbSnap();

            //        if (DbSnapConfigs.GetConfig().RecordeLog && snapLogList.Capacity > snapLogList.Count)
            //            snapLogList.Add(string.Format("{{'SouceID' : {0}, 'DbconnectString' : '{1}', 'CommandText' : '{2}', 'PostDateTime' : '{3}'}},",
            //                             dbSnapInfo.SouceID,
            //                             dbSnapInfo.DbconnectString,
            //                             commandText.Replace("'", ""),
            //                             Ri.Common.Utils.GetDateTime()));

            //        return dbSnapInfo.DbconnectString;
            //    }
            //}

            return ConnectionString;
        }

        /// <summary>
        /// ������־����
        /// </summary>
        private List<string> snapLogList = new List<string>(400);

        /// <summary>
        /// ��ȡ���ݿ���JSON����
        /// </summary>
        /// <returns></returns>
        public string GetSnapLogJson()
        {
            string snapLogJson = "";
            foreach (string snapLog in snapLogList)
            {
                snapLogJson += snapLog;
            }

            return "[" + snapLogJson.Trim(',') + "]";
        }

        /// <summary>
        /// ������ݿ���JSON����
        /// </summary>
        public void ClearSnapLogJson()
        {
            snapLogList = new List<string>(400);
        }

        ///// <summary>
        ///// ���ؾ�����Ƚӿ�
        ///// </summary>
        //private ILoadBalanceScheduling m_loadBalanceSche;
        ///// <summary>
        ///// ��ʼ�����ؾ�����Ƚӿ�ʵ��
        ///// </summary>
        //private ILoadBalanceScheduling GetLoadBalanceScheduling
        //{
        //    get
        //    {
        //        if (m_loadBalanceSche == null)
        //        {
        //            try
        //            {
        //                m_loadBalanceSche = (ILoadBalanceScheduling)Activator.CreateInstance(Type.GetType(string.Format("Ri.EntLib.{0}, Ri.EntLib", DbSnapConfigs.GetConfig().LoadBalanceScheduling), false, true));
        //            }
        //            catch
        //            {
        //                throw new Exception("����config/dbsnap.config�������Ƿ���ȷ");
        //            }
        //        }
        //        return m_loadBalanceSche;
        //    }
        //}
        #endregion

        #region ����ת����

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<T> setReaderToObject<T>(IDataReader reader) where T : new()
        {
            List<T> ls = new List<T>();
            while (reader.Read())
            {
                T t = new T();
                setOneReaderToObject(reader, t);
                ls.Add(t);
            }
            reader.Close();
            return ls;
        }

        /// <summary>
        /// Readerһ������ת��ΪObjClass����
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        public static void setOneReaderToObject(IDataReader reader, object targetObj)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                System.Reflection.PropertyInfo propertyInfo = targetObj.GetType().GetProperty(reader.GetName(i));
                if (propertyInfo != null)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                            propertyInfo.SetValue(targetObj, Enum.ToObject(propertyInfo.PropertyType, reader.GetValue(i)), null);
                        else
                            propertyInfo.SetValue(targetObj, reader.GetValue(i), null);
                    }
                }
            }
        }

        /// <summary>
        /// ��DataTable����ת��ΪList Object
        /// </summary>
        /// <param name="dt">DataTable</param>
        public static List<T> setDataTableToObject<T>(DataTable dt) where T : new()
        {
            List<T> ls = new List<T>();
            List<string> dics = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
                dics.Add(dt.Columns[i].ColumnName);
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (string columnName in dics)
                {
                    System.Reflection.PropertyInfo propertyInfo = t.GetType().GetProperty(columnName);
                    if (propertyInfo != null)
                    {
                        if (dr[columnName] != DBNull.Value)
                        {
                            if (propertyInfo.PropertyType.IsEnum)
                                propertyInfo.SetValue(t, Enum.ToObject(propertyInfo.PropertyType, dr[columnName]), null);
                            else
                                propertyInfo.SetValue(t, dr[columnName], null);
                        }
                    }
                }
                ls.Add(t);
            }
            return ls;
        }
        #endregion

        #region ����ת�� 3.0

        public static T GetmapIt<T>(IDataReader reader) where T : new()
        {
            int hash = SqlMapper.GetColumnHash(reader);
            var tuple = new SqlMapper.DeserializerState(hash, SqlMapper.GetDeserializer(typeof(T), reader, 0, -1, false));
            var func = tuple.Func;
            return (T)func(reader);
        }

        public static T GetReaderToFirstOrDefault<T>(IDataReader reader) where T : new()
        {
            T rst = new T();
            if (reader.Read())
            {
                rst = GetmapIt<T>(reader);
            }
            else
            {
                rst = default(T);
            }
            reader.Close();
            return rst;
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetReaderToIEnumerable<T>(IDataReader reader) where T : new()
        {
            int hash = SqlMapper.GetColumnHash(reader);
            var tuple = new SqlMapper.DeserializerState(hash, SqlMapper.GetDeserializer(typeof(T), reader, 0, -1, false));
            var func = tuple.Func;
            while (reader.Read())
            {
                yield return (T)func(reader);
            }
            reader.Close();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TReturn>(IDataReader reader, Func<TFirst, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TReturn>(IDataReader reader, Func<TFirst, TSecond, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, SqlMapper.DontMap, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, SqlMapper.DontMap, TReturn>(reader, map, splitOn).ToList();
        }

        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static List<TReturn> GetReaderToList<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(IDataReader reader, Func<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn> map, string splitOn = "Id")
        {
            return GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(reader, map, splitOn).ToList();
        }


        /// <summary>
        /// Reader����ת��ΪObjClass����
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> GetReaderToIEnumerable<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(IDataReader reader, object map, string splitOn)
        {

            SqlMapper.DeserializerState deserializer = default(SqlMapper.DeserializerState);
            Func<IDataReader, object>[] otherDeserializers = null;

            var deserializers = SqlMapper.GenerateDeserializers(new Type[] { typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15) }, splitOn, reader);
            int hash = SqlMapper.GetColumnHash(reader);
            deserializer = new SqlMapper.DeserializerState(hash, deserializers[0]);
            otherDeserializers = deserializers.Skip(1).ToArray();
            Func<IDataReader, TReturn> mapIt = SqlMapper.GenerateMapper<TFirst, TSecond, TThird, TFourth, TFifth, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturn>(deserializer.Func, otherDeserializers, map);

            if (mapIt != null)
            {
                while (reader.Read())
                {
                    yield return mapIt(reader);
                }
            }

            reader.Close();
        }

        public static List<T> GetReaderToList<T>(IDataReader reader) where T : new()
        {
            return GetReaderToIEnumerable<T>(reader).ToList();
        }

        #endregion

        #region Utils
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objparams">new {id=1}</param>
        /// <returns></returns>
        public static string Get_Where_Obj(object objparams, string asName = "")
        {
            if (objparams == null) return string.Empty;

            string where_str = string.Empty;
            foreach (PropertyInfo Obj_Prop_item in objparams.GetType().GetProperties())
            {
                where_str += " " + (asName == "" ? "" : (asName + ".")) + "[" + Obj_Prop_item.Name + "]=@" + Obj_Prop_item.Name + " AND";
            }

            if (where_str == string.Empty)
                return string.Empty;
            else
                return "WHERE" + where_str.Substring(0, where_str.Length - 4);
        }

        public static string Get_Where_Str(string[] item_a)
        {
            string where_str = string.Empty;
            foreach (string item in item_a)
            {
                where_str += " [" + item + "]=@" + item + " AND";
            }

            if (where_str == string.Empty)
                return string.Empty;
            else
                return "WHERE" + where_str.Substring(0, where_str.Length - 4);
        }

        public static List<DbParameter> Get_List_Params(string[][] item_a, object Obj)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            if (Obj != null)
            {
                Dictionary<string, bool> itemflag = new Dictionary<string, bool>();
                int itemCount = 0;
                int Obj_Prop_count = Obj.GetType().GetProperties().Count();
                foreach (string[] item in item_a)
                {
                    if (itemCount == Obj_Prop_count)
                        break;

                    string item_name = item[0];
                    string item_type = item[1];
                    int item_len = TypeConverter.ObjectToInt(item[2], 100);

                    foreach (PropertyInfo Obj_Prop_item in Obj.GetType().GetProperties())
                    {
                        if (item_name == Obj_Prop_item.Name)
                        {
                            l_parms.Add(SqlHelper.MakeInParam("@" + Obj_Prop_item.Name, (DbType)Enum.Parse(typeof(SqlDbType), item_type), item_len, Obj_Prop_item.GetValue(Obj, null)));
                            itemCount++;
                            break;
                        }
                    }
                }
            }
            return l_parms;
        }

        public static void Get_Inserte_Set(string[][] item_a, string[] Include, string[] Exclude, object info, ref string item_name_str, ref string item_value_str, ref List<DbParameter> l_parms)
        {
            Dictionary<string, bool> itemflag = new Dictionary<string, bool>();
            foreach (string[] item in item_a)
            {
                string item_name = item[0];
                string item_type = item[1];
                int item_len = TypeConverter.ObjectToInt(item[2], 100);
                if (Include != null)
                {
                    if (Utils.InArray(item_name, Include, false)) itemflag[item_name] = true;
                }
                else
                {
                    itemflag[item_name] = true;
                }
                if (Exclude != null)
                {
                    if (Utils.InArray(item_name, Exclude, false) && itemflag.Keys.Contains(item_name)) itemflag.Remove(item_name);
                }

                if (itemflag.Keys.Contains(item_name))
                {
                    item_name_str += "[" + item_name + "], ";
                    item_value_str += "@" + item_name + ", ";
                    l_parms.Add(SqlHelper.MakeInParam("@" + item_name, (DbType)Enum.Parse(typeof(SqlDbType), item_type), item_len, Utils.GetValueByProp(info, item_name)));
                }
            }
            item_name_str = item_name_str.Trim().Trim(',');
            item_value_str = item_value_str.Trim().Trim(',');
        }

        public static void Get_Update_Set(string[] key_a, string[][] item_a, string[] Include, string[] Exclude, object info, ref string set_str, ref List<DbParameter> l_parms)
        {
            Dictionary<string, bool> itemflag = new Dictionary<string, bool>();
            foreach (string[] item in item_a)
            {
                string item_name = item[0];
                string item_type = item[1];
                int item_len = TypeConverter.ObjectToInt(item[2], 100);
                if (Include != null)
                {
                    if (Utils.InArray(item_name, Include, false)) itemflag[item_name] = true;
                }
                else
                {
                    itemflag[item_name] = true;
                }
                if (Exclude != null)
                {
                    if (Utils.InArray(item_name, Exclude, false) && itemflag.Keys.Contains(item_name)) itemflag.Remove(item_name);
                }

                if (itemflag.Keys.Contains(item_name))
                {
                    set_str += "[" + item_name + "]=@" + item_name + ", ";
                    l_parms.Add(SqlHelper.MakeInParam("@" + item_name, (DbType)Enum.Parse(typeof(SqlDbType), item_type), item_len, Utils.GetValueByProp(info, item_name)));
                }
                else if (Utils.InArray(item_name, key_a, false) && !itemflag.Keys.Contains(item_name))
                    l_parms.Add(SqlHelper.MakeInParam("@" + item_name, (DbType)Enum.Parse(typeof(SqlDbType), item_type), item_len, Utils.GetValueByProp(info, item_name)));
            }
            set_str = set_str.Trim().Trim(',');
        }
        #endregion

    }

}