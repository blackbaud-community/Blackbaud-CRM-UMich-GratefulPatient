--UPDATES BASE TEMPLATE FOR CREATING NEW BATCH TEMPLATES
declare @duplicateCheckID nvarchar(150) = N'60DCA9C9-9C1A-4F50-B255-EF0EB1414779', --'4DB3B9FD-3CE9-4FAC-9E9A-13AE387F958D' (OG)
		@searchListID nvarchar(150) = N'747530a1-be80-4054-a021-d2a599248261', --8D689026-E3B8-4D0E-8CBD-AC640039783C (OG)
		@lookupSearchListID nvarchar(150) = N'875b6cfb-b52f-45a0-bf30-9f1992ab28ae' --6C1DA9A2-766E-4B02-8878-5F5AF7EC527E (OG)

declare	@BATCHSPECID UNIQUEIDENTIFIER,
		@BATCHSPEC XML = NULL,
		@ADDSPECID UNIQUEIDENTIFIER,
		@ADDINSTANCEID UNIQUEIDENTIFIER,
		@ADDSPEC XML = NULL,
		@ADDFORM XML = NULL,
		@EDITSPECID UNIQUEIDENTIFIER,
		@EDITINSTANCEID UNIQUEIDENTIFIER,
		@EDITSPEC XML = NULL,
		@EDITFORM XML = NULL,
		@COMMITSPECID UNIQUEIDENTIFIER,
		@COMMITINSTANCEID UNIQUEIDENTIFIER,
		@COMMITSPEC XML = NULL,
		@COMMITFORM XML = NULL
  
select @BATCHSPECID = ID from BATCHTYPECATALOG where name = 'Enhanced Revenue Batch'
  
;with XMLNAMESPACES(default 'bb_appfx_adddataformtemplate')	
select  @BATCHSPEC = bc.SPECXML, 
		@ADDINSTANCEID = dc.TEMPLATESPECXML.value('/AddDataFormTemplateSpec[1]/@DataFormInstanceID', 'varchar(36)')
from dbo.[BATCHTYPECATALOG] bc
inner join dbo.DATAFORMTEMPLATECATALOG dc on bc.ADDROWDATAFORMTEMPLATEID = dc.ID
where  bc.ID = @BATCHSPECID	

SELECT @BATCHSPEC = B.SPECXML,
	   @ADDSPECID = A.ID,
       @ADDSPEC = A.TEMPLATESPECXML,
	   @ADDFORM = A.FORMDEFINITIONXML,
	   @EDITSPECID = E.ID,
       @EDITSPEC = E.TEMPLATESPECXML,
	   @EDITFORM = E.FORMDEFINITIONXML,
	   @COMMITSPECID = C.ID,
       @COMMITSPEC = C.TEMPLATESPECXML,
	   @COMMITFORM = C.FORMDEFINITIONXML
FROM   BATCHTYPECATALOG B
       LEFT JOIN DATAFORMTEMPLATECATALOG A ON A.ID = B.ADDROWDATAFORMTEMPLATEID
	   left join DATAFORMINSTANCECATALOG AI ON AI.DATAFORMTEMPLATECATALOGID = B.ADDROWDATAFORMTEMPLATEID
       LEFT JOIN DATAFORMTEMPLATECATALOG E ON E.ID = B.EDITROWDATAFORMTEMPLATEID
       LEFT JOIN DATAFORMTEMPLATECATALOG C ON C.ID = B.COMMITROWADDDATAFORMTEMPLATEID
       LEFT JOIN TABLECATALOG TC ON TC.TABLENAME = B.BASETABLENAME
       LEFT JOIN RECORDOPERATIONCATALOG ROC ON ROC.ID = B.ROWRECORDOPERATIONID
       LEFT JOIN DATAFORMTEMPLATECATALOG CE ON CE.ID = B.COMMITROWEDITDATAFORMTEMPLATEID
WHERE  B.ID = @BATCHSPECID

--SELECT @BATCHSPEC,@ADDSPEC,@EDITSPEC,@COMMITSPEC,@ADDFORM,@EDITFORM,@COMMITFORM

DECLARE @t TABLE (
	rowId INT IDENTITY PRIMARY KEY, 
	specID uniqueidentifier, 
	xmlData XML 
)

INSERT INTO @t ( specID, xmlData ) VALUES ( @BATCHSPECID, @BATCHSPEC )
INSERT INTO @t ( specID, xmlData ) VALUES ( @ADDSPECID, @ADDSPEC)
INSERT INTO @t ( specID, xmlData ) VALUES ( @EDITSPECID, @EDITSPEC)
INSERT INTO @t ( specID, xmlData ) VALUES ( @COMMITSPECID, @COMMITSPEC)
INSERT INTO @t ( specID, xmlData ) VALUES ( @ADDINSTANCEID, @ADDFORM)

--check data before
--SELECT 'BEFORE' S, DATALENGTH(XMLDATA) DL, SPECID, XMLDATA FROM @T

--Update Batch Duplicate Search Form
UPDATE @t SET 
xmlData.modify('declare default element namespace "bb_appfx_batchtype"; 
replace value of (/BatchTypeSpec/DuplicateRecordCheck/@SearchListID)[1] with "60DCA9C9-9C1A-4F50-B255-EF0EB1414779"')
WHERE rowId = 1

--Update Add Form Constituent and Lookup ID searches
UPDATE @t SET 
xmlData.modify('declare default element namespace "bb_appfx_adddataformtemplate"; declare namespace d1p1="bb_appfx_commontypes"; 
replace value of (/AddDataFormTemplateSpec/d1p1:FormMetaData/d1p1:FormFields/d1p1:FormField[@FieldID=("CONSTITUENTID")]/d1p1:SearchList/@SearchListID)[1] with "8D689026-E3B8-4D0E-8CBD-AC640039783C"')
WHERE rowId = 2

UPDATE @t SET 
xmlData.modify('declare default element namespace "bb_appfx_adddataformtemplate"; declare namespace d1p1="bb_appfx_commontypes"; 
replace value of (/AddDataFormTemplateSpec/d1p1:FormMetaData/d1p1:FormFields/d1p1:FormField[@FieldID=("CONSTITUENTLOOKUPID")]/d1p1:SearchList/@SearchListID)[1] with "6C1DA9A2-766E-4B02-8878-5F5AF7EC527E"')
WHERE rowId = 2

--Update Edit Form Constituent and Lookup ID searches
UPDATE @t SET 
xmlData.modify('declare default element namespace "bb_appfx_editdataformtemplate"; declare namespace d1p1="bb_appfx_commontypes"; 
replace value of (/EditDataFormTemplateSpec/d1p1:FormMetaData/d1p1:FormFields/d1p1:FormField[@FieldID=("CONSTITUENTID")]/d1p1:SearchList/@SearchListID)[1] with "8D689026-E3B8-4D0E-8CBD-AC640039783C"')
WHERE rowId = 3

UPDATE @t SET 
xmlData.modify('declare default element namespace "bb_appfx_editdataformtemplate"; declare namespace d1p1="bb_appfx_commontypes"; 
replace value of (/EditDataFormTemplateSpec/d1p1:FormMetaData/d1p1:FormFields/d1p1:FormField[@FieldID=("CONSTITUENTLOOKUPID")]/d1p1:SearchList/@SearchListID)[1] with "6C1DA9A2-766E-4B02-8878-5F5AF7EC527E"')
WHERE rowId = 3

--UPDATE ADD FORM DEFINITION
UPDATE @t SET 
xmlData.modify('declare namespace c="bb_appfx_commontypes"; 
replace value of (/c:FormMetaData/c:FormFields/c:FormField[@FieldID=("CONSTITUENTID")]/c:SearchList/@SearchListID)[1] with "8D689026-E3B8-4D0E-8CBD-AC640039783C"')
WHERE rowId = 5

UPDATE @t SET 
xmlData.modify('declare namespace c="bb_appfx_commontypes"; 
replace value of (/c:FormMetaData/c:FormFields/c:FormField[@FieldID=("CONSTITUENTLOOKUPID")]/c:SearchList/@SearchListID)[1] with "6C1DA9A2-766E-4B02-8878-5F5AF7EC527E"')
WHERE rowId = 5

--validate data
--SELECT 'AFTER' S, DATALENGTH(XMLDATA) DL,SPECID, XMLDATA FROM @T

UPDATE BATCHTYPECATALOG SET SPECXML = (SELECT XMLDATA FROM @t WHERE ROWID = 1) --update the duplicate search in the batch type spec
WHERE ID = @BATCHSPECID

UPDATE DATAFORMTEMPLATECATALOG SET 
	TEMPLATESPECXML = (SELECT XMLDATA FROM @T WHERE ROWID = 2), --replace constituentid and constituentlookupid searchlists
	FORMDEFINITIONXML = (select xmldata from @t where rowId = 5) --addform definition
WHERE ID = @ADDSPECID

--update the instance that create a new batch template
UPDATE DATAFORMINSTANCECATALOG SET 
	FORMUIXML = (select xmldata from @t where rowId = 5) --addform definition
WHERE ID = @ADDINSTANCEID
