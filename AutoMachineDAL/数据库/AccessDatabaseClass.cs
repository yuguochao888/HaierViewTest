using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADOX;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace AutoMachineDAL
{//   
    public  class AccessDatabaseClass
    {//   

        private bool AccessEngineMode = false;//true：64 位,false：32 位;

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="DatabasePath"></param>
        public bool CreateAccessDatabase(string  DatabasePath)
        {//   

            ADOX.Catalog catalog = new Catalog();
            if (AccessEngineMode)
            {//   
                catalog.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+DatabasePath+";Jet OLEDB:Engine Type=5");//64 位
            }
            else
            {//   
                catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath + ";Jet OLEDB:Engine Type=5");//32 位
            }


            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog.ActiveConnection);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog);
            return true;

        }


        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="DatabasePath"></param>
        /// <param name="TableSql"></param>
        public void CreateAccessTable(string DatabasePath,string TableSql)
        {//   

            string strDSN;

            if (AccessEngineMode)
            {//   
                strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabasePath;//64 位
            }
            else
            {//   
                strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath;//32 位
            }

            OleDbConnection myConn = new OleDbConnection(strDSN);
            myConn.Open();

            OleDbCommand cmdStr = new OleDbCommand();
            cmdStr.Connection = myConn;
            cmdStr.CommandText = TableSql;
            cmdStr.ExecuteNonQuery();
            myConn.Close();
        }


        /// <summary>
        /// 插入数据到数据库
        /// </summary>
        /// <param name="DatabasePath"></param>
        /// <param name="strSQL"></param>
        public void InsertDataToAccessDatabase(string DatabasePath, string strSQL)
        {//   

            string strDSN;

            if (AccessEngineMode)
            {//   
                strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabasePath;//64 位
            }
            else
            {//   
                strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath;//32 位
            }

            // 实例化OleDbCommand对象
            OleDbConnection myConn = new OleDbConnection(strDSN);

            // 实例化OleDbCommand对象
            OleDbCommand myCmd = new OleDbCommand(strSQL, myConn);

            // 打开数据库，执行插入SQL语句
            try
            {//   
                myConn.Open();
                myCmd.ExecuteNonQuery();
                //MessageBox.Show("插入操作成功!");
                myConn.Close();
            }
            catch (Exception ex)
            {//   

                string Error = string.Format("{//   0}", ex.Message);
                MessageBox.Show(Error);
            }
            finally
            {//   
                myConn.Close();
            }

        }



        /// <summary>
        /// 查询数据表
        /// </summary>
        public void Query_Data(string DatabasePath, string TableName,  ListView listView_ROI)
        {//   

          
           //查询ROI
            string strDSN;

            if (AccessEngineMode)
            {//   
                strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabasePath;//64 位
            }
            else
            {//   
                strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath;//32 位
            }

           OleDbConnection myConn = new OleDbConnection(strDSN);
           myConn.Open();

           
          string strCom = "Select * from " + TableName;
          OleDbCommand myCommand = new OleDbCommand(strCom, myConn);


          OleDbDataReader reader;
          reader = myCommand.ExecuteReader();


          while (reader.Read())
          {//   
              ListViewItem lt = new ListViewItem(reader.GetValue(0).ToString());
              lt.SubItems.Add(reader.GetValue(1).ToString());
              lt.SubItems.Add(reader.GetValue(2).ToString());
              listView_ROI.Items.Add(lt);
          }


          reader.Close();
  
          myConn.Close();
          

        }


        /// <summary>
        /// 查询数据表
        /// </summary>
        public void Query_RoiData(string DatabasePath, string TableName, ListView listView_ROI)
        {//   

            //查询ROI
            string strDSN;

            if (AccessEngineMode)
            {//   
                strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabasePath;//64 位
            }
            else
            {//   
                strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath;//32 位
            }

            // 实例化OleDbCommand对象
            OleDbConnection myConn = new OleDbConnection(strDSN);
            string strCom = "Select * from " + TableName;
            OleDbCommand myCommand = new OleDbCommand(strCom, myConn);
            myConn.Open();
            OleDbDataReader reader;
            reader = myCommand.ExecuteReader();


            while (reader.Read())
            {//   
                ListViewItem lt = new ListViewItem(reader.GetValue(0).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(1).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(2).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(3).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(4).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(5).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(6).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(7).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(8).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(9).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(10).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(11).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(12).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(13).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(14).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(15).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(16).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(17).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(18).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(19).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(20).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(21).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(22).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(23).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(24).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(25).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(26).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(27).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(28).ToString().Trim());
                lt.SubItems.Add(reader.GetValue(29).ToString().Trim());
                listView_ROI.Items.Add(lt);
            }

            reader.Close();
            myConn.Close();


        }

        public void Update_Roi(string DatabasePath, string strSQL)
        {//   

            //更新ROI坐标插入数据库
            string strDSN;

            if (AccessEngineMode)
            {//   
                strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabasePath;//64 位
            }
            else
            {//   
                strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath;//32 位
            }

            OleDbConnection myConn = new OleDbConnection(strDSN);

            // 实例化OleDbCommand对象
            OleDbCommand myCmd = new OleDbCommand(strSQL, myConn);

            // 打开数据库，执行插入SQL语句
            try
            {//   
                myConn.Open();
                myCmd.ExecuteNonQuery();
                myConn.Close();
            }
            catch (Exception ex)
            {//   

                string Error = string.Format("{//   0}", ex.Message);
                MessageBox.Show(Error);
            }
            finally
            {//   
                myConn.Close();
            }


        }


        /// <summary>
        /// 删除数据表中的数据
        /// </summary>
        public void Delete_Data(int RoiId, string DatabasePath, string TableName)
        {//   

            OleDbConnection myConn;

            string strDSN;

            if (AccessEngineMode)
            {//   
                strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabasePath;//64 位
            }
            else
            {//   
                strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabasePath;//32 位
            }

            myConn = new OleDbConnection(strDSN);

            try
            {//   


                myConn.Open();

                string sqlstr = "delete * from " + TableName + " where id=" + RoiId;

                //定义command对象，并执行相应的SQL语句
                OleDbCommand myCommand = new OleDbCommand(sqlstr, myConn);
                myCommand.ExecuteNonQuery();
                myConn.Close();
                //MessageBox.Show("删除成功!");

            }
            catch (Exception ex)
            {//   
                myConn.Close();
                MessageBox.Show("删除失败!" + ex.Message);
            }

        }

    }
}
