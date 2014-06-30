using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ADUserInformation.Business_Layer
{
    public class SQLOps
    {
        SqlConnection cnn = new SqlConnection();
        public SQLOps()
        {
            cnn.ConnectionString = Convert.ToString( ConfigurationManager.AppSettings["EDWConnString"]) ;
        }

        public DataTable getEmpInfo(string strPsno)
        {
            DataSet ds = new DataSet();
 
            string query1 = @"
                select top 1 t1.t_nama[NAME], t1.t_info [EMAIL] , t1.location_id [LOCATION], t2.t_bpsn [ISPSNO], t2.t_dept [DEPTCODE], t3.t_info as [ISEMAIL], t3.t_nama as [ISNAME] from TCCOM001 as t1 JOIN LPCOM009 as t2
ON t1.t_emno = t2.t_emno and t1.location_id=t2.location_id 
JOIN TCCOM001 as t3 ON t2.t_bpsn=t3.t_emno and  t2.location_id=t3.location_id
where t1.t_emno='" + strPsno + "' Order by t1.location_id" ;

            SqlDataAdapter da = new SqlDataAdapter(query1,cnn);
            da.Fill(ds);
            return ds.Tables[0];
        }
    }
}
