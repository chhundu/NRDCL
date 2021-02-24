using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models.Common
{
    public class CommonProperties
    {
        #region public variables

        #region customer
        public static String customerTitle = "Customer Registration"
        , citizenshipIDLabel = "Citizenship ID "
        , customerIdLabel = "Customer ID "
        , customerNameLabel = "Customer Name "
        , telephoneNoLabel = "Telephone Number"
        , emailIdLabel = "E-mail Id "
        , invalidEmailMsg = "Invalid email address. Please try again."
        , mailAlreadyExistMsg = "Email address already exist. Try another."
        , citizenshipIDExistMsg = "Entered customer ID already exist!. Try another."
        , citizenshipIDNotRegisteredMsg = "Entered customer ID is not registered. Please register!."
        , noCustomerDataMsg = "There are no customer data."
        #endregion

        #region Site
        , siteTitle = "Site Registration"
        , siteNameLabel = "Site Name "
        , siteDistanceFrom = "Distance From "
        , siteAlreadyExist = "Site is already registered!."
        , invalidSiteDistance = "Invalid Distance. Try again!."
        , noSiteDataMsg = "There are no site data."
        , noSiteForCustomerIdMsg = "There is no site registered for entered customer ID.!"
        , invalidSiteMsg = "Invalid site selection.!"
        #endregion

        #region Product
        , productTitle = "Product Entry"
        , productNameLabel = "Product Name "
        , pricePerUnitLabel = "Price Per Unit "
        , transportRateLabel = "Transport Rate "
        , productAlreadyExistMsg = "Product already exist!."
        , invalidRateMsg = "Invalid rate. Try again!."
        , noProductDataMsg = "There are no products."
        , invalidProductMsg = "Invalid product selection.!"
        #endregion

        #region Deposit
        , depositTitle = "Deposit Advance"
        , depositAmountLabel = "Amount "
        , invalidDepositAmountMsg = "Invalid amount. Try again!."
        #endregion

        #region Order
        , orderTitle = "Place Order"
        , customerSiteLabel = "Select Customer Site "
        , selectProductLabel = "Select Product "
        , enterQuantityLabel = "Enter Quantity "
        , invalidQuantityMsg = "Invalid quantity. Try again!."
        #endregion

        #region Report
        , reportTitle = "Generate Report"
        , noDataFoundMsg = "No data found."
        #endregion

        #region common
        , dataNotValidMsg = "Data not valid. Retry again !!!"
        , tryAgainFailureMsg = "Sorry, you tried enough. Have a Nice Day !!!"
        , saveSuccessMsg = "Data saved successfully."
        , updateSuccessMsg = "Data updated successfully."
        , deleteSuccessMsg = "Data deleted successfully.";
        #endregion

        #endregion

    }
}
