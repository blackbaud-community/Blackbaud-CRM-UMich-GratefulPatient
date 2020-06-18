using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbaud.AppFx.Server.AppCatalog;
using Blackbaud.AppFx.XmlTypes;
using System.Resources;

public class UMHSDataConstituentSearchBase : AppSearchList
{
    public string KeyName = string.Empty;
    public string FullName = string.Empty;
    public string FirstName = string.Empty;
    public string LookupID = string.Empty;
    public string AddressBlock = string.Empty;
    public string City = string.Empty;
    public Guid StateID = Guid.Empty;
    public Guid CountryID = Guid.Empty;
    public string PostCode = string.Empty;
    public bool IncludeInactive = false;
    public bool IncludeDeceased = false;
    public bool ExactMatchOnly = false;
    public bool? OnlyPrimaryAddress;
    public int ConstituentType = 0;
    public string SSN = string.Empty;
    public bool CheckAliases = false;
    public bool CheckNickname = false;
    public int ClassOf = 0;
    public string PrimaryBusiness = string.Empty;
    public bool CheckMergedConstituents = false;
    public DateTime MinimumDate = DateTime.MinValue;
    public bool ExcludeCustomGroups = false;
    public bool ExcludeHouseholds = false;
    public string EmailAddress = string.Empty;
    public string PhoneNumber = string.Empty;
    public Guid ProspectManagerID = Guid.Empty;
    public bool? IncludeIndividuals;
    public bool? IncludeOrganizations;
    public bool? IncludeGroups;
    public bool CheckAlternateLookupIDs = false;
    public bool FuzzySearchOnName = false;
    public bool IncludeNonConstituentRecords = false;
    public string MiddleName = string.Empty;
    public Guid SuffixCodeID = Guid.Empty;
    public Guid Constituency = Guid.Empty;
    public string SourceCode = string.Empty;
    public string GenderCode = string.Empty;
    public Guid SCU = Guid.Empty;
    public bool IsMatchingOrg = false;

    //MIMED Data
    public string UMHSDATA = string.Empty;
    public string MRN = string.Empty;
    public string CPISEQUENCE = string.Empty;
    public bool PATIENTSONLY = false;
    public string BIRTHDATE = string.Empty;

    public string MatchingOrgName = string.Empty;

    public bool QuickFind = false;
    public int SiteFilterMode = 0;

    //public Blackbaud.DataForms.DataFormItem[] SitesSelected;

    private System.Data.SqlClient.SqlCommand _cmd = new System.Data.SqlClient.SqlCommand();

    private bool _constituencyIsUserDefined = false;

    protected const string ESCAPE_CHAR = "\\";
    protected enum SearchType
    {
        Base,
        Aliases,
        GroupOfFound,
        Patient
    }

    public override AppSearchListResult GetSearchResults()
    {

        //Setup parameters
        if (!IncludeIndividuals.HasValue)
        {
            IncludeIndividuals = true;
        }

        if (!IncludeOrganizations.HasValue)
        {
            IncludeOrganizations = true;
        }

        if (!IncludeGroups.HasValue)
        {
            IncludeGroups = true;
        }

        //Handle legacy field CONSTITUENTTYPE
        switch (ConstituentType)
        {
            case 0:
                //Everybody
                break;
            case 1:
                //Individuals only
                IncludeIndividuals = true;
                IncludeOrganizations = false;
                IncludeGroups = false;
                break;
            case 2:
                //Orgs Only
                IncludeIndividuals = false;
                IncludeOrganizations = true;
                IncludeGroups = false;
                break;
            case 3:
                //Groups Only
                IncludeIndividuals = false;
                IncludeOrganizations = false;
                IncludeGroups = true;
                break;
        }

        //Handle Quick find field
        if (!String.IsNullOrEmpty(FullName))
        {
            QuickFind = true;
            //if we know this is a not an individual don't parse the name
            if (!IncludeIndividuals.Value)
            {
                KeyName = FullName;
            }
            else
            {
                Blackbaud.AppFx.Server.Parsing.ParseName(FullName, ref FirstName, ref MiddleName, ref KeyName);
            }
            //clear out full name for when the filters are reloaded into the search control
            FullName = string.Empty;
        }

        if (CheckMergedConstituents)
        {
            //Only LOOKUP ID is used. 
            //Set other values to defaults.
            LookupID = getMergedLookupID();
            KeyName = "";
            FirstName = "";
            AddressBlock = "";
            City = "";
            StateID = Guid.Empty;
            PostCode = "";
            EmailAddress = "";
            CheckNickname = false;
            CheckAliases = false;
            OnlyPrimaryAddress = true;
            PhoneNumber = "";
        }

        //Set MaxRows Param
        _cmd.Parameters.AddWithValue("MAXROWS", base.ProcessContext.MaxRows + 1);

        //used to build the string the selects from the ctes
        StringBuilder mainSQL = new StringBuilder();

        //Set Descriptor
        mainSQL.AppendLine("-- Generated by " + this.GetType().FullName);

        //Add date variables
        AddSQLVariables(mainSQL);

        //Open Encryption Key if needed
        if (EncryptKeyNeeded)
        {
            mainSQL.AppendLine("exec dbo.USP_GET_KEY_ACCESS;");
        }

        //Build Create TBL statement
        mainSQL.AppendLine(BuildTableVariable());

        //Get initial constituent matches
        mainSQL.AppendLine(BuildConstitSQL(SearchType.Patient));

        if (!PATIENTSONLY)
        {
            mainSQL.AppendLine(BuildConstitSQL(SearchType.Base));
        }

        IncrementRecordCount(mainSQL);

        //If check aliases do it here.
        if (CheckAliases)
        {
            WrapStatementInCountCheck(mainSQL, BuildConstitSQL(SearchType.Aliases));
        }

        //Get Groups of matching members if necessary
        if (AreGroupsIncluded())
        {
            WrapStatementInCountCheck(mainSQL, BuildConstitSQL(SearchType.GroupOfFound));
        }

        //Allow inheritors insert additional output rows if necessary
        mainSQL.AppendLine(BuildAdditionalSQL());

        //Open Encryption Key if needed
        if (EncryptKeyNeeded)
        {
            mainSQL.AppendLine("close symmetric key sym_BBInfinity;");
        }

        //Build the final select from the TableVariable
        mainSQL.AppendLine(BuildFinalSelect());

        _cmd.CommandTimeout = ProcessContext.TimeOutSeconds;
        _cmd.CommandText = mainSQL.ToString();

        return new AppSearchListResult(_cmd);
    }

    protected virtual void AddSQLVariables(StringBuilder sql)
    {
        sql.AppendLine("declare @CURRENTDATE datetime = getdate();");
        sql.AppendLine("declare @CURRENTDATEEARLIESTTIME date = @CURRENTDATE;");
        sql.AppendLine("declare @FOUND integer = 0;");
    }

    private void IncrementRecordCount(StringBuilder sql)
    {
        sql.AppendLine("set @FOUND = @@ROWCOUNT;");
    }

    private void WrapStatementInCountCheck(StringBuilder sqlSB, string sql)
    {
        sqlSB.AppendLine("if @FOUND < @MAXROWS");
        sqlSB.AppendLine("begin");
        sqlSB.AppendLine(sql);
        IncrementRecordCount(sqlSB);
        sqlSB.AppendLine("end");
    }

    private string BuildTableVariable()
    {
        StringBuilder columns = new StringBuilder();

        SearchListOutputType outputDefinition = default(SearchListOutputType);
        outputDefinition = this.ProcessContext.OutputDefinition;

        foreach (SearchListOutputFieldType outputField in outputDefinition.OutputFields)
        {
            if (columns.Length > 0)
                columns.Append(", ");
            columns.Append(string.Format("{0} {1}", FormatFieldName(outputField.FieldID), GetColumnDBType(outputField)));
        }

        return string.Format("declare {0} table({1});", OutputTable, columns.ToString());
    }

    private string FormatFieldName(string fieldName)
    {
        return string.Format("[{0}]", fieldName);
    }

    private string BuildConstitSQL(SearchType searchType)
    {
        StringBuilder columns = new StringBuilder();
        StringBuilder fromClause = new StringBuilder();
        StringBuilder whereClause = new StringBuilder();

        SearchListOutputType outputDefinition = default(SearchListOutputType);
        outputDefinition = this.ProcessContext.OutputDefinition;

        foreach (SearchListOutputFieldType outputField in outputDefinition.OutputFields)
        {
            if (columns.Length > 0)
                columns.Append(", ");
            columns.Append(GetColumnSQL(outputField, searchType));
        }

        BuildFrom(fromClause, searchType);
        BuildWhere(whereClause, searchType);

        AddConstituentSecurity(whereClause, SecurityRecordType);

        if (whereClause.Length == 0)
        {
            return string.Format("insert into {0} select distinct top (@MAXROWS) {1} from {2};", OutputTable, columns.ToString(), fromClause.ToString());
        }
        else
        {
            return string.Format("insert into {0} select distinct top (@MAXROWS) {1} from {2} where {3};", OutputTable, columns.ToString(), fromClause.ToString(), whereClause.ToString());
        }

    }

    //Value is used by security services to determine if search is for a covered record type.
    protected virtual string SecurityRecordType
    {
        get { return "CONSTITUENT"; }
    }

    protected void AddConstituentSecurity(StringBuilder whereClause, string constituentAlias)
    {
        if (!this.RequestContext.AppUserInfo.IsSysAdmin)
        {
            if (RequestContext.RecordSecurityManager.RecordSecurityInEffect)
            {
                Blackbaud.AppFx.Server.RecordSecurity.SearchListFilterClauseArgs args = new Blackbaud.AppFx.Server.RecordSecurity.SearchListFilterClauseArgs("CONSTITUENT", RequestArgs.SecurityContext, constituentAlias);
                string searchListClause = RequestContext.RecordSecurityManager.GetSearchListFilterClause(args);
                if (!string.IsNullOrEmpty(searchListClause))
                {
                    Where(whereClause, searchListClause);
                }
            }
        }
    }

    //Override this function to insert additional results into the @RETVAL table
    protected virtual string BuildAdditionalSQL()
    {
        return string.Empty;
    }

    private string BuildFinalSelect()
    {
        StringBuilder columns = new StringBuilder();
        StringBuilder retval = new StringBuilder();

        SearchListOutputType outputDefinition = default(SearchListOutputType);
        outputDefinition = this.ProcessContext.OutputDefinition;

        foreach (SearchListOutputFieldType outputField in outputDefinition.OutputFields)
        {
            if (columns.Length > 0)
                columns.Append(", ");
            columns.Append(FormatFieldName(outputField.FieldID));
        }

        retval.AppendFormat("select distinct {0} from {1}", columns.ToString(), OutputTable);

        string sortOrder = GetSortOrder();
        if (sortOrder.Length > 0)
        {
            retval.AppendFormat(" order by {0}", sortOrder);
        }

        retval.AppendLine(";");

        return retval.ToString();
    }

    protected string OutputTable
    {
        get { return "@RETVAL"; }
    }

    protected virtual string GetSortOrder()
    {
        return "SORTCONSTITUENTNAME, LOOKUPID";
    }

    //Add any where clauses that are common to all join types to this subroutine
    protected virtual void BuildFrom(StringBuilder fromClause, SearchType searchType)
    {
        switch (searchType)
        {
            case SearchType.Base:
                buildBaseFrom(fromClause, false);
                break;
            case SearchType.Aliases:
                buildBaseFrom(fromClause, true);
                break;
            case SearchType.GroupOfFound:
                buildGroupFrom(fromClause);
                break;
            case SearchType.Patient:
                buildPatientFrom(fromClause);
                break;
        }
    }

    private void buildPatientFrom(StringBuilder fromClause)
    {
        fromClause.AppendLine("dbo.CONSTITUENT with (nolock)");
        fromClause.AppendLine("JOIN DBO.USR_UMHS_DATA UMHS WITH (NOLOCK) ON UMHS.CONSTITUENTID = CONSTITUENT.ID");
        fromClause.AppendLine("outer apply dbo.UFN_CONSTITUENT_DISPLAYNAME(CONSTITUENT.ID) NF");

        string addressjoinType = Convert.ToString((filterAddress ? "inner" : "left outer"));

        if (OnlyPrimaryAddress.Value)
        {
            fromClause.AppendFormat("{0} join dbo.USR_UMHS_ADDRESS with (nolock) on UMHS.ID = USR_UMHS_ADDRESS.UMHSID and USR_UMHS_ADDRESS.ISPRIMARY = 1", addressjoinType);
        }
        else
        {
            fromClause.AppendFormat("{0} join dbo.USR_UMHS_ADDRESS with (nolock) on UMHS.ID = USR_UMHS_ADDRESS.UMHSID", addressjoinType);
        }
        fromClause.AppendLine();

        if (ClassOf > 0)
        {
            fromClause.AppendLine("inner join dbo.EDUCATIONALHISTORY with (nolock) on EDUCATIONALHISTORY.CONSTITUENTID = CONSTITUENT.ID");
            fromClause.AppendLine("inner join dbo.EDUCATIONALINSTITUTION with (nolock) on EDUCATIONALINSTITUTION.ID = EDUCATIONALHISTORY.EDUCATIONALINSTITUTIONID and EDUCATIONALINSTITUTION.ISAFFILIATED = 1");
        }

        if (!String.IsNullOrEmpty(PrimaryBusiness))
        {
            fromClause.AppendLine("inner join dbo.RELATIONSHIP with (nolock) on RELATIONSHIP.RECIPROCALCONSTITUENTID = CONSTITUENT.ID");
            fromClause.AppendLine("inner join dbo.CONSTITUENT as ORG with (nolock) on ORG.ID = RELATIONSHIP.RELATIONSHIPCONSTITUENTID and RELATIONSHIP.ISPRIMARYBUSINESS = 1 and ORG.ISORGANIZATION = 1");
        }

        if (includeProspectManager)
        {
            fromClause.AppendLine("inner join dbo.PROSPECT with (nolock) on PROSPECT.ID = CONSTITUENT.ID ");
        }

        if (includeConstituency)
        {
            BuildConstituencyJoin(fromClause);
        }

        if (!String.IsNullOrEmpty(SourceCode))
        {
            fromClause.AppendLine("inner join dbo.CONSTITUENTAPPEAL with (nolock) on CONSTITUENTAPPEAL.CONSTITUENTID = CONSTITUENT.ID");
        }

        // LookupID, Include Deceased, and Include Inactive apply whether check merged is selected or not
        if ((!String.IsNullOrEmpty(LookupID)) && CheckAlternateLookupIDs)
        {
            fromClause.AppendLine("inner join (select CONSTITUENTSUB.ID as CONSTITUENTID, CONSTITUENTSUB.LOOKUPID from dbo.CONSTITUENT CONSTITUENTSUB with (nolock) union all select CONSTITUENTID, ALTERNATELOOKUPID from dbo.ALTERNATELOOKUPID with (nolock)) as CONSTITUENTLOOKUPID on CONSTITUENTLOOKUPID.CONSTITUENTID = CONSTITUENT.ID");
        }

        if (includeEmailAddress)
        {
            if (!String.IsNullOrEmpty(EmailAddress))
            {
                if (OnlyPrimaryAddress.Value)
                {
                    fromClause.AppendLine("left join dbo.USR_UMHS_EMAILADDRESS with (nolock) on UMHS.ID = USR_UMHS_EMAILADDRESS.UMHSID and USR_UMHS_EMAILADDRESS.ISPRIMARY = 1");
                }
                else
                {
                    fromClause.AppendLine("left join dbo.USR_UMHS_EMAILADDRESS with (nolock) on UMHS.ID = USR_UMHS_EMAILADDRESS.UMHSID");
                }
            }
            else
            {
                fromClause.AppendLine("left outer join dbo.USR_UMHS_EMAILADDRESS with (nolock) on UMHS.ID = USR_UMHS_EMAILADDRESS.UMHSID and USR_UMHS_EMAILADDRESS.ISPRIMARY = 1");
            }
        }

        if (includePhone)
        {
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                if (OnlyPrimaryAddress.Value)
                {
                    fromClause.AppendLine("left join dbo.USR_UMHS_PHONE with (nolock) on UMHS.ID = USR_UMHS_PHONE.UMHSID and USR_UMHS_PHONE.ISPRIMARY = 1");
                }
                else
                {
                    fromClause.AppendLine("left join dbo.USR_UMHS_PHONE with (nolock) on UMHS.ID = USR_UMHS_PHONE.UMHSID");
                }
            }
            else
            {
                fromClause.AppendLine("left outer join dbo.USR_UMHS_PHONE with (nolock) on UMHS.ID = USR_UMHS_PHONE.UMHSID and USR_UMHS_PHONE.ISPRIMARY = 1");
            }
        }
        fromClause.AppendLine("left outer join dbo.GROUPDATA GD with (nolock) on CONSTITUENT.ID = GD.ID");
        fromClause.AppendLine("left outer join dbo.UFN_EDUCATIONALINVOLVEMENT_GETLIST() as I on I.CONSTITUENTID = CONSTITUENT.ID left outer join USR_EDUCATIONALINVOLVEMENT UEI on I.ID = UEI.ID");
        fromClause.AppendLine("left outer join EDUCATIONALHISTORY ehprimary on CONSTITUENT.ID = ehprimary.CONSTITUENTID left outer join EDUCATIONADDITIONALINFORMATION eaiprimary on ehprimary.ID = eaiprimary.EDUCATIONALHISTORYID and ehprimary.ISPRIMARYRECORD = 1 ");
    }
    private void buildBaseFrom(StringBuilder fromClause, bool isAlias)
    {
        if (isAlias)
        {
                fromClause.AppendLine("dbo.ALIAS with (nolock)");
                fromClause.AppendLine("INNER JOIN DBO.CONSTITUENT WITH (NOLOCK) ON CONSTITUENT.ID = ALIAS.CONSTITUENTID");
                fromClause.AppendLine("LEFT JOIN DBO.USR_UMHS_DATA UMHS WITH (NOLOCK) ON UMHS.CONSTITUENTID = CONSTITUENT.ID");
        }
        else
        {
                fromClause.AppendLine("dbo.CONSTITUENT with (nolock)");
                fromClause.AppendLine("LEFT JOIN DBO.USR_UMHS_DATA UMHS WITH (NOLOCK) ON UMHS.CONSTITUENTID = CONSTITUENT.ID");
        }
        fromClause.AppendLine("outer apply dbo.UFN_CONSTITUENT_DISPLAYNAME(CONSTITUENT.ID) NF");

        string addressjoinType = Convert.ToString((filterAddress ? "inner" : "left outer"));

        if (OnlyPrimaryAddress.Value)
        {
            fromClause.AppendFormat("{0} join dbo.ADDRESS with (nolock) on CONSTITUENT.ID = ADDRESS.CONSTITUENTID and ADDRESS.ISPRIMARY = 1", addressjoinType);
        }
        else
        {
            fromClause.AppendFormat("{0} join dbo.ADDRESS with (nolock) on CONSTITUENT.ID = ADDRESS.CONSTITUENTID", addressjoinType);
        }
        fromClause.AppendLine();

        if (ClassOf > 0)
        {
            fromClause.AppendLine("inner join dbo.EDUCATIONALHISTORY with (nolock) on EDUCATIONALHISTORY.CONSTITUENTID = CONSTITUENT.ID");
            fromClause.AppendLine("inner join dbo.EDUCATIONALINSTITUTION with (nolock) on EDUCATIONALINSTITUTION.ID = EDUCATIONALHISTORY.EDUCATIONALINSTITUTIONID and EDUCATIONALINSTITUTION.ISAFFILIATED = 1");
        }

        if (!String.IsNullOrEmpty(PrimaryBusiness))
        {
            fromClause.AppendLine("inner join dbo.RELATIONSHIP with (nolock) on RELATIONSHIP.RECIPROCALCONSTITUENTID = CONSTITUENT.ID");
            fromClause.AppendLine("inner join dbo.CONSTITUENT as ORG with (nolock) on ORG.ID = RELATIONSHIP.RELATIONSHIPCONSTITUENTID and RELATIONSHIP.ISPRIMARYBUSINESS = 1 and ORG.ISORGANIZATION = 1");
        }

        if (includeProspectManager)
        {
            fromClause.AppendLine("inner join dbo.PROSPECT with (nolock) on PROSPECT.ID = CONSTITUENT.ID ");
        }

        if (includeConstituency)
        {
            BuildConstituencyJoin(fromClause);
        }

        if (!String.IsNullOrEmpty(SourceCode))
        {
            fromClause.AppendLine("inner join dbo.CONSTITUENTAPPEAL with (nolock) on CONSTITUENTAPPEAL.CONSTITUENTID = CONSTITUENT.ID");
        }

        // LookupID, Include Deceased, and Include Inactive apply whether check merged is selected or not
        if ((!String.IsNullOrEmpty(LookupID)) && CheckAlternateLookupIDs)
        {
            fromClause.AppendLine("inner join (select CONSTITUENTSUB.ID as CONSTITUENTID, CONSTITUENTSUB.LOOKUPID from dbo.CONSTITUENT CONSTITUENTSUB with (nolock) union all select CONSTITUENTID, ALTERNATELOOKUPID from dbo.ALTERNATELOOKUPID with (nolock)) as CONSTITUENTLOOKUPID on CONSTITUENTLOOKUPID.CONSTITUENTID = CONSTITUENT.ID");
        }

        if (includeEmailAddress)
        {
            if (!String.IsNullOrEmpty(EmailAddress))
            {
                if (OnlyPrimaryAddress.Value)
                {
                    fromClause.AppendLine("left join dbo.EMAILADDRESS with (nolock) on CONSTITUENT.ID = EMAILADDRESS.CONSTITUENTID and EMAILADDRESS.ISPRIMARY = 1");
                }
                else
                {
                    fromClause.AppendLine("left join dbo.EMAILADDRESS with (nolock) on CONSTITUENT.ID = EMAILADDRESS.CONSTITUENTID");
                }
            }
            else
            {
                fromClause.AppendLine("left outer join dbo.EMAILADDRESS with (nolock) on CONSTITUENT.ID = EMAILADDRESS.CONSTITUENTID and EMAILADDRESS.ISPRIMARY = 1");
            }
        }

        if (includePhone)
        {
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                if (OnlyPrimaryAddress.Value)
                {
                    fromClause.AppendLine("left join dbo.PHONE with (nolock) on CONSTITUENT.ID = PHONE.CONSTITUENTID and PHONE.ISPRIMARY = 1");
                }
                else
                {
                    fromClause.AppendLine("left join dbo.PHONE with (nolock) on CONSTITUENT.ID = PHONE.CONSTITUENTID");
                }
            }
            else
            {
                fromClause.AppendLine("left outer join dbo.PHONE with (nolock) on CONSTITUENT.ID = PHONE.CONSTITUENTID and PHONE.ISPRIMARY = 1");
            }
        }

        fromClause.AppendLine("left outer join dbo.GROUPDATA GD with (nolock) on CONSTITUENT.ID = GD.ID");
        fromClause.AppendLine("left outer join dbo.UFN_EDUCATIONALINVOLVEMENT_GETLIST() as I on I.CONSTITUENTID = CONSTITUENT.ID left outer join USR_EDUCATIONALINVOLVEMENT UEI on I.ID = UEI.ID");
        fromClause.AppendLine("left outer join EDUCATIONALHISTORY ehprimary on CONSTITUENT.ID = ehprimary.CONSTITUENTID left outer join EDUCATIONADDITIONALINFORMATION eaiprimary on ehprimary.ID = eaiprimary.EDUCATIONALHISTORYID and ehprimary.ISPRIMARYRECORD = 1 ");
    }

    private void BuildConstituencyJoin(StringBuilder fromClause)
    {
        switch (Constituency.ToString().ToUpper())
        {
            case "F828E957-5F5E-479A-8F23-2FFD6C7C68FF":
                fromClause.AppendLine("inner join dbo.BOARDMEMBERDATERANGE with (nolock) on BOARDMEMBERDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "6093915E-ADE9-42BE-88AE-304731754467":
                fromClause.AppendLine("inner join dbo.STAFFDATERANGE with (nolock) on STAFFDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "D2DCA06A-BE6E-40B3-B95D-59A926181923":
                fromClause.AppendLine("inner join dbo.FUNDRAISERDATERANGE with (nolock) on FUNDRAISERDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "00E748FB-940D-4A7D-A133-C148B29410A8":
                fromClause.AppendLine("inner join dbo.PROSPECTDATERANGE with (nolock) on PROSPECTDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "E7489703-3D63-4017-A2BC-88C092563C5D":
                fromClause.AppendLine("inner join dbo.VOLUNTEERDATERANGE with (nolock) on VOLUNTEERDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "AC9DB5A4-14E0-416A-9FB2-04038AC66799":
                fromClause.AppendLine("inner join dbo.COMMITTEEDATERANGE with (nolock) on COMMITTEEDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "4D746A03-A0AB-45F3-A30B-1AD4F304E622":
            case "F89E03BC-E724-4e5d-943B-72D4D1E1E916":
            case "908E521C-B0A5-4832-B664-7D7B079D77C2":
                fromClause.AppendLine("inner join dbo.SPONSORDATERANGE with (nolock) on SPONSORDATERANGE.CONSTITUENTID = CONSTITUENT.ID");
                switch (Constituency.ToString())
                {
                    case "4D746A03-A0AB-45F3-A30B-1AD4F304E622":
                        fromClause.AppendLine("SPONSORDATERANGE.SPONSORTYPECODE = 0");
                        break;
                    case "F89E03BC-E724-4e5d-943B-72D4D1E1E916":
                        fromClause.AppendLine("SPONSORDATERANGE.SPONSORTYPECODE = 1");
                        break;
                    case "908E521C-B0A5-4832-B664-7D7B079D77C2":
                        fromClause.AppendLine("SPONSORDATERANGE.SPONSORTYPECODE = 2");
                        break;
                }
                break;
            case "171AB3CD-C4E1-4825-B693-10F524A7A594":
                fromClause.AppendLine("inner join dbo.BANK with (nolock) on BANK.ID = CONSTITUENT.ID");
                break;
            case "70165682-4324-46EC-9439-83FC0CC67E7F":
                fromClause.AppendLine("inner join dbo.REVENUE with (nolock) on REVENUE.CONSTITUENTID = CONSTITUENT.ID");

                break;
            case "55FE8E7C-2B68-44C8-B35C-818AD1944C03":
                fromClause.AppendLine("inner join dbo.NETCOMMUNITYCLIENTUSER with (nolock) on NETCOMMUNITYCLIENTUSER.CONSTITUENTID = CONSTITUENT.ID");

                break;
            case "5435C96D-8617-46C3-9A62-5AFF08451A53":
                fromClause.AppendLine("inner join dbo.EVENTEXPENSE with (nolock) on EVENTEXPENSE.VENDORID = CONSTITUENT.ID");

                break;
            case "CEE46FE7-3FBB-4DFE-97EB-BA67DD33C634":
                fromClause.AppendLine("inner join dbo.PLANNEDGIFT with (nolock) on PLANNEDGIFT.CONSTITUENTID = CONSTITUENT.ID");

                break;
            case "A843B859-4C6B-445B-97F3-179582E270A5":
                fromClause.AppendLine("inner join dbo.SALESORDER with (nolock) on SALESORDER.CONSTITUENTID = CONSTITUENT.ID and SALESORDER.STATUSCODE <> 0");

                break;
            case "3DFAC92E-78BD-4051-ABDC-02C675DEB8F6":
                fromClause.AppendLine("inner join dbo.CONSTITUENTRECOGNITION with (nolock) on CONSTITUENTRECOGNITION.CONSTITUENTID = CONSTITUENT.ID");
                fromClause.AppendLine("inner join dbo.RECOGNITIONPROGRAM with (nolock) on CONSTITUENTRECOGNITION.RECOGNITIONPROGRAMID = RECOGNITIONPROGRAM.ID");

                break;
            case "2D11326E-8F3B-4322-9797-57C1AACFA5DF":
                fromClause.AppendLine("inner join dbo.MEMBER with (nolock) on MEMBER.CONSTITUENTID = CONSTITUENT.ID");
                fromClause.AppendLine("inner join dbo.MEMBERSHIP with (nolock) on MEMBER.MEMBERSHIPID=MEMBERSHIP.ID");
                fromClause.AppendLine("inner join MEMBERSHIPLEVELTERM with (nolock) on MEMBERSHIP.MEMBERSHIPLEVELTERMID = MEMBERSHIPLEVELTERM.ID");

                break;
            case "2D04A9C5-27D0-4646-BF0F-6826E4C12632":
                fromClause.AppendLine("inner join dbo.MATCHFINDERCONSTITUENT with (nolock) on MATCHFINDERCONSTITUENT.ID = CONSTITUENT.ID");

                break;
            case "4DB8F4FC-BC43-421D-B592-69BEF109B5FC":
            case "46EC3424-BA54-4431-A7DC-C6CEBB3B4592":
                fromClause.AppendLine("inner join dbo.EDUCATIONALHISTORY EH with (nolock) on EH.CONSTITUENTID = CONSTITUENT.ID");
                fromClause.AppendLine("inner join dbo.EDUCATIONALINSTITUTION EI with (nolock) on EH.EDUCATIONALINSTITUTIONID = EI.ID and EI.ISAFFILIATED = 1");
                fromClause.AppendLine("inner join EDUCATIONALHISTORYSTATUS EHS on EH.EDUCATIONALHISTORYSTATUSID = EHS.ID");

                break;
            case "D9982C99-15C1-4C90-873E-56FD4B164056":
                fromClause.AppendLine("inner join dbo.GRANTOR with (nolock) on GRANTOR.ID = CONSTITUENT.ID");

                break;
            case "C49D4B46-72A7-4206-91AA-BEABA2323E3C":
                fromClause.AppendLine("inner join dbo.REGISTRANT with (nolock) on REGISTRANT.CONSTITUENTID = CONSTITUENT.ID");
                break;
            case "093A3D4F-2974-447F-AD92-870EB4A04593":
                fromClause.AppendLine("inner join dbo.GROUPMEMBER with (nolock) on GROUPMEMBER.MEMBERID = CONSTITUENT.ID");
                fromClause.AppendLine("inner join dbo.GROUPMEMBERDATERANGE with (nolock) on GROUPMEMBER.ID = GROUPMEMBERDATERANGE.GROUPMEMBERID");

                break;
            case "1A9BFE80-604D-4B5B-8065-E751DDF6EF39":
                fromClause.AppendLine("inner join dbo.UFN_SELECTION_CONSTITUENT_MAJORDONORS() as MAJORDONORS on MAJORDONORS.ID = CONSTITUENT.ID");

                break;
            case "E5A0EA42-65BA-4B25-AFE2-9B709F99E72B":
                fromClause.AppendLine("inner join dbo.UFN_SELECTION_CONSTITUENT_LOYALDONORS() as LOYALDONORS on LOYALDONORS.ID = CONSTITUENT.ID");

                break;
            case "97688220-0AFA-4354-A327-590D715961D7":
                fromClause.AppendLine("inner join dbo.SCHOOL on SCHOOL.ID = CONSTITUENT.ID");

                break;
            case "08D55D6A-10C8-4A72-92A0-EF87033AD7B6":
                fromClause.AppendLine("inner join dbo.FACULTY on FACULTY.ID = CONSTITUENT.ID");

                break;
            default:
                _constituencyIsUserDefined = true;
                fromClause.AppendLine("inner join dbo.CONSTITUENCY with (nolock) on CONSTITUENCY.CONSTITUENTID = CONSTITUENT.ID");
                break;
        }
    }

    private void buildGroupFrom(StringBuilder fromClause)
    {
        fromClause.AppendLine("dbo.GROUPMEMBER GM with (nolock)");
        fromClause.AppendLine("inner join dbo.CONSTITUENT with (nolock) on GM.GROUPID = CONSTITUENT.ID");
        fromClause.AppendLine("left join USR_UMHS_DATA UMHS with (nolock) on UMHS.CONSTITUENTID = CONSTITUENT.ID");
        fromClause.AppendLine("outer apply dbo.UFN_CONSTITUENT_DISPLAYNAME(CONSTITUENT.ID) NF");
        fromClause.AppendLine("inner join dbo.GROUPDATA GD with (nolock) on GM.GROUPID = GD.ID");
        fromClause.AppendLine("inner join " + OutputTable + " R on R.ID = GM.MEMBERID");
        fromClause.AppendLine("left outer join dbo.GROUPMEMBERDATERANGE GMDR with (nolock) on GM.ID = GMDR.GROUPMEMBERID");
        fromClause.AppendLine("left outer join dbo.ADDRESS with (nolock) on CONSTITUENT.ID = ADDRESS.CONSTITUENTID and ADDRESS.ISPRIMARY = 1");
        fromClause.AppendLine("left outer join dbo.USR_UMHS_ADDRESS with (nolock) on UMHS.ID = USR_UMHS_ADDRESS.UMHSID and USR_UMHS_ADDRESS.ISPRIMARY = 1");
        fromClause.AppendLine("left outer join dbo.UFN_EDUCATIONALINVOLVEMENT_GETLIST() as I on I.CONSTITUENTID = CONSTITUENT.ID left outer join USR_EDUCATIONALINVOLVEMENT UEI on I.ID = UEI.ID");
        fromClause.AppendLine("left outer join EDUCATIONALHISTORY ehprimary on CONSTITUENT.ID = ehprimary.CONSTITUENTID left outer join EDUCATIONADDITIONALINFORMATION eaiprimary on ehprimary.ID = eaiprimary.EDUCATIONALHISTORYID and ehprimary.ISPRIMARYRECORD = 1  ");

        if (includeEmailAddress)
        {
            fromClause.AppendLine("left outer join dbo.EMAILADDRESS with (nolock) on CONSTITUENT.ID = EMAILADDRESS.CONSTITUENTID and EMAILADDRESS.ISPRIMARY = 1");
            fromClause.AppendLine("left outer join dbo.USR_UMHS_EMAILADDRESS with (nolock) on UMHS.ID = USR_UMHS_EMAILADDRESS.UMHSID and USR_UMHS_EMAILADDRESS.ISPRIMARY = 1");
        }

        if (includePhone)
        {
            fromClause.AppendLine("left outer join dbo.PHONE with (nolock) on CONSTITUENT.ID = PHONE.CONSTITUENTID and PHONE.ISPRIMARY = 1");
            fromClause.AppendLine("left outer join dbo.USR_UMHS_PHONE with (nolock) on UMHS.ID = USR_UMHS_PHONE.UMHSID and USR_UMHS_PHONE.ISPRIMARY = 1");
        }
    }

    protected virtual void BuildWhere(StringBuilder whereClause, SearchType searchType)
    {
        switch (searchType)
        {
            case UMHSDataConstituentSearchBase.SearchType.Base:
                buildBaseWhere(whereClause, false);
                break;
            case UMHSDataConstituentSearchBase.SearchType.Aliases:
                buildBaseWhere(whereClause, true);
                break;
            case UMHSDataConstituentSearchBase.SearchType.GroupOfFound:
                buildGroupWhere(whereClause);
                break;
            case SearchType.Patient:
                buildPatientWhere(whereClause);
                break;
        }
    }

    private string UMHSNameJoin()
    {
        //string CONSTITorALIAS = Convert.ToString((isAlias ? "ALIAS" : "CONSTITUENT"));
        StringBuilder nameJoin = new StringBuilder();
        if (!String.IsNullOrEmpty(KeyName))
        {
            nameJoin.AppendLine(string.Format("and {0} like '{1}' escape '{2}'", "UMHS.KEYNAME", GetLikeParameterValue(KeyName), ESCAPE_CHAR));
            //if (FuzzySearchOnName && !ExactMatchOnly)
            //{
            //    StringBuilder sbKeyName = new StringBuilder();
            //    WhereLike(sbKeyName, CONSTITorALIAS + ".KEYNAME", "@KEYNAME", KeyName);
            //    sbKeyName.Append(" or soundex(" + CONSTITorALIAS + ".KEYNAME) = soundex(@KEYNAME)");
            //    Where(nameJoin, "(" + sbKeyName.ToString() + ")", "@KEYNAME", KeyName);
            //}
            //else
            //{
            //    WhereLike(nameJoin, CONSTITorALIAS + ".KEYNAME", "@KEYNAME", KeyName);
            //    //WhereLikeOr(nameJoin, "UMHS.KEYNAME", "@KEYNAME", KeyName);
            //    //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            //}
            //if (QuickFind)
            //{
            //    if (System.Text.RegularExpressions.Regex.Match(KeyName, "\\d").Success)
            //    {
            //        nameJoin.Append(" OR (LOOKUPID like '%" + KeyName + "%')");
            //        //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            //        //ElseIf System.Text.RegularExpressions.Regex.Match(KeyName, "\d").Success And System.Text.RegularExpressions.Regex.Match(KeyName, "\w").Success Then
            //        //nameJoin.Append(" OR ((LOOKUPID like '%" + KeyName + "%') AND ((CONSTITUENT.FIRSTNAME like '%" + KeyName + "%') OR (CONSTITUENT.KEYNAME like '%" + KeyName + "%')))")
            //    }
            //}
        }

        //if (MiddleName.Length > 0)
        //{
        //    WhereLike(nameJoin, CONSTITorALIAS + ".MIDDLENAME", "@MIDDLENAME", MiddleName);
        //    //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        //}

        //if (!SuffixCodeID.Equals(Guid.Empty))
        //{
        //    if (!isAlias)
        //    {
        //        Where(nameJoin, "CONSTITUENT.SUFFIXCODEID = @SUFFIXCODEID", "@SUFFIXCODEID", SuffixCodeID);
        //        //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        //    }
        //}

        //if (FirstName.Length > 0)
        //{
        //    // If only groups and orgs are being searched, disregard the first name.
        //    if (IncludeIndividuals.Value)
        //    {
        //        if (CheckNickname)
        //        {
        //            if (!_cmd.Parameters.Contains("@FIRSTNAME"))
        //            {
        //                _cmd.Parameters.AddWithValue("@FIRSTNAME", GetLikeParameterValue(FirstName));
        //            }

        //            if (FuzzySearchOnName && !ExactMatchOnly)
        //            {
        //                Where(nameJoin, string.Format("(" + CONSTITorALIAS + ".FIRSTNAME like @FIRSTNAME escape '{0}' or CONSTITUENT.NICKNAME like @FIRSTNAME escape '{0}' or soundex(" + CONSTITorALIAS + ".FIRSTNAME) = soundex(@FIRSTNAME) or soundex(CONSTITUENT.NICKNAME) = soundex(@FIRSTNAME))", ESCAPE_CHAR));
        //                //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        //            }
        //            else
        //            {
        //                Where(nameJoin, string.Format("(" + CONSTITorALIAS + ".FIRSTNAME like @FIRSTNAME escape '{0}' or CONSTITUENT.NICKNAME like @FIRSTNAME escape '{0}')", ESCAPE_CHAR));
        //                //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        //            }
        //        }
        //        else
        //        {
        //            if (FuzzySearchOnName && !ExactMatchOnly)
        //            {
        //                StringBuilder sbFirstName = new StringBuilder();
        //                WhereLike(sbFirstName, CONSTITorALIAS + ".FIRSTNAME", "@FIRSTNAME", FirstName);
        //                sbFirstName.Append(" or soundex(" + CONSTITorALIAS + ".FIRSTNAME) = soundex(@FIRSTNAME)");
        //                Where(nameJoin, "(" + sbFirstName.ToString() + ")", "@FIRSTNAME", FirstName);
        //                //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        //            }
        //            else
        //            {
        //                WhereLike(nameJoin, CONSTITorALIAS + ".FIRSTNAME", "@FIRSTNAME", FirstName);
        //                WhereLikeOr(nameJoin, "UMHS.FIRSTNAME", "@FIRSTNAME", FirstName);
        //                //nameJoin.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        //            }
        //        }

        //    }

        //    //If this is a quick find ensure orgs with spaces in the name can be found.
        //    if (KeyName.Length > 0 && !KeyName.Contains("UMHS".ToLower()))
        //    {
        //        StringBuilder sbOrigName = new StringBuilder();

        //        WhereLike(sbOrigName, CONSTITorALIAS + ".KEYNAME", "@FULLNAME", string.Format("{0} {1}", FirstName, KeyName));

        //        if (nameJoin.Length > 0)
        //        {
        //            nameJoin.Insert(0, "((");
        //            nameJoin.Append(") or (");
        //            nameJoin.Append(sbOrigName.ToString());
        //            nameJoin.Append("))");
        //        }
        //    }
        //}
        return nameJoin.ToString();
    }

    private string BuildPatientNameWhereClause()
    {
        StringBuilder nameClause = new StringBuilder();
        if (!String.IsNullOrEmpty(KeyName))
        {
            if (FuzzySearchOnName && !ExactMatchOnly)
            {
                StringBuilder sbKeyName = new StringBuilder();
                WhereLike(sbKeyName, "UMHS.KEYNAME", "@KEYNAME", KeyName);
                sbKeyName.Append(" or soundex(UMHS.KEYNAME) = soundex(@KEYNAME)");
                Where(nameClause, "(" + sbKeyName.ToString() + ")", "@KEYNAME", KeyName);
            }
            else
            {
                WhereLike(nameClause, "UMHS.KEYNAME", "@KEYNAME", KeyName);
            }
            if (QuickFind)
            {
                if (System.Text.RegularExpressions.Regex.Match(KeyName, "\\d").Success)
                {
                    nameClause.Append(" OR (LOOKUPID like '%" + KeyName.Replace("'","") + "%')");
                    //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                    //ElseIf System.Text.RegularExpressions.Regex.Match(KeyName, "\d").Success And System.Text.RegularExpressions.Regex.Match(KeyName, "\w").Success Then
                    //nameClause.Append(" OR ((LOOKUPID like '%" + KeyName + "%') AND ((CONSTITUENT.FIRSTNAME like '%" + KeyName + "%') OR (CONSTITUENT.KEYNAME like '%" + KeyName + "%')))")
                }
            }
        }
        if (!String.IsNullOrEmpty(MiddleName))
        {
            WhereLike(nameClause, "UMHS.MIDDLENAME", "@MIDDLENAME", MiddleName);
            //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (!SuffixCodeID.Equals(Guid.Empty))
        {
           Where(nameClause, "UMHS.SUFFIXCODEID = @SUFFIXCODEID", "@SUFFIXCODEID", SuffixCodeID);
        }

        if (!String.IsNullOrEmpty(FirstName))
        {
            // If only groups and orgs are being searched, disregard the first name.
            if (IncludeIndividuals.Value)
            {
                if (CheckNickname)
                {
                    if (!_cmd.Parameters.Contains("@FIRSTNAME"))
                    {
                        _cmd.Parameters.AddWithValue("@FIRSTNAME", GetLikeParameterValue(FirstName));
                    }

                    if (FuzzySearchOnName && !ExactMatchOnly)
                    {
                        Where(nameClause, string.Format("(UMHS.FIRSTNAME like @FIRSTNAME escape '{0}' or UMHS.NICKNAME like @FIRSTNAME escape '{0}' or soundex(UMHS.FIRSTNAME) = soundex(@FIRSTNAME) or soundex(UMHS.NICKNAME) = SOUNDEX(@FIRSTNAME))", ESCAPE_CHAR));
                    }
                    else
                    {
                        Where(nameClause, string.Format("(UMHS.FIRSTNAME like @FIRSTNAME escape '{0}' or CONSTITUENT.NICKNAME like @FIRSTNAME or UMHS.NICKNAME like @FIRSTNAME escape '{0}')", ESCAPE_CHAR));
                    }
                }
                else
                {
                    if (FuzzySearchOnName && !ExactMatchOnly)
                    {
                        StringBuilder sbFirstName = new StringBuilder();
                        WhereLike(sbFirstName, "UMHS.FIRSTNAME", "@FIRSTNAME", FirstName);
                        sbFirstName.Append(" or soundex(UMHS.FIRSTNAME) = soundex(@FIRSTNAME)");
                        Where(nameClause, "(" + sbFirstName.ToString() + ")", "@FIRSTNAME", FirstName);
                    }
                    else
                    {
                        WhereLike(nameClause,"UMHS.FIRSTNAME", "@FIRSTNAME", FirstName);
                    }
                }

            }

            //If this is a quick find ensure orgs with spaces in the name can be found.
            if (!String.IsNullOrEmpty(KeyName))
            {
                if (!KeyName.Contains("UMHS".ToLower()))
                {
                    StringBuilder sbOrigName = new StringBuilder();

                    WhereLike(sbOrigName, "UMHS.KEYNAME", "@FULLNAME", string.Format("{0} {1}", FirstName, KeyName));

                    if (nameClause.Length > 0)
                    {
                        nameClause.Insert(0, "((");
                        nameClause.Append(") or (");
                        nameClause.Append(sbOrigName.ToString());
                        nameClause.Append("))");
                    }
                }
            }
        }
        return nameClause.ToString();
    }
    private string BuildNameWhereClause(bool isAlias)
    {
        string CONSTITorALIAS = Convert.ToString((isAlias ? "ALIAS" : "CONSTITUENT"));
        StringBuilder nameClause = new StringBuilder();

        //If Len(KeyName) > 0 AndAlso KeyName.Contains("UMHS") Then
        //    If FuzzySearchOnName AndAlso Not ExactMatchOnly Then
        //        Dim sbKeyName As New StringBuilder
        //        WhereLike(sbKeyName, CONSTITorALIAS & ".KEYNAME", "@KEYNAME", KeyName)
        //        sbKeyName.Append(" or soundex(" & CONSTITorALIAS & ".KEYNAME) = soundex(@KEYNAME)")
        //        Where(nameClause, "(" & sbKeyName.ToString & ")", "@KEYNAME", KeyName)
        //        nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'")
        //    Else
        //        WhereLike(nameClause, CONSTITorALIAS & ".KEYNAME", "@KEYNAME", KeyName)
        //    End If
        //    If QuickFind Then
        //        If System.Text.RegularExpressions.Regex.Match(KeyName, "\d").Success Then
        //            nameClause.Append(" OR (LOOKUPID like '%" + KeyName + "%')")
        //            nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'")
        //            'ElseIf System.Text.RegularExpressions.Regex.Match(KeyName, "\d").Success And System.Text.RegularExpressions.Regex.Match(KeyName, "\w").Success Then
        //            'nameClause.Append(" OR ((LOOKUPID like '%" + KeyName + "%') AND ((CONSTITUENT.FIRSTNAME like '%" + KeyName + "%') OR (CONSTITUENT.KEYNAME like '%" + KeyName + "%')))")
        //        End If
        //    End If
        //End If


        //AndAlso Not KeyName.Contains("UMHS") Then
        if (!String.IsNullOrEmpty(KeyName))
        {
            if (FuzzySearchOnName && !ExactMatchOnly)
            {
                StringBuilder sbKeyName = new StringBuilder();
                WhereLike(sbKeyName, CONSTITorALIAS + ".KEYNAME", "@KEYNAME", KeyName);
                sbKeyName.Append(" or soundex(" + CONSTITorALIAS + ".KEYNAME) = soundex(@KEYNAME)");
                Where(nameClause, "(" + sbKeyName.ToString() + ")", "@KEYNAME", KeyName);
                //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }
            else
            {
                WhereLike(nameClause, CONSTITorALIAS + ".KEYNAME", "@KEYNAME", KeyName);
                //WhereLikeOr(nameClause, "UMHS.KEYNAME", "@KEYNAME", KeyName);
                //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }
            if (QuickFind)
            {
                if (System.Text.RegularExpressions.Regex.Match(KeyName, "\\d").Success)
                {
                    nameClause.Append(" OR (LOOKUPID like '%" + KeyName + "%')");
                    //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                    //ElseIf System.Text.RegularExpressions.Regex.Match(KeyName, "\d").Success And System.Text.RegularExpressions.Regex.Match(KeyName, "\w").Success Then
                    //nameClause.Append(" OR ((LOOKUPID like '%" + KeyName + "%') AND ((CONSTITUENT.FIRSTNAME like '%" + KeyName + "%') OR (CONSTITUENT.KEYNAME like '%" + KeyName + "%')))")
                }
            }
        }
        if (!String.IsNullOrEmpty(MiddleName))
        {
            WhereLike(nameClause, CONSTITorALIAS + ".MIDDLENAME", "@MIDDLENAME", MiddleName);
            //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (!SuffixCodeID.Equals(Guid.Empty))
        {
            if (!isAlias)
            {
                Where(nameClause, "CONSTITUENT.SUFFIXCODEID = @SUFFIXCODEID", "@SUFFIXCODEID", SuffixCodeID);
                //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }
        }

        if (!String.IsNullOrEmpty(FirstName))
        {
            // If only groups and orgs are being searched, disregard the first name.
            if (IncludeIndividuals.Value)
            {
                if (CheckNickname)
                {
                    if (!_cmd.Parameters.Contains("@FIRSTNAME"))
                    {
                        _cmd.Parameters.AddWithValue("@FIRSTNAME", GetLikeParameterValue(FirstName));
                    }

                    if (FuzzySearchOnName && !ExactMatchOnly)
                    {
                        Where(nameClause, string.Format("(" + CONSTITorALIAS + ".FIRSTNAME like @FIRSTNAME escape '{0}' or CONSTITUENT.NICKNAME like @FIRSTNAME escape '{0}' or soundex(" + CONSTITorALIAS + ".FIRSTNAME) = soundex(@FIRSTNAME) or soundex(CONSTITUENT.NICKNAME) = soundex(@FIRSTNAME))", ESCAPE_CHAR));
                        //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                    }
                    else
                    {
                        Where(nameClause, string.Format("(" + CONSTITorALIAS + ".FIRSTNAME like @FIRSTNAME escape '{0}' or CONSTITUENT.NICKNAME like @FIRSTNAME)", ESCAPE_CHAR));
                        //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                    }
                }
                else
                {
                    if (FuzzySearchOnName && !ExactMatchOnly)
                    {
                        StringBuilder sbFirstName = new StringBuilder();
                        WhereLike(sbFirstName, CONSTITorALIAS + ".FIRSTNAME", "@FIRSTNAME", FirstName);
                        sbFirstName.Append(" or soundex(" + CONSTITorALIAS + ".FIRSTNAME) = soundex(@FIRSTNAME)");
                        Where(nameClause, "(" + sbFirstName.ToString() + ")", "@FIRSTNAME", FirstName);
                        //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                    }
                    else
                    {
                        WhereLike(nameClause, CONSTITorALIAS + ".FIRSTNAME", "@FIRSTNAME", FirstName);
                        //WhereLikeOr(nameClause, "UMHS.FIRSTNAME", "@FIRSTNAME", FirstName);
                        //nameClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                    }
                }

            }

            //If this is a quick find ensure orgs with spaces in the name can be found.
            if (!String.IsNullOrEmpty(KeyName))
            {
                if (!KeyName.Contains("UMHS".ToLower()))
                {
                    StringBuilder sbOrigName = new StringBuilder();

                    WhereLike(sbOrigName, CONSTITorALIAS + ".KEYNAME", "@FULLNAME", string.Format("{0} {1}", FirstName, KeyName));

                    if (nameClause.Length > 0)
                    {
                        nameClause.Insert(0, "((");
                        nameClause.Append(") or (");
                        nameClause.Append(sbOrigName.ToString());
                        nameClause.Append("))");
                    }
                }
            }
        }

        return nameClause.ToString();
    }
    private void buildPatientWhere(StringBuilder whereClause)
    {
        //Gender
        if (GenderCode != null)
        {
            Where(whereClause, "UMHS.GENDERCODE = @GENDERCODE", "@GENDERCODE", GenderCode);
        }

        //SCU
        if (SCU != Guid.Empty)
        {
            Where(whereClause, "CONSTITUENT.ID in (select ehprimary.CONSTITUENTID from EDUCATIONADDITIONALINFORMATION eaiprimary inner join EDUCATIONALHISTORY ehprimary on eaiprimary.EDUCATIONALHISTORYID = ehprimary.ID where eaiprimary.EDUCATIONALCOLLEGECODEID = @SCU ) ", "@SCU", SCU);
        }

        #region Address Fields
        if (!String.IsNullOrEmpty(AddressBlock))
        {
            WhereLike(whereClause, "USR_UMHS_ADDRESS.ADDRESSBLOCK", "@ADDRESSBLOCK", AddressBlock);
        }

        if (!String.IsNullOrEmpty(City))
        {
            WhereLike(whereClause, "USR_UMHS_ADDRESS.CITY", "@CITY", City);
        }

        if (StateID != Guid.Empty)
        {
            Where(whereClause, "USR_UMHS_ADDRESS.STATEID = @STATEID", "@STATEID", StateID);
        }

        if (CountryID != Guid.Empty)
        {
            Where(whereClause, "USR_UMHS_ADDRESS.COUNTRYID = @COUNTRYID", "@COUNTRYID", CountryID);
        }

        if (!String.IsNullOrEmpty(PostCode))
        {
            WhereLike(whereClause, "USR_UMHS_ADDRESS.POSTCODE", "@POSTCODE", PostCode);
        }
        #endregion


        //Main Constit Fields
        string nameWhereClause = BuildPatientNameWhereClause();
        if (nameWhereClause.Length > 0)
            Where(whereClause, nameWhereClause);
        if ((!IncludeNonConstituentRecords))
        {
            Where(whereClause, "CONSTITUENT.ISCONSTITUENT = 1");
        }

        if (!String.IsNullOrEmpty(SSN))
        {
            Where(whereClause, "CONSTITUENT.SSNINDEX = dbo.UFN_GET_MAC_FOR_TEXT(@SSN, 'dbo.CONSTITUENT') and convert(nvarchar, DecryptByKey(CONSTITUENT.SSN)) = @SSN", "@SSN", SSN);
        }
        //End Main Demographics

        //Email
        if (!String.IsNullOrEmpty(EmailAddress))
        {
            WhereLike(whereClause, "USR_UMHS_EMAILADDRESS.EMAILADDRESS", "@EMAILADDRESS", EmailAddress);
        }

        if (!String.IsNullOrEmpty(PhoneNumber))
        {
            //handle the case where the user types in the number formatted or not
            //for some reason if the PhoneNumber is "(" and you try to Replace(Replace(PhoneNumber, "(", String.Empty), ")", String.Empty) the string is returned as nothing
            //so rewrote as separate if blocks
            StringBuilder phoneStringBuilder = new StringBuilder();
            string unformattedPhone = PhoneNumber;
            if (unformattedPhone.Contains("("))
            {
                unformattedPhone = unformattedPhone.Replace("(", string.Empty);
            }
            if (unformattedPhone.Contains(")"))
            {
                unformattedPhone = unformattedPhone.Replace(")", string.Empty);
            }
            if (unformattedPhone.Contains("-"))
            {
                unformattedPhone = unformattedPhone.Replace("-", string.Empty);
            }
            //if (unformattedPhone.Length > 0)
            //{
            //    WhereLike(phoneStringBuilder, "PHONE.NUMBERNOFORMAT", "@PHONENUMBERNOFORMAT", unformattedPhone);
            //    phoneStringBuilder.Append(" or ");
            //}
            phoneStringBuilder.Append(" USR_UMHS_PHONE.NUMBER = @PHONENUMBER");
            Where(whereClause, "(" + phoneStringBuilder.ToString() + ")", "@PHONENUMBER", PhoneNumber);
        }

        if (ClassOf > 0)
        {
            Where(whereClause, "EDUCATIONALHISTORY.CLASSOF = @CLASSOF and EDUCATIONALHISTORY.ISPRIMARYRECORD = 1 and LOOKUPID NOT LIKE '8-%'", "@CLASSOF", ClassOf);
        }

        if (!String.IsNullOrEmpty(PrimaryBusiness))
        {
            WhereLike(whereClause, "ORG.KEYNAME", "@PRIMARYBUSINESS", PrimaryBusiness);
        }

        if (includeProspectManager)
        {
            Where(whereClause, "PROSPECT.PROSPECTMANAGERFUNDRAISERID = @PROSPECTMANAGERID", "@PROSPECTMANAGERID", ProspectManagerID);
        }

        if (includeConstituency)
        {
            BuildConstituencyWhere(whereClause);
        }

        if (!String.IsNullOrEmpty(SourceCode))
        {
            WhereLike(whereClause, "CONSTITUENTAPPEAL.SOURCECODE", "@SOURCECODE", SourceCode);
        }

        if (!IncludeDeceased)
        {
            Where(whereClause, "not exists (select top 1 ID from dbo.DECEASEDCONSTITUENT with (nolock) where DECEASEDCONSTITUENT.ID = CONSTITUENT.ID)");
        }

        if (!String.IsNullOrEmpty(MRN))
        {
            if (ExactMatchOnly)
            {
                Where(whereClause, "UMHS.MRN = @MRN", "@MRN", MRN);
            }
            else
            {
                WhereLike(whereClause, "UMHS.MRN", "@MRN", MRN);
            }
        }

        if (!String.IsNullOrEmpty(CPISEQUENCE))
        {
            if (ExactMatchOnly)
            {
                Where(whereClause, "UMHS.CPISEQUENCE = @CPISEQUENCE", "@CPISEQUENCE", CPISEQUENCE);
            }
            else
            {
                WhereLike(whereClause, "UMHS.CPISEQUENCE", "@CPISEQUENCE", CPISEQUENCE);
            }
        }

        if (!String.IsNullOrEmpty(BIRTHDATE))
        {
            if (BIRTHDATE != "00000000")
            {
                WhereLike(whereClause, "UMHS.BIRTHDATE", "@BIRTHDATE", BIRTHDATE);
            }
        }

        // LookupID, Include Deceased, and Include Inactive apply whether check merged is selected or not
        if (!String.IsNullOrEmpty(LookupID))
        {
            if (CheckAlternateLookupIDs)
            {
                if (ExactMatchOnly)
                {
                    Where(whereClause, "CONSTITUENTLOOKUPID.LOOKUPID = @LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
                else
                {
                    WhereLike(whereClause, "CONSTITUENTLOOKUPID.LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
            }
            else
            {
                if (ExactMatchOnly)
                {
                    Where(whereClause, "CONSTITUENT.LOOKUPID = @LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
                else
                {
                    WhereLike(whereClause, "CONSTITUENT.LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
            }
        }

        //Filter Constituent Types
        if (IncludeIndividuals.Value && IncludeOrganizations.Value && IncludeGroups.Value)
        {
            //Do nothing
        }
        else
        {
            if (!IncludeIndividuals.Value)
            {
                Where(whereClause, "(CONSTITUENT.ISORGANIZATION = 1 or CONSTITUENT.ISGROUP = 1)");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }

            if (!IncludeOrganizations.Value)
            {
                Where(whereClause, "CONSTITUENT.ISORGANIZATION = 0");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }

            if (!IncludeGroups.Value)
            {
                Where(whereClause, "CONSTITUENT.ISGROUP = 0");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }

        }

        if (ExcludeCustomGroups)
        {
            Where(whereClause, "coalesce(GD.GROUPTYPECODE, 0) = 0");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (ExcludeHouseholds)
        {
            Where(whereClause, "coalesce(GD.GROUPTYPECODE, 1) = 1");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (!IncludeInactive)
        {
            Where(whereClause, "CONSTITUENT.ISINACTIVE = 0");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        //Site Filter
        //if (SiteFilterMode != 0)
        //{
        //    StringBuilder siteFilterClause = new StringBuilder();
        //    siteFilterClause.AppendLine("exists (select 1 from dbo.CONSTITUENTSITE with (nolock)");
        //    siteFilterClause.AppendLine("inner join dbo.UFN_SITE_BUILDDATALISTSITEFILTER(@CURRENTAPPUSERID, @SITEFILTERMODE, @SITESSELECTED) on UFN_SITE_BUILDDATALISTSITEFILTER.SITEID = CONSTITUENTSITE.SITEID");
        //    siteFilterClause.AppendLine("where CONSTITUENTSITE.CONSTITUENTID = CONSTITUENT.ID )");

        //    Where(whereClause, siteFilterClause.ToString());
        //    AddParameter("@CURRENTAPPUSERID", this.RequestContext.AppUserInfo.AppUserDBId);
        //    AddParameter("@SITEFILTERMODE", SiteFilterMode);

        //    if (SitesSelected == null)
        //    {
        //        AddParameter("@SITESSELECTED", DBNull.Value);
        //    }
        //    else
        //    {
        //        FormField fieldInfo = new FormField();
        //        CollectionFieldDescriptor collDesptr = new CollectionFieldDescriptor();

        //        var _with1 = collDesptr;
        //        // ERROR: Not supported in C#: ReDimStatement

        //        _with1.Fields[0] = new FormField();

        //        var _with2 = _with1.Fields[0];
        //        _with2.FieldID = "SITEID";
        //        //_with2.Caption = My.Resources.Content.ConstituentSearch_SiteID;
        //        _with2.DataType = FormFieldDataType.Guid;

        //        var _with3 = fieldInfo;
        //        _with3.FieldID = "SITESSELECTED";
        //        _with3.DataType = FormFieldDataType.XML;
        //        _with3.Item = collDesptr;

        //        AddParameter("@SITESSELECTED", this.RequestArgs.Filter.Values("SITESSELECTED").SqlParameterValue(fieldInfo));
        //    }
        //}

    }

    private void buildBaseWhere(StringBuilder whereClause, bool isAlias)
    {
        //Gender
        if (GenderCode != null)
        {
            Where(whereClause, "CONSTITUENT.GENDERCODE = @GENDERCODE", "@GENDERCODE", GenderCode);
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        //SCU
        if (SCU != Guid.Empty)
        {
            Where(whereClause, "CONSTITUENT.ID in (select ehprimary.CONSTITUENTID from EDUCATIONADDITIONALINFORMATION eaiprimary inner join EDUCATIONALHISTORY ehprimary on eaiprimary.EDUCATIONALHISTORYID = ehprimary.ID where eaiprimary.EDUCATIONALCOLLEGECODEID = @SCU ) ", "@SCU", SCU);
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        #region Address Fields
        if (!String.IsNullOrEmpty(AddressBlock))
        {
            WhereLike(whereClause, "ADDRESS.ADDRESSBLOCK", "@ADDRESSBLOCK", AddressBlock);
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (!String.IsNullOrEmpty(City))
        {
            WhereLike(whereClause, "ADDRESS.CITY", "@CITY", City);
        }

        if (StateID != Guid.Empty)
        {
            Where(whereClause, "ADDRESS.STATEID = @STATEID", "@STATEID", StateID);
        }

        if (CountryID != Guid.Empty)
        {
            Where(whereClause, "ADDRESS.COUNTRYID = @COUNTRYID", "@COUNTRYID", CountryID);
        }

        if (!String.IsNullOrEmpty(PostCode))
        {
            WhereLike(whereClause, "ADDRESS.POSTCODE", "@POSTCODE", PostCode);
        }
        #endregion


        //Main Constit Fields

        string nameWhereClause = BuildNameWhereClause(isAlias);
        if (nameWhereClause.Length > 0)
            Where(whereClause, nameWhereClause);
        if ((!IncludeNonConstituentRecords))
        {
            Where(whereClause, "CONSTITUENT.ISCONSTITUENT = 1");
        }

        if (!String.IsNullOrEmpty(SSN))
        {
            Where(whereClause, "CONSTITUENT.SSNINDEX = dbo.UFN_GET_MAC_FOR_TEXT(@SSN, 'dbo.CONSTITUENT') and convert(nvarchar, DecryptByKey(CONSTITUENT.SSN)) = @SSN", "@SSN", SSN);
        }
        //End Main Demographics

        //Email
        if (!String.IsNullOrEmpty(EmailAddress))
        {
            WhereLike(whereClause, "EMAILADDRESS.EMAILADDRESS", "@EMAILADDRESS", EmailAddress);
        }

        if (!String.IsNullOrEmpty(PhoneNumber))
        {
            //handle the case where the user types in the number formatted or not
            //for some reason if the PhoneNumber is "(" and you try to Replace(Replace(PhoneNumber, "(", String.Empty), ")", String.Empty) the string is returned as nothing
            //so rewrote as separate if blocks
            StringBuilder phoneStringBuilder = new StringBuilder();
            string unformattedPhone = PhoneNumber;
            if (unformattedPhone.Contains("("))
            {
                unformattedPhone = unformattedPhone.Replace("(", string.Empty);
            }
            if (unformattedPhone.Contains(")"))
            {
                unformattedPhone = unformattedPhone.Replace(")", string.Empty);
            }
            if (unformattedPhone.Contains("-"))
            {
                unformattedPhone = unformattedPhone.Replace("-",string.Empty);
            }
            if (unformattedPhone.Length > 0)
            {
                WhereLike(phoneStringBuilder, "PHONE.NUMBERNOFORMAT", "@PHONENUMBERNOFORMAT", unformattedPhone);
                phoneStringBuilder.Append(" or ");
            }
            phoneStringBuilder.Append(" PHONE.NUMBER = @PHONENUMBER");
            Where(whereClause, "(" + phoneStringBuilder.ToString() + ")", "@PHONENUMBER", PhoneNumber);
        }

        if (ClassOf > 0)
        {
            Where(whereClause, "EDUCATIONALHISTORY.CLASSOF = @CLASSOF and EDUCATIONALHISTORY.ISPRIMARYRECORD = 1 and LOOKUPID NOT LIKE '8-%'", "@CLASSOF", ClassOf);
        }

        if (!String.IsNullOrEmpty(PrimaryBusiness))
        {
            WhereLike(whereClause, "ORG.KEYNAME", "@PRIMARYBUSINESS", PrimaryBusiness);
        }

        if (includeProspectManager)
        {
            Where(whereClause, "PROSPECT.PROSPECTMANAGERFUNDRAISERID = @PROSPECTMANAGERID", "@PROSPECTMANAGERID", ProspectManagerID);
        }

        if (includeConstituency)
        {
            BuildConstituencyWhere(whereClause);
        }

        if (!String.IsNullOrEmpty(SourceCode))
        {
            WhereLike(whereClause, "CONSTITUENTAPPEAL.SOURCECODE", "@SOURCECODE", SourceCode);
        }

        if (!IncludeDeceased)
        {
            Where(whereClause, "not exists (select top 1 ID from dbo.DECEASEDCONSTITUENT with (nolock) where DECEASEDCONSTITUENT.ID = CONSTITUENT.ID)");
        }

        if (!String.IsNullOrEmpty(MRN))
        {
            if (ExactMatchOnly)
            {
                Where(whereClause, "UMHS.MRN = @MRN", "@MRN", MRN);
            }
            else
            {
                WhereLike(whereClause, "UMHS.MRN", "@MRN", MRN);
            }
        }

        if (!String.IsNullOrEmpty(CPISEQUENCE))
        {
            if (ExactMatchOnly)
            {
                Where(whereClause, "UMHS.CPISEQUENCE = @CPISEQUENCE", "@CPISEQUENCE", CPISEQUENCE);
            }
            else
            {
                WhereLike(whereClause, "UMHS.CPISEQUENCE", "@CPISEQUENCE", CPISEQUENCE);
            }
        }

        if (!String.IsNullOrEmpty(BIRTHDATE))
        {
            if (BIRTHDATE != "00000000")
            {
                WhereLike(whereClause, "CONSTITUENT.BIRTHDATE", "@BIRTHDATE", BIRTHDATE);
            }
        }

        // LookupID, Include Deceased, and Include Inactive apply whether check merged is selected or not
        if (!String.IsNullOrEmpty(LookupID))
        {
            if (CheckAlternateLookupIDs)
            {
                if (ExactMatchOnly)
                {
                    Where(whereClause, "CONSTITUENTLOOKUPID.LOOKUPID = @LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
                else
                {
                    WhereLike(whereClause, "CONSTITUENTLOOKUPID.LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
            }
            else
            {
                if (ExactMatchOnly)
                {
                    Where(whereClause, "CONSTITUENT.LOOKUPID = @LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
                else
                {
                    WhereLike(whereClause, "CONSTITUENT.LOOKUPID", "@LOOKUPID", LookupID);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
            }
        }

        //Filter Constituent Types
        if (IncludeIndividuals.Value && IncludeOrganizations.Value && IncludeGroups.Value)
        {
            //Do nothing
        }
        else
        {
            if (!IncludeIndividuals.Value)
            {
                Where(whereClause, "(CONSTITUENT.ISORGANIZATION = 1 or CONSTITUENT.ISGROUP = 1)");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }

            if (!IncludeOrganizations.Value)
            {
                Where(whereClause, "CONSTITUENT.ISORGANIZATION = 0");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }

            if (!IncludeGroups.Value)
            {
                Where(whereClause, "CONSTITUENT.ISGROUP = 0");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
            }

        }

        if (ExcludeCustomGroups)
        {
            Where(whereClause, "coalesce(GD.GROUPTYPECODE, 0) = 0");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (ExcludeHouseholds)
        {
            Where(whereClause, "coalesce(GD.GROUPTYPECODE, 1) = 1");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        if (!IncludeInactive)
        {
            Where(whereClause, "CONSTITUENT.ISINACTIVE = 0");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

        //Site Filter
        //if (SiteFilterMode != 0)
        //{
        //    StringBuilder siteFilterClause = new StringBuilder();
        //    siteFilterClause.AppendLine("exists (select 1 from dbo.CONSTITUENTSITE with (nolock)");
        //    siteFilterClause.AppendLine("inner join dbo.UFN_SITE_BUILDDATALISTSITEFILTER(@CURRENTAPPUSERID, @SITEFILTERMODE, @SITESSELECTED) on UFN_SITE_BUILDDATALISTSITEFILTER.SITEID = CONSTITUENTSITE.SITEID");
        //    siteFilterClause.AppendLine("where CONSTITUENTSITE.CONSTITUENTID = CONSTITUENT.ID )");

        //    Where(whereClause, siteFilterClause.ToString());
        //    AddParameter("@CURRENTAPPUSERID", this.RequestContext.AppUserInfo.AppUserDBId);
        //    AddParameter("@SITEFILTERMODE", SiteFilterMode);

        //    if (SitesSelected == null)
        //    {
        //        AddParameter("@SITESSELECTED", DBNull.Value);
        //    }
        //    else
        //    {
        //        FormField fieldInfo = new FormField();
        //        CollectionFieldDescriptor collDesptr = new CollectionFieldDescriptor();

        //        var _with1 = collDesptr;
        //        // ERROR: Not supported in C#: ReDimStatement

        //        _with1.Fields[0] = new FormField();

        //        var _with2 = _with1.Fields[0];
        //        _with2.FieldID = "SITEID";
        //        //_with2.Caption = My.Resources.Content.ConstituentSearch_SiteID;
        //        _with2.DataType = FormFieldDataType.Guid;

        //        var _with3 = fieldInfo;
        //        _with3.FieldID = "SITESSELECTED";
        //        _with3.DataType = FormFieldDataType.XML;
        //        _with3.Item = collDesptr;

        //        AddParameter("@SITESSELECTED", this.RequestArgs.Filter.Values("SITESSELECTED").SqlParameterValue(fieldInfo));
        //    }
        //}

    }

    private void buildGroupWhere(StringBuilder whereClause)
    {
        Where(whereClause, "((GMDR.DATEFROM is null and (GMDR.DATETO is null or GMDR.DATETO > @CURRENTDATEEARLIESTTIME)) " + "or (GMDR.DATETO is null and (GMDR.DATEFROM is null or GMDR.DATEFROM <= @CURRENTDATEEARLIESTTIME)) " + "or (GMDR.DATEFROM <= @CURRENTDATEEARLIESTTIME and GMDR.DATETO > @CURRENTDATEEARLIESTTIME))");

        whereClause.AppendLine(" and not exists(select RET.ID from " + OutputTable + " RET where RET.ID = GM.GROUPID)");
        //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

        if (!IncludeInactive)
        {
            Where(whereClause, "CONSTITUENT.ISINACTIVE = 0");
            //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
        }

    }


    private void BuildConstituencyWhere(StringBuilder whereClause)
    {
        switch (Constituency.ToString().ToUpper())
        {
            case "55FE8E7C-2B68-44C8-B35C-818AD1944C03":
                Where(whereClause, " CONSTITUENT.NETCOMMUNITYMEMBER = 1 and NETCOMMUNITYCLIENTUSER.ACTIVE = 1 and NETCOMMUNITYCLIENTUSER.DELETED = 0 ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "CEE46FE7-3FBB-4DFE-97EB-BA67DD33C634":
                Where(whereClause, " PLANNEDGIFT.STATUSCODE = 2 and exists (select ID from dbo.PLANNEDGIFTDESIGNATION where PLANNEDGIFTDESIGNATION.PLANNEDGIFTID=PLANNEDGIFT.ID)");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "3DFAC92E-78BD-4051-ABDC-02C675DEB8F6":
                Where(whereClause, " (CONSTITUENTRECOGNITION.EXPIRATIONDATE >= getdate() or RECOGNITIONPROGRAM.TYPECODE=1) ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "2D11326E-8F3B-4322-9797-57C1AACFA5DF":
                Where(whereClause, " MEMBER.ISDROPPED = 0 and (getdate() <= dbo.UFN_MEMBERSHIPLEVEL_CREATERENEWALAFTEREXPIRATIONDATE(MEMBERSHIP.MEMBERSHIPLEVELID,MEMBERSHIP.EXPIRATIONDATE) or MEMBERSHIPLEVELTERM.TERMCODE = 6) and MEMBERSHIP.STATUSCODE <> 1 ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "2D04A9C5-27D0-4646-BF0F-6826E4C12632":
                Where(whereClause, " MATCHFINDERCONSTITUENT.MATCHFINDERRECORDID Is Not null and MATCHFINDERCONSTITUENT.MATCHFINDERRECORDID <> 0 ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "4DB8F4FC-BC43-421D-B592-69BEF109B5FC":
                Where(whereClause, " (EHS.CONSTITUENCYIMPLIEDCODE = 0) ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "46EC3424-BA54-4431-A7DC-C6CEBB3B4592":
                Where(whereClause, " (EHS.CONSTITUENCYIMPLIEDCODE = 1) ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            case "093A3D4F-2974-447F-AD92-870EB4A04593":
                Where(whereClause, " dbo.UFN_CONSTITUENT_ISCOMMITTEE(GROUPMEMBER.GROUPID) = 1 ");
                //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");

                break;
            default:
                if (_constituencyIsUserDefined)
                {
                    Where(whereClause, " CONSTITUENCY.CONSTITUENCYCODEID = @CONSTITUENCY ", "@CONSTITUENCY", Constituency);
                    //whereClause.Append(" and CONSTITUENT.KEYNAME not like 'UMHS Friend%'");
                }
                break;
        }
    }

    protected virtual string GetColumnDBType(SearchListOutputFieldType field)
    {
        switch (field.FieldID.ToUpper())
        {
            case "ID":
                return "uniqueidentifier";
            case "NAME":
                return "nvarchar(700)";
            case "ADDRESS":
                return "nvarchar(300)";
            case "CITY":
                return "nvarchar(100)";
            case "STATE":
                return "nvarchar(100)";
            case "POSTCODE":
                return "nvarchar(24)";
            case "LOOKUPID":
                return "nvarchar(200)";
            case "GIVESANONYMOUSLY":
                return "bit";
            case "CLASSOF":
                return "nvarchar(100)";
            case "PRIMARYBUSINESS":
                return "nvarchar(100)";
            case "ISORGANIZATION":
                return "bit";
            case "CONSTITUENTTYPE":
                return "nvarchar(20)";
            case "SORTCONSTITUENTNAME":
                return "nvarchar(500)";
            case "EMAILADDRESS":
                return "nvarchar(200)";
            case "PHONE":
                return "nvarchar(100)";
            case "ISGROUP":
                return "bit";
            case "ISHOUSEHOLD":
                return "bit";
            case "COUNTRYID":
                return "nvarchar(100)";
            case "MIDDLENAME":
                return "nvarchar(100)";
            case "SUFFIXCODEID":
                return "nvarchar(100)";
            case "PROSPECTMANAGER":
                return "nvarchar(700)";
            case "PRIMARYMANAGERS":
                return "nvarchar(700)";
            case "DECEASED":
                return "nvarchar(10)";
            case "ADDRESSTYPE":
                return "nvarchar(50)";
            case "GENDER":
                return "nvarchar(7)";
            case "SCU":
                return "nvarchar(100)";
            case "ISMATCHINGORG":
                return "bit";
            case "MATCHINGORGNAME":
                return "nvarchar(100)";
            case "MRN":
                return "nvarchar(50)";
            case "CPISEQUENCE":
                return "nvarchar(50)";
            case "PLNAME":
                return "nvarchar(50)";
            case "UMHSID":
                return "uniqueidentifier";
            case "UMHSDATA":
                return "nvarchar(5)";
        }

        throw new Exception("Invalid Field ID specified for output: " + field.FieldID);

    }

    protected virtual string GetColumnSQL(SearchListOutputFieldType field, SearchType searchType)
    {
        switch (searchType)
        {
            case SearchType.Patient:
                switch (field.FieldID.ToUpper())
                {
                    case "ID":
                        return "CONSTITUENT.ID";
                    case "NAME":
                        if (searchType == UMHSDataConstituentSearchBase.SearchType.Aliases)
                        {
                            return "ALIAS.NAME";
                        }
                        else
                        {
                            return "NF.NAME";
                        }
                    case "ADDRESS":
                        return "USR_UMHS_ADDRESS.ADDRESSBLOCK";
                    case "CITY":
                        return "USR_UMHS_ADDRESS.CITY";
                    case "STATE":
                        return "dbo.UFN_STATE_GETDESCRIPTION(USR_UMHS_ADDRESS.STATEID)";
                    case "POSTCODE":
                        return "USR_UMHS_ADDRESS.POSTCODE";
                    case "LOOKUPID":
                        if (!(String.IsNullOrEmpty(LookupID) && CheckAlternateLookupIDs) && (searchType == UMHSDataConstituentSearchBase.SearchType.Base || searchType == UMHSDataConstituentSearchBase.SearchType.Aliases))
                        {
                            return "CONSTITUENTLOOKUPID.LOOKUPID";
                        }
                        return "CONSTITUENT.LOOKUPID";
                    case "GIVESANONYMOUSLY":
                        return "CONSTITUENT.GIVESANONYMOUSLY";
                    case "CLASSOF":
                        return BuildClassOfSQL();
                    case "PRIMARYBUSINESS":
                        return BuildPrimaryBusinessSQL();
                    case "ISORGANIZATION":
                        return "CONSTITUENT.ISORGANIZATION";
                    case "CONSTITUENTTYPE":
                        StringBuilder sb = new StringBuilder();

                        if (searchType == UMHSDataConstituentSearchBase.SearchType.GroupOfFound)
                        {
                            sb.AppendFormat("case when GD.GROUPTYPECODE=0 then '{0}' ", "Household");
                            sb.AppendFormat("else '{0}' ", "Group");
                        }
                        else
                        {
                            sb.AppendFormat("case when CONSTITUENT.ISGROUP = 0 and CONSTITUENT.ISORGANIZATION = 0 then '{0}' ", "Individual");
                            sb.AppendFormat("when CONSTITUENT.ISORGANIZATION = 1 then '{0}' ", "Organization");

                            //If includeGroupHouseholdsInConstituentTypeCase Then
                            sb.AppendFormat("when GD.GROUPTYPECODE=0 then '{0}' ", "Household");
                            sb.AppendFormat("else '{0}' ", "Group");
                            //End If

                        }
                        sb.Append(" end");

                        return sb.ToString();
                    case "SORTCONSTITUENTNAME":
                        if (searchType == UMHSDataConstituentSearchBase.SearchType.Aliases)
                        {
                            return "dbo.UFN_NAMEFORMAT_08(CONSTITUENT.ID, ALIAS.KEYNAME, ALIAS.FIRSTNAME, ALIAS.MIDDLENAME, null, null, null, null, null, null, null)";
                        }
                        else
                        {
                            return "dbo.UFN_NAMEFORMAT_08(CONSTITUENT.ID, CONSTITUENT.KEYNAME, CONSTITUENT.FIRSTNAME, CONSTITUENT.MIDDLENAME, null, null, null, null, null, null, null)";
                        }
                    case "EMAILADDRESS":
                        return "USR_UMHS_EMAILADDRESS.EMAILADDRESS";
                    case "ISGROUP":
                        return "CONSTITUENT.ISGROUP";
                    case "ISHOUSEHOLD":
                        return "case when GD.GROUPTYPECODE=0 then 1 else 0 end";
                    case "COUNTRYID":
                        return "dbo.UFN_COUNTRY_GETDESCRIPTION(USR_UMHS_ADDRESS.COUNTRYID)";
                    case "MIDDLENAME":
                        if (searchType == UMHSDataConstituentSearchBase.SearchType.Aliases)
                        {
                            return "ALIAS.MIDDLENAME";
                        }
                        else
                        {
                            return "CONSTITUENT.MIDDLENAME";
                        }
                    case "SUFFIXCODEID":
                        return "dbo.UFN_SUFFIXCODE_GETDESCRIPTION(CONSTITUENT.SUFFIXCODEID)";
                    case "PHONE":
                        return "USR_UMHS_PHONE.NUMBER";
                    case "PROSPECTMANAGER":
                        return BuildProspectManagerSQL();
                    case "PRIMARYMANAGERS":
                        return "dbo.UFN_PROSPECT_MANAGERS(CONSTITUENT.ID)";
                    case "DECEASED":
                        return "case (select 1 from DECEASEDCONSTITUENT where ID = CONSTITUENT.ID) when 1 then 'Yes' else 'No' end";
                    case "ADDRESSTYPE":
                        return "case when ((USR_UMHS_ADDRESS.HISTORICALENDDATE is null) or (USR_UMHS_ADDRESS.HISTORICALENDDATE >= @CURRENTDATE)) then coalesce(dbo.UFN_ADDRESSTYPECODE_GETDESCRIPTION(USR_UMHS_ADDRESS.ADDRESSTYPECODEID), N'') + N' (Current)' else coalesce(dbo.UFN_ADDRESSTYPECODE_GETDESCRIPTION(USR_UMHS_ADDRESS.ADDRESSTYPECODEID), N'') + N' (Former)' end as ADDRESSTYPE";
                    case "GENDER":
                        return "CONSTITUENT.GENDER";
                    case "SCU":
                        return "(select top 1 ecc.DESCRIPTION from EDUCATIONALCOLLEGECODE ecc inner join EDUCATIONADDITIONALINFORMATION eaiprimary on ecc.ID = eaiprimary.EDUCATIONALCOLLEGECODEID inner join EDUCATIONALHISTORY ehprimary on eaiprimary.EDUCATIONALHISTORYID = ehprimary.ID where ecc.ID = eaiprimary.EDUCATIONALCOLLEGECODEID and ehprimary.ISPRIMARYRECORD = 1 and ehprimary.CONSTITUENTID = CONSTITUENT.ID)";
                    case "ISMATCHINGORG":
                        return "case when exists (select 1 from MATCHINGGIFTCONDITION where ORGANIZATIONID = CONSTITUENT.ID) then 'true' else 'false' end";
                    case "MATCHINGORGNAME":
                        return "dbo.USR_UFN_GETMATCHINGGIFTORGANIZATIONNAME(CONSTITUENT.ID)";
                    case "MRN":
                        return "UMHS.MRN";
                    case "CPISEQUENCE":
                        return "UMHS.CPISEQUENCE";
                    case "PLNAME":
                        return "CONCAT(UMHS.KEYNAME,', ',UMHS.FIRSTNAME)";
                    case "UMHSID":
                        return "UMHS.ID";
                    case "UMHSDATA":
                        return "CASE WHEN UMHS.ID IS NOT NULL THEN 'Yes' ELSE 'No' END";
                }
                break;
            default:
                switch (field.FieldID.ToUpper())
                {
                    case "ID":
                        return "CONSTITUENT.ID";
                    case "NAME":
                        if (searchType == UMHSDataConstituentSearchBase.SearchType.Aliases)
                        {
                            return "ALIAS.NAME";
                        }
                        else
                        {
                            return "NF.NAME";
                        }
                    case "ADDRESS":
                        return "ADDRESS.ADDRESSBLOCK";
                    case "CITY":
                        return "ADDRESS.CITY";
                    case "STATE":
                        return "dbo.UFN_STATE_GETDESCRIPTION(ADDRESS.STATEID)";
                    case "POSTCODE":
                        return "ADDRESS.POSTCODE";
                    case "LOOKUPID":
                        if ((!String.IsNullOrEmpty(LookupID) && CheckAlternateLookupIDs) && (searchType == UMHSDataConstituentSearchBase.SearchType.Base || searchType == UMHSDataConstituentSearchBase.SearchType.Aliases))
                        {
                            return "CONSTITUENTLOOKUPID.LOOKUPID";
                        }
                        return "CONSTITUENT.LOOKUPID";
                    case "GIVESANONYMOUSLY":
                        return "CONSTITUENT.GIVESANONYMOUSLY";
                    case "CLASSOF":
                        return BuildClassOfSQL();
                    case "PRIMARYBUSINESS":
                        return BuildPrimaryBusinessSQL();
                    case "ISORGANIZATION":
                        return "CONSTITUENT.ISORGANIZATION";
                    case "CONSTITUENTTYPE":
                        StringBuilder sb = new StringBuilder();

                        if (searchType == UMHSDataConstituentSearchBase.SearchType.GroupOfFound)
                        {
                            sb.AppendFormat("case when GD.GROUPTYPECODE=0 then '{0}' ", "Household");
                            sb.AppendFormat("else '{0}' ", "Group");
                        }
                        else
                        {
                            sb.AppendFormat("case when CONSTITUENT.ISGROUP = 0 and CONSTITUENT.ISORGANIZATION = 0 then '{0}' ", "Individual");
                            sb.AppendFormat("when CONSTITUENT.ISORGANIZATION = 1 then '{0}' ", "Organization");

                            //If includeGroupHouseholdsInConstituentTypeCase Then
                            sb.AppendFormat("when GD.GROUPTYPECODE=0 then '{0}' ", "Household");
                            sb.AppendFormat("else '{0}' ", "Group");
                            //End If

                        }
                        sb.Append(" end");

                        return sb.ToString();
                    case "SORTCONSTITUENTNAME":
                        if (searchType == UMHSDataConstituentSearchBase.SearchType.Aliases)
                        {
                            return "dbo.UFN_NAMEFORMAT_08(CONSTITUENT.ID, ALIAS.KEYNAME, ALIAS.FIRSTNAME, ALIAS.MIDDLENAME, null, null, null, null, null, null, null)";
                        }
                        else
                        {
                            return "dbo.UFN_NAMEFORMAT_08(CONSTITUENT.ID, CONSTITUENT.KEYNAME, CONSTITUENT.FIRSTNAME, CONSTITUENT.MIDDLENAME, null, null, null, null, null, null, null)";
                        }
                    case "EMAILADDRESS":
                        return "EMAILADDRESS.EMAILADDRESS";
                    case "ISGROUP":
                        return "CONSTITUENT.ISGROUP";
                    case "ISHOUSEHOLD":
                        return "case when GD.GROUPTYPECODE=0 then 1 else 0 end";
                    case "COUNTRYID":
                        return "dbo.UFN_COUNTRY_GETDESCRIPTION(ADDRESS.COUNTRYID)";
                    case "MIDDLENAME":
                        if (searchType == UMHSDataConstituentSearchBase.SearchType.Aliases)
                        {
                            return "ALIAS.MIDDLENAME";
                        }
                        else
                        {
                            return "CONSTITUENT.MIDDLENAME";
                        }
                    case "SUFFIXCODEID":
                        return "dbo.UFN_SUFFIXCODE_GETDESCRIPTION(CONSTITUENT.SUFFIXCODEID)";
                    case "PHONE":
                        return "PHONE.NUMBER";
                    case "PROSPECTMANAGER":
                        return BuildProspectManagerSQL();
                    case "PRIMARYMANAGERS":
                        return "dbo.UFN_PROSPECT_MANAGERS(CONSTITUENT.ID)";
                    case "DECEASED":
                        return "case (select 1 from DECEASEDCONSTITUENT where ID = CONSTITUENT.ID) when 1 then 'Yes' else 'No' end";
                    case "ADDRESSTYPE":
                        return "case when ((ADDRESS.HISTORICALENDDATE is null) or (ADDRESS.HISTORICALENDDATE >= @CURRENTDATE)) then coalesce(dbo.UFN_ADDRESSTYPECODE_GETDESCRIPTION(ADDRESS.ADDRESSTYPECODEID), N'') + N' (Current)' else coalesce(dbo.UFN_ADDRESSTYPECODE_GETDESCRIPTION(ADDRESS.ADDRESSTYPECODEID), N'') + N' (Former)' end as ADDRESSTYPE";
                    case "GENDER":
                        return "CONSTITUENT.GENDER";
                    case "SCU":
                        return "(select top 1 ecc.DESCRIPTION from EDUCATIONALCOLLEGECODE ecc inner join EDUCATIONADDITIONALINFORMATION eaiprimary on ecc.ID = eaiprimary.EDUCATIONALCOLLEGECODEID inner join EDUCATIONALHISTORY ehprimary on eaiprimary.EDUCATIONALHISTORYID = ehprimary.ID where ecc.ID = eaiprimary.EDUCATIONALCOLLEGECODEID and ehprimary.ISPRIMARYRECORD = 1 and ehprimary.CONSTITUENTID = CONSTITUENT.ID)";
                    case "ISMATCHINGORG":
                        return "case when exists (select 1 from MATCHINGGIFTCONDITION where ORGANIZATIONID = CONSTITUENT.ID) then 'true' else 'false' end";
                    case "MATCHINGORGNAME":
                        return "dbo.USR_UFN_GETMATCHINGGIFTORGANIZATIONNAME(CONSTITUENT.ID)";
                    case "MRN":
                        return "UMHS.MRN";
                    case "CPISEQUENCE":
                        return "UMHS.CPISEQUENCE";
                    case "PLNAME":
                        return "CONCAT(UMHS.KEYNAME,', ',UMHS.FIRSTNAME)";
                    case "UMHSID":
                        return "UMHS.ID";
                    case "UMHSDATA":
                        return "CASE WHEN UMHS.ID IS NOT NULL THEN 'Yes' ELSE 'No' END";
                }
                break;
        }
        throw new Exception("Invalid Field ID specified for output: " + field.FieldID);


    }

    protected virtual bool AreGroupsIncluded()
    {
        return (!ExactMatchOnly && IncludeIndividuals.Value && IncludeGroups.Value);
    }

    private string BuildClassOfSQL()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("(Select TOP 1 EDUCATIONALHISTORY.CLASSOF from dbo.EDUCATIONALHISTORY with (nolock) ");
        sb.Append("inner join dbo.EDUCATIONALINSTITUTION with (nolock) on EDUCATIONALHISTORY.EDUCATIONALINSTITUTIONID = EDUCATIONALINSTITUTION.ID and EDUCATIONALINSTITUTION.ISAFFILIATED = 1 ");
        sb.Append("where EDUCATIONALHISTORY.CONSTITUENTID = CONSTITUENT.ID and EDUCATIONALHISTORY.ISPRIMARYRECORD = 1) as CLASSOF");

        return sb.ToString();
    }

    private string BuildPrimaryBusinessSQL()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("(Select TOP 1 ORG.NAME from dbo.RELATIONSHIP with (nolock) ");
        sb.Append("inner join dbo.CONSTITUENT as ORG with (nolock) on ORG.ID = RELATIONSHIP.RELATIONSHIPCONSTITUENTID ");
        sb.Append("where RELATIONSHIP.ISPRIMARYBUSINESS = 1 and ORG.ISORGANIZATION = 1 and RELATIONSHIP.RECIPROCALCONSTITUENTID = CONSTITUENT.ID) as PRIMARYBUSINESS");

        return sb.ToString();
    }

    private string BuildProspectManagerSQL()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("(select PROSPECTMANAGER_NF.NAME from dbo.PROSPECT with (nolock) ");
        sb.Append(" outer apply dbo.UFN_CONSTITUENT_DISPLAYNAME(PROSPECT.PROSPECTMANAGERFUNDRAISERID) PROSPECTMANAGER_NF");
        sb.Append(" where PROSPECT.ID = CONSTITUENT.ID and prospect.PROSPECTMANAGERENDDATE is null) as PROSPECTMANAGER ");
        return sb.ToString();
    }

    private bool includeEmailAddress
    {
        get
        {
            //Check for filter
            if (!String.IsNullOrEmpty(EmailAddress))
            {
                return true;
            }

            //Check for Return Column
            foreach (SearchListOutputFieldType outputfield in this.ProcessContext.OutputDefinition.OutputFields)
            {
                if (outputfield.FieldID.ToUpper() == "EMAILADDRESS")
                    return true;
            }

            return false;
        }
    }

    private bool includePhone
    {
        get
        {
            //Check for filter
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                return true;
            }

            //Check for Return Column
            foreach (SearchListOutputFieldType outputfield in this.ProcessContext.OutputDefinition.OutputFields)
            {
                if (outputfield.FieldID.ToUpper() == "PHONE")
                    return true;
            }

            return false;
        }
    }

    private bool includeProspectManager
    {
        get
        {
            if (!ProspectManagerID.Equals(Guid.Empty))
            {
                return true;
            }

            return false;
        }
    }

    private bool includeConstituency
    {
        get
        {
            if (!Constituency.Equals(Guid.Empty))
            {
                return true;
            }

            return false;
        }
    }

    private bool filterAddress
    {
        get
        {
            if (!String.IsNullOrEmpty(AddressBlock))
            {
                return true;
            }

            if (!String.IsNullOrEmpty(City))
            {
                return true;
            }

            if (StateID != Guid.Empty)
            {
                return true;
            }

            if (CountryID != Guid.Empty)
            {
                return true;
            }

            if (!String.IsNullOrEmpty(PostCode))
            {
                return true;
            }

            return false;

        }
    }

    protected virtual bool EncryptKeyNeeded
    {
        get { return !String.IsNullOrEmpty(SSN); }
    }

    private string getMergedLookupID()
    {
        MergedConstituentsExtensionSearch mcs = new MergedConstituentsExtensionSearch();

        using (System.Data.SqlClient.SqlConnection conn = RequestContext.OpenAppDBConnection())
        {
            // Check if the lookup ID is currently being used.  If not,
            // see if it was used by a merged constituent.
            int foundRowCount = 0;
            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
            {
                cmd.Connection = conn;

                cmd.CommandText = "select count(1) from dbo.CONSTITUENT with (nolock) where LOOKUPID = @LOOKUPID";
                cmd.Parameters.AddWithValue("@LOOKUPID", LookupID);
                foundRowCount = (int)cmd.ExecuteScalar();
            }

            if (foundRowCount == 0)
            {
                if (MinimumDate < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                {
                    MinimumDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                }

                // If the lookup ID wasn't found in a merged constituent, just keep using the current 
                // lookup ID since no rows should be returned.
                string newLookupID = mcs.FindConstituent(conn, LookupID, MinimumDate);
                if (!string.IsNullOrEmpty(newLookupID))
                {
                    //New Lookup ID found. Use it.
                    return newLookupID;
                }
            }
        }

        //No merged lookup ID found use original
        return LookupID;
    }


    protected void WhereOr(StringBuilder whereClause, string parameterizedClause, string parameterName, object parameterValue)
    {
        WhereOr(whereClause, parameterizedClause);
        AddParameter(parameterName, parameterValue);
    }

    protected void Where(StringBuilder whereClause, string parameterizedClause, string parameterName, object parameterValue)
    {
        Where(whereClause, parameterizedClause);
        AddParameter(parameterName, parameterValue);
    }

    protected void AddParameter(string parameterName, object parameterValue)
    {
        if (!_cmd.Parameters.Contains(parameterName))
        {
            _cmd.Parameters.AddWithValue(parameterName, parameterValue);
        }
    }

    protected void Where(StringBuilder whereClause, string clause)
    {
        if (whereClause.Length > 0)
            whereClause.AppendLine(" and ");
        whereClause.AppendLine(clause);
    }
    protected void WhereOr(StringBuilder whereClause, string clause)
    {
        if (whereClause.Length > 0)
            whereClause.AppendLine(" or ");
        whereClause.AppendLine(clause);
    }

    protected void WhereLike(StringBuilder whereClause, string fieldName, string parameterName, string parameterValue)
    {
        parameterValue = GetLikeParameterValue(parameterValue);
        Where(whereClause, string.Format("{0} like {1} escape '{2}'", fieldName, parameterName, ESCAPE_CHAR), parameterName, parameterValue);
    }

    protected void WhereLikeOr(StringBuilder whereClause, string fieldName, string parameterName, string parameterValue)
    {
        parameterValue = GetLikeParameterValue(parameterValue);
        WhereOr(whereClause, string.Format("{0} like {1} escape '{2}'", fieldName, parameterName, ESCAPE_CHAR), parameterName, parameterValue);
    }

    protected string GetLikeParameterValue(string parameterValue)
    {
        if (!String.IsNullOrEmpty(parameterValue))
        { 
            parameterValue = parameterValue.Replace(ESCAPE_CHAR, ESCAPE_CHAR + ESCAPE_CHAR).Replace("[", ESCAPE_CHAR + "[").Replace("]", ESCAPE_CHAR + "]");
            if (ExactMatchOnly)
            {
                parameterValue = parameterValue.Replace("%", ESCAPE_CHAR + "%").Replace("_", ESCAPE_CHAR + "_");
            }
            else
            {
                parameterValue = parameterValue + "%";
                parameterValue = parameterValue.Replace("*", "%").Replace("?", "_");
            }
        }
        return parameterValue;
    }
}
