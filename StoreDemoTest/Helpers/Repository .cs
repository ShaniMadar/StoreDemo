using StoreDemoTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace StoreDemoTest.Helpers
{
    public class Repository
    {
        private static Repository mInstance = null;
        private string mConnectionString = string.Empty;
        private static readonly object mLockObj = new object();

        public static Dictionary<string, object> Errors = null;
        public static Repository Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mLockObj)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new Repository();

                        }
                    }
                }
                return mInstance;
            }
        }

        private Repository()
        {

        }

        public int InsertNewPurchase(Purchase purchase, string connectionString)
        {
            int purchaseId = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "InsertNewPurchase";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@date", purchase.Date));
                        command.Parameters.Add(new SqlParameter("@employee", purchase.Employee));
                        command.Parameters.Add(new SqlParameter("@totalSum", purchase.TotalSum));
                        command.Parameters.Add(new SqlParameter("@paidAmount", purchase.PurchasePayment.OfType<PurchasePayment>().FirstOrDefault().Sum));
                        command.Parameters.Add(new SqlParameter("@paymentMethod", purchase.PurchasePayment.OfType<PurchasePayment>().FirstOrDefault().PaymentMethod));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["purchaseId"].ToString()))
                                {
                                    purchaseId = int.Parse(rdr["purchaseId"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return purchaseId;
        }

        public int InsertPurchaseDetails(PurchaseDetails purchaseDetails, string connectionString)
        {
            int purchaseDetailsId = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "InsertPurchaseDetails";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PurchaseId", purchaseDetails.PurchaseId));
                        command.Parameters.Add(new SqlParameter("@item", purchaseDetails.Item));
                        command.Parameters.Add(new SqlParameter("@quantity", purchaseDetails.Quantity));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["id"].ToString()))
                                {
                                    purchaseDetailsId = int.Parse(rdr["id"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return purchaseDetailsId;
        }
        public int InsertPurchasePayment(PurchasePayment purchasePayment, string connectionString)
        {
            int purchaseStatus = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "InsertPurchasePayment";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@purchaseId", purchasePayment.PurchaseId));
                        command.Parameters.Add(new SqlParameter("@paymentMethod", purchasePayment.PaymentMethod));
                        command.Parameters.Add(new SqlParameter("@sum", purchasePayment.Sum));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["status"].ToString()))
                                {
                                    purchaseStatus = int.Parse(rdr["status"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return purchaseStatus;
        }

        public ReturnMethod ValidateReturn(PurchaseDetails purchaseDetails, string connectionString)
        {
            ReturnMethod returnMethod = ReturnMethod.NoReturnType;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "ValidateReturn";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@purchaseId", purchaseDetails.PurchaseId));
                        command.Parameters.Add(new SqlParameter("@returnQuantity", purchaseDetails.Quantity));
                        command.Parameters.Add(new SqlParameter("@purchaseDetailId", purchaseDetails.Id));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["ReturnMethod"].ToString()))
                                {
                                    returnMethod = (ReturnMethod)(int.Parse(rdr["ReturnMethod"].ToString()));
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return returnMethod;
        }

        public int InsertNewReturn(Returns returns, string connectionString)
        {
            int returnId = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "InsertNewReturn";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@purchaseDetailId", returns.PurchaseDetailId));
                        command.Parameters.Add(new SqlParameter("@quantity", returns.Quantity));
                        command.Parameters.Add(new SqlParameter("@Employee", 2));
                        command.Parameters.Add(new SqlParameter("@CreditMethod", returns.CreditType));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["returnId"].ToString()))
                                {
                                    returnId = int.Parse(rdr["returnId"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return returnId;
        }

        public Purchase GetPurchase(int id, string connectionString)
        {
            Purchase purchase = new Purchase();
            purchase.Id = id;
            List<PurchaseDetails> purchaseDetails = new List<PurchaseDetails>();
            List<PurchasePayment> purchasePayments = GetPurchasePayment(id, connectionString);
            List<Returns> returns = new List<Returns>();

            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPurchase";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", id));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (purchase.Employee == 0)
                                {
                                    if (!string.IsNullOrEmpty(rdr["Date"].ToString()))
                                    {
                                        purchase.Date = DateTime.Parse(rdr["Date"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["Employee"].ToString()))
                                    {
                                        purchase.Employee = int.Parse(rdr["Employee"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["Status"].ToString()))
                                    {
                                        purchase.Status = int.Parse(rdr["Status"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["TotalSum"].ToString()))
                                    {
                                        purchase.TotalSum = decimal.Parse(rdr["TotalSum"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["PaidAmount"].ToString()))
                                    {
                                        purchase.PaidAmount = decimal.Parse(rdr["PaidAmount"].ToString());
                                    }
                                }
                                if (!string.IsNullOrEmpty(rdr["PdId"].ToString()))
                                {
                                    PurchaseDetails pd = new PurchaseDetails();
                                    
                                    pd.Id = int.Parse(rdr["PdId"].ToString());

                                    pd.Returns = GetPurchaseDetailsReturns(pd.Id, connectionString);

                                    if (!string.IsNullOrEmpty(rdr["PdItem"].ToString()))
                                    {
                                        pd.Item = int.Parse(rdr["PdItem"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["PdQuantity"].ToString()))
                                    {
                                        pd.Quantity = int.Parse(rdr["PdQuantity"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["PdStatus"].ToString()))
                                    {
                                        pd.Status = int.Parse(rdr["PdStatus"].ToString());
                                    }
                                    purchaseDetails.Add(pd);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            purchase.PurchaseDetails = purchaseDetails;
            purchase.PurchasePayment = purchasePayments;
            return purchase;
        }

        public List<PurchasePayment> GetPurchasePayment(int id, string connectionString)
        {
            List<PurchasePayment> payments = new List<PurchasePayment>();

            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPurchasePayment";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", id));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["Id"].ToString()))
                                {
                                    PurchasePayment r = new PurchasePayment();
                                    r.Id = int.Parse(rdr["Id"].ToString());

                                    if (!string.IsNullOrEmpty(rdr["PaymentMethod"].ToString()))
                                    {
                                        r.PaymentMethod = int.Parse(rdr["PaymentMethod"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["Sum"].ToString()))
                                    {
                                        r.Sum = decimal.Parse(rdr["Sum"].ToString());
                                    }

                                    payments.Add(r);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return payments;
        }

        public ICollection<Returns> GetPurchaseDetailsReturns(int id, string connectionString)
        {
            List<Returns> returns = new List<Returns>();

            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPurchaseDetailReturns";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", id));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!string.IsNullOrEmpty(rdr["Id"].ToString()))
                                {
                                    Returns r = new Returns();
                                    r.Id = int.Parse(rdr["Id"].ToString());

                                    if (!string.IsNullOrEmpty(rdr["TotalAmount"].ToString()))
                                    {
                                        r.TotalAmount = decimal.Parse(rdr["TotalAmount"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["CreditType"].ToString()))
                                    {
                                        r.CreditType = int.Parse(rdr["CreditType"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["Employee"].ToString()))
                                    {
                                        r.Employee = int.Parse(rdr["Employee"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["Date"].ToString()))
                                    {
                                        r.Date = DateTime.Parse(rdr["Date"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(rdr["Quantity"].ToString()))
                                    {
                                        r.Quantity = int.Parse(rdr["Quantity"].ToString());
                                    }
                                    returns.Add(r);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return returns;
        }
        public string GetPurchaseAndReturnReport(DateTime date, string connectionString)
        {
            string table = "<HTML><head></head><body><table border=1><tr>";
            table += "<th>Date</th>";
            table += "<th>Total Purchases</th>";
            table += "<th>Purchases Total Sum</th>";
            table += "<th>Employees who completed purchases</th>";
            table += "<th>Total Returns</th>";
            table += "<th>Returns total sum</th>";
            table += "<th>Employees who completed a return</th>";
            table += "</tr>";
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPurchaseAndReturnReport";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@date", date));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                table += "<tr>";
                                if (!string.IsNullOrEmpty(rdr["date"].ToString()))
                                {
                                    table += "<td>" + rdr["date"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Total Purchases"].ToString()))
                                {
                                    table += "<td>" + rdr["Total Purchases"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Purchases Total Sum"].ToString()))
                                {
                                    table += "<td>" + rdr["Purchases Total Sum"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Employees who completed purchases"].ToString()))
                                {
                                    table += "<td>" + rdr["Employees who completed purchases"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Total Returns"].ToString()))
                                {
                                    table += "<td>" + rdr["Total Returns"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Returns total sum"].ToString()))
                                {
                                    table += "<td>" + rdr["Returns total sum"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Employees who completed a return"].ToString()))
                                {
                                    table += "<td>" + rdr["Employees who completed a return"].ToString() + "</td>";
                                }
                                table += "</tr>";
                            }
                        }
                    }
                }
            }
            
            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            table += "</body></table></html>";
            return table;
        }
        public string GetReportPurchaseAndReturnByEmployee(DateTime date, string connectionString)
        {
            string table = "<HTML><head></head><body><table border=1><tr>";
            table += "<th>Employee</th>";
            table += "<th>Name</th>";
            table += "<th>Total Purchases</th>";
            table += "<th>Purchases Total Sum</th>";
            table += "<th>Total Returns</th>";
            table += "<th>Returns total sum</th>";
            table += "</tr>";
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "GetReportPurchaseAndReturnByEmployee";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@date", date));

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                table += "<tr>";
                                if (!string.IsNullOrEmpty(rdr["Employee"].ToString()))
                                {
                                    table += "<td>" + rdr["Employee"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Name"].ToString()))
                                {
                                    table += "<td>" + rdr["Name"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Total Purchases"].ToString()))
                                {
                                    table += "<td>" + rdr["Total Purchases"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Purchases Total Sum"].ToString()))
                                {
                                    table += "<td>" + rdr["Purchases Total Sum"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Total Returns"].ToString()))
                                {
                                    table += "<td>" + rdr["Total Returns"].ToString() + "</td>";
                                }
                                if (!string.IsNullOrEmpty(rdr["Returns total sum"].ToString()))
                                {
                                    table += "<td>" + rdr["Returns total sum"].ToString() + "</td>";
                                }
                                
                                table += "</tr>";
                            }
                        }
                    }
                }
            }

            catch (SqlException sqlEx)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            table += "</body></table></html>";
            return table;
        }
    }
}
