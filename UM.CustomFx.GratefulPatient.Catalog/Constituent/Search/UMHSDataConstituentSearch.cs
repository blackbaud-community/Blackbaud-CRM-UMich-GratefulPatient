using Blackbaud.AppFx.Server;
using Blackbaud.AppFx.Server.AppCatalog;
using System.Text;

namespace UM.CustomFx.GratefulPatient.Catalog.Constituent.Search
{
    public sealed class UMHSDataConstituentSearch : UMHSDataConstituentSearchBase
    {
        public bool OnlyProspects;
        public bool OnlyFundraisers;
        public bool OnlyStaff;
        public bool OnlyBoardMembers;
        public bool OnlyVolunteers;

        public bool OnlyMatchingGiftOrganizations;

        public bool OnlySchools = false;
        protected override void BuildFrom(StringBuilder fromClause, SearchType searchType)
        {
            base.BuildFrom(fromClause, searchType);

            switch (searchType)
            {
                case UMHSDataConstituentSearchBase.SearchType.Base:
                case UMHSDataConstituentSearchBase.SearchType.Aliases:
                    if (OnlyStaff)
                    {
                        fromClause.AppendLine("inner join dbo.STAFFDATERANGE with (nolock) on STAFFDATERANGE.CONSTITUENTID = CONSTITUENT.ID and (STAFFDATERANGE.DATEFROM <= @CURRENTDATEEARLIESTTIME or STAFFDATERANGE.DATEFROM is null) and (@CURRENTDATEEARLIESTTIME <= STAFFDATERANGE.DATETO or STAFFDATERANGE.DATETO is null)");
                    }
                    if (OnlyBoardMembers)
                    {
                        fromClause.AppendLine("inner join dbo.BOARDMEMBERDATERANGE with (nolock) on BOARDMEMBERDATERANGE.CONSTITUENTID = CONSTITUENT.ID and (BOARDMEMBERDATERANGE.DATEFROM <= @CURRENTDATEEARLIESTTIME or BOARDMEMBERDATERANGE.DATEFROM is null) and (@CURRENTDATEEARLIESTTIME <= BOARDMEMBERDATERANGE.DATETO or BOARDMEMBERDATERANGE.DATETO is null)");
                    }
                    if (OnlyFundraisers)
                    {
                        fromClause.AppendLine("inner join dbo.FUNDRAISERDATERANGE with (nolock) on FUNDRAISERDATERANGE.CONSTITUENTID = CONSTITUENT.ID and (FUNDRAISERDATERANGE.DATEFROM <= @CURRENTDATEEARLIESTTIME or FUNDRAISERDATERANGE.DATEFROM is null) and (@CURRENTDATEEARLIESTTIME <= FUNDRAISERDATERANGE.DATETO or FUNDRAISERDATERANGE.DATETO is null)");
                    }

                    if (OnlyProspects)
                    {
                        fromClause.AppendLine("inner join dbo.PROSPECTDATERANGE with (nolock) on PROSPECTDATERANGE.CONSTITUENTID = CONSTITUENT.ID and (PROSPECTDATERANGE.DATEFROM <= @CURRENTDATEEARLIESTTIME or PROSPECTDATERANGE.DATEFROM is null) and (@CURRENTDATEEARLIESTTIME <= PROSPECTDATERANGE.DATETO or PROSPECTDATERANGE.DATETO is null)");
                    }

                    if (OnlyVolunteers)
                    {
                        fromClause.AppendLine("inner join dbo.VOLUNTEERDATERANGE with (nolock) on VOLUNTEERDATERANGE.CONSTITUENTID = CONSTITUENT.ID and (VOLUNTEERDATERANGE.DATEFROM <= @CURRENTDATEEARLIESTTIME or VOLUNTEERDATERANGE.DATEFROM is null) and (@CURRENTDATEEARLIESTTIME <= VOLUNTEERDATERANGE.DATETO or VOLUNTEERDATERANGE.DATETO is null)");
                    }

                    if (OnlySchools)
                    {
                        fromClause.AppendLine("inner join dbo.SCHOOL with (nolock) on SCHOOL.ID = C.ID");
                    }

                    break;
            }
        }
    }
}