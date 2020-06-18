
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Blackbaud.AppFx.Server;
using Blackbaud.AppFx;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public class MergedConstituentsExtensionSearch : Blackbaud.AppFx.Server.AppCatalog.AppSearchList
{


    private const string MERGEDSTATUS = "Merged";
    public string LookupID;

    public DateTime FloorDate;

    private SqlCommand _cmd;
    public override Blackbaud.AppFx.Server.AppCatalog.AppSearchListResult GetSearchResults()
    {
        List<ListOutputRow> resultList = new List<ListOutputRow>();

        if (FloorDate < SqlDateTime.MinValue.Value)
            FloorDate = SqlDateTime.MinValue.Value;

        _cmd = new SqlCommand();
        _cmd.CommandTimeout = this.ProcessContext.TimeOutSeconds;
        using (SqlConnection conn = this.RequestContext.OpenAppDBConnection())
        {
            _cmd.Connection = conn;

            //Check the constituent merge operations table for the lookupid
            string newLookupID = null;
            newLookupID = FindConstituent(conn, LookupID, FloorDate);

            if (!string.IsNullOrEmpty(newLookupID))
            {
                _cmd.Parameters.Clear();
                _cmd.CommandText = "select C.ID, C.NAME from CONSTITUENT C where C.LOOKUPID = @lookupID";
                _cmd.Parameters.AddWithValue("@lookupID", newLookupID);
                SqlDataReader rdr = _cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    List<string> fields = new List<string>();
                    rdr.Read();
                    fields.Add(rdr.GetGuid(rdr.GetOrdinal("ID")).ToString());
                    fields.Add(rdr.GetString(rdr.GetOrdinal("NAME")));
                    fields.Add(newLookupID);
                    ListOutputRow output = new ListOutputRow();
                    output.Values = fields.ToArray();
                    resultList.Add(output);
                }
                rdr.Close();
            }
        }

        return new Blackbaud.AppFx.Server.AppCatalog.AppSearchListResult(resultList);
    }

    internal string FindConstituent(SqlConnection conn, string id, DateTime minDate)
    {
        //Build the SQL command that will be used to search for the lookupID
        //It is important to order the results by DATEADDED so that we always
        //retrieve the oldest record that meets the criteria.  This is done
        //to ameliorate the effect of recycling lookupid's - a practice
        //used by several organizations.
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "select TARGETLOOKUPID, DATEADDED " + "from dbo.CONSTITUENTMERGEOPERATIONS " + "where SOURCELOOKUPID = @LOOKUPID and DATEADDED >= @FLOORDATE " + "order by DATEADDED asc";
        cmd.Parameters.Add("@LOOKUPID", SqlDbType.NVarChar);
        cmd.Parameters.Add("@FLOORDATE", SqlDbType.DateTime);

        //Search for the given lookup id
        string newLookupID = null;
        newLookupID = SearchForLookupID(cmd, id, minDate);

        //If the original lookupid was returned then no merge was found.
        if (newLookupID != null)
        {
            if (newLookupID.Equals(id))
                return null;
        }


        return newLookupID;
    }

    //Recursive function that will search for the logged merge operations
    //to find the existing constituent into which the constituent with
    //the given lookupid was merged.
    private string SearchForLookupID(SqlCommand cmd, string id, DateTime minDate)
    {

        object lookupIDParamValue = null;
        if (id != null)
        {
            lookupIDParamValue = id;
        }
        else
        {
            lookupIDParamValue = DBNull.Value;
        }

        cmd.Parameters.AddWithValue("@LOOKUPID", lookupIDParamValue);
        cmd.Parameters.AddWithValue("@FLOORDATE", minDate);
        SqlDataReader rdr = cmd.ExecuteReader();
        if (rdr.HasRows)
        {
            //If we found a match then search to see if the
            //target itself was merged in a later operation
            rdr.Read();
            string newID = rdr.GetString(rdr.GetOrdinal("TARGETLOOKUPID"));
            DateTime dte = rdr.GetDateTime(rdr.GetOrdinal("DATEADDED"));
            rdr.Close();
            return SearchForLookupID(cmd, newID, dte);
        }
        else
        {
            //if no match was found then return the given lookupid
            rdr.Close();
            return id;
        }
    }

}
