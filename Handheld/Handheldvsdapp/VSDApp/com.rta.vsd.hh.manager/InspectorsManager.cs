using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.utilities;

namespace VSDApp.com.rta.vsd.hh.manager
{
    /// <summary>
    /// added By kashif abbasi on dated 27-Dec-2015
    /// </summary>
    class InspectorsManager : IInspectorsManager
    {
        #region Data Member
        private static InspectorsManager _inspectorsManager;
        #endregion

        #region Constructors
        private InspectorsManager()
        {
        }
        #endregion

        #region Methods
        public static InspectorsManager GetInstance()
        {
            if (_inspectorsManager == null)
            {
                _inspectorsManager = new InspectorsManager();
            }
            return _inspectorsManager;
        }

        public DataTable GetInspactorsSummery(string startDate, string endDate, string inspactorID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("inspectorId");
            dt.Columns.Add("inspectorUserName");
            dt.Columns.Add("inspectorName");
            dt.Columns.Add("inspectorNameArabic");
            dt.Columns.Add("totalInspections", Type.GetType("System.Int32"));
            dt.Columns.Add("totalViolations", Type.GetType("System.Int32"));
            
            App.VSDLog.Info(" \n entered in InspectorManager.GetInspactorsummery()");
            try
            {
                handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();
                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();
                header.password = AppProperties.empPassword;
                header.userName = AppProperties.empUserName;
                service.authHeader = header;
                handHeldService.inquireInspectorSummaryResponseItem respItem = new VSDApp.handHeldService.inquireInspectorSummaryResponseItem();
                service.Timeout = 180000;

                respItem = service.inquireInspectorSummary(startDate, endDate, inspactorID);
                App.VSDLog.Info(" \n InspectorsManager.GetInspactorsSummery(): called inquireInspectorSummary(), it return code=  " + respItem.response.code);
                if (respItem.response.code.Equals("1000"))
                {  //1000 (Success)
                    DataRow dr;
                    foreach (var item in respItem.inspectorList)
                    {
                        dr = dt.NewRow();
                        dr["inspectorId"] = item.inspectorId.ToString();
                        dr["inspectorUserName"] = item.inspectorUserName.ToString();
                        dr["inspectorName"] = item.inspectorName.ToString();
                        dr["inspectorNameArabic"] = item.inspectorNameArabic.ToString();
                        dr["totalInspections"] = Convert.ToInt16(item.totalInspections.ToString());
                        dr["totalViolations"] = Convert.ToInt16(item.totalViolations.ToString());
                        dt.Rows.Add(dr);
                    }
                }
                else if (respItem.response.code.Equals("2000"))
                { //2000 (Validation failure)
                    App.VSDLog.Info(" \n InspectorsManager.GetInspactorsSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                else if (respItem.response.code.Equals("3000"))
                {//3000 (No data found)
                    App.VSDLog.Info(" \n InspectorsManager.GetInspactorsSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                else if (respItem.response.code.Equals("4000"))
                {//4000 (Internal failure)
                    App.VSDLog.Info(" \n InspectorsManager.GetInspactorsSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                else
                {
                    App.VSDLog.Info(" \n InspectorsManager.GetInspactorsSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" \n InspectorsManager.GetInspactorsSummery(): exception thrown  exception message is " + ex.Message + "\n" + ex.StackTrace);
                dt = null;
            }
            return dt;
        }

        public DataTable GetGoodByeInspactorSummery(string startDate, string endDate, string inspactorID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("totalInspectionWithFine");
            dt.Columns.Add("totalInspectionWithoutFine");
            dt.Columns.Add("totalDefects");
            dt.Columns.Add("totalFineAmout");
            
            App.VSDLog.Info(" \n entered in InspectorManager.GetGoodByeInspactorSummery()");
             try
            {
                handHeldService.HandHeldService service = new VSDApp.handHeldService.HandHeldService();
                handHeldService.AuthHeader header = new VSDApp.handHeldService.AuthHeader();
                header.password = AppProperties.empPassword;
                header.userName = AppProperties.empUserName;
                service.authHeader = header;
                handHeldService.goodbyeScreenResponseItem respItem = new VSDApp.handHeldService.goodbyeScreenResponseItem();
                service.Timeout = 180000;
                App.VSDLog.Info("Start Date" + startDate);
                App.VSDLog.Info("End Date" + endDate);
                App.VSDLog.Info("Inspector ID" + inspactorID);
                respItem = service.goodbyeScreen(startDate, endDate, inspactorID);
                App.VSDLog.Info(" \n InspectorsManager.GetGoodByeInspactorSummery(): called goodbyeScreen(), it return code=  " + respItem.response.code);
                if (respItem.response.code.Equals("1000"))
                {  //1000 (Success)
                    DataRow dr;
                    dr = dt.NewRow();
                    dr["totalInspectionWithFine"] = (respItem.summary.totalInspectionWithFine != null) ? Convert.ToString(respItem.summary.totalInspectionWithFine) : "0";
                    dr["totalInspectionWithoutFine"] = (respItem.summary.totalInspectionWithoutFine != null) ? Convert.ToString(respItem.summary.totalInspectionWithoutFine) : "0";
                    dr["totalDefects"] = (respItem.summary.totalDefects != null) ? Convert.ToString(respItem.summary.totalDefects) : "0"; ;
                    dr["totalFineAmout"] = (respItem.summary.totalFineAmout != null) ? Convert.ToString(respItem.summary.totalFineAmout) : "0";
                    
                    App.VSDLog.Info("Total Inspection With Fine" + dr["totalInspectionWithFine"]);
                    App.VSDLog.Info("Total Inspection Without Fine" + dr["totalInspectionWithoutFine"]);
                    App.VSDLog.Info("Total Defects" + dr["totalDefects"]);
                    App.VSDLog.Info("Total Fine Ammount" + dr["totalFineAmout"]);
                    dt.Rows.Add(dr);

                }
                else if (respItem.response.code.Equals("2000"))
                { //2000 (Validation failure)
                    App.VSDLog.Info(" \n InspectorsManager.GetGoodByeInspactorSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                else if (respItem.response.code.Equals("3000"))
                {//3000 (No data found)
                    App.VSDLog.Info(" \n InspectorsManager.GetGoodByeInspactorSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                else if (respItem.response.code.Equals("4000"))
                {//4000 (Internal failure)
                    App.VSDLog.Info(" \n InspectorsManager.GetGoodByeInspactorSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                else
                {
                    App.VSDLog.Info(" \n InspectorsManager.GetGoodByeInspactorSummery(): retunrn message is " + respItem.response.message);
                    dt = null;
                }
                
            }
            catch (Exception ex)
            {
                App.VSDLog.Info(" \n InspectorsManager.GetGoodByeInspactorSummery(): exception thrown  exception message is " + ex.Message + "\n" + ex.StackTrace);

                dt = null;
            }
            return dt;
        }
        #endregion



    }
}
