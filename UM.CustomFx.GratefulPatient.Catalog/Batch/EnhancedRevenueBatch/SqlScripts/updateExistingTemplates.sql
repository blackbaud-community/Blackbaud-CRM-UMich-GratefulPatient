begin transaction

declare @batchTemplates table
(
	ID UNIQUEIDENTIFIER,
	NAME NVARCHAR(200)
)

INSERT INTO @batchTemplates
select '93A2824E-E7B2-427A-B59B-56B51A6E0674','(UM) AAUM LEAD One Time Payment'
union
select 'D1B8A8C0-B354-47AD-B2D7-5B35FB71DC6D','(UM) AAUM LEAD Pledge Full'
union
select '67552883-0BF7-4E16-99D3-9C68DD808C84','(UM) AAUM Membership Annual Batch'
union
select 'CA4DBA2E-DD59-4677-BCDE-E04799100346','(UM) AAUM Membership Life Full Batch'
union
select '6C336C49-28FF-49D8-B7DC-C63ED704F30B','(UM) Acct Recon & Sponsored Grant Entry Batch'
union
select '4F703D8F-96BA-4D14-AF80-28DDCD40618B','(UM) Athletics Batch'
union
select 'C277782D-CFBE-4FEC-B10E-C47DD39BF662','(UM) Athletics Ticket Transfer Import Batch'
union
select '9CEFA41A-8A47-4706-85A9-48BC4F18961A','(UM) Batch for Pledge Import'
union
select 'B80624DA-6E78-4F6D-ACBE-ADBA2C81F88F','(UM) Credit Card and ACH Processing for Recurring Gifts'
union
select '6F595455-BDAE-4207-AEA3-17B054F09B16','(UM) Do Not Post Batch'
union
select '36EB7B53-D759-4E28-97FE-EE0905B5C61A','(UM) GIK Gift In Kind Import Batch'
union
select '570BD04F-47D0-40F3-8782-D4138ACBB9AD','(UM) GIK Gift In Kind Manual Entry Batch'
union
select 'DB0F57FA-E94E-468A-9B51-49D8CFF89E29','(UM) GPS Credit Card Charge & Payments Batch'
union
select '4CAB7818-5672-45A4-8F51-B61CD50377C9','(UM) GRA Manual Entry Batch'
union
select 'B1EC4A85-3644-4C6A-8B04-C4590648BF74','(UM) Kintera Import'
union
select 'AD6711D8-7A54-44A7-8863-9E768BB6BAAA','(UM) Mellon Bank Lockbox Batch'
union
select 'A4F73164-0589-44F9-95A8-32D8F90434AE','(UM) Radio Revenue Import Batch'
union
select '6F297409-B00C-4515-8088-F0CB7BB6D2AE','(UM) Securities Batch'
union
select '2F90889B-D390-43AD-BCF8-6538E31A85D7','(UM) Super Revenue Batch'
union
select '0E246F75-0A9E-454F-AEEE-24B178D06C2D','(UM) TeamRaiser Import'
union
select 'F13FE503-D91C-4E7C-8FFD-61ACA1B213C2','(UM) Telefund BBIS Payment and Recurring Batch Template'
union
select '5BB3BBC6-D457-4E80-B26F-188C15B9276F','(UM) Telefund Payments Batch'
union
select '93F10C8E-5C71-4F3B-BDC1-58AFC5B94B3D','(UM) Telefund Pledge Batch'
union
select '1DACAD5B-0F13-40BF-A668-89A982FF0214','(UM) Third Party Gifts'

while exists(select 1 from @batchTemplates)
BEGIN
	declare @duplicateCheckID nvarchar(150) = N'60DCA9C9-9C1A-4F50-B255-EF0EB1414779',
			@searchListID nvarchar(150) = N'747530a1-be80-4054-a021-d2a599248261', --8D689026-E3B8-4D0E-8CBD-AC640039783C (OG)
			@lookupSearchListID nvarchar(150) = N'875b6cfb-b52f-45a0-bf30-9f1992ab28ae', --6C1DA9A2-766E-4B02-8878-5F5AF7EC527E (OG)
			@batchTemplateID UNIQUEIDENTIFIER = null,
			@TEMPLATEID UNIQUEIDENTIFIER,
			@ADDINSTANCEID UNIQUEIDENTIFIER,
			@EDITINSTANCEID UNIQUEIDENTIFIER,
			@FORMDEFINITION XML = NULL

	set @batchTemplateID = (select top 1 ID from @batchTemplates)
  
	SELECT @TEMPLATEID = ID,
		   @ADDINSTANCEID = ADDROWDATAFORMINSTANCEID,
		   @EDITINSTANCEID = EDITROWDATAFORMINSTANCEID,
		   @FORMDEFINITION = FORMDEFINITIONXML
	FROM BATCHTEMPLATE WHERE id = @batchTemplateID

	declare @ADDFORM xml = (
		SELECT  FORMUIXML FROM DATAFORMINSTANCECATALOG A
		WHERE ID = @ADDINSTANCEID
	)

	declare @EDITFORM xml = (
		SELECT FORMUIXML FROM DATAFORMINSTANCECATALOG
		WHERE ID = @EDITINSTANCEID
	)

	DECLARE @t TABLE (
		rowId INT IDENTITY PRIMARY KEY, 
		specID uniqueidentifier, 
		xmlData XML 
	)

	INSERT INTO @t ( specID, xmlData ) VALUES ( @TEMPLATEID, @FORMDEFINITION )
	INSERT INTO @t ( specID, xmlData ) VALUES ( @ADDINSTANCEID, @ADDFORM)
	INSERT INTO @t ( specID, xmlData ) VALUES ( @EDITINSTANCEID, @EDITFORM)

	UPDATE @t SET 
	xmlData.modify('declare default element namespace "bb_appfx_commontypes"; replace value of (/FormMetaData/FormFields/FormField[@FieldID=("CONSTITUENTID")]/SearchList/@SearchListID)[1] with "8D689026-E3B8-4D0E-8CBD-AC640039783C"')


	UPDATE @t SET 
	xmlData.modify('declare default element namespace "bb_appfx_commontypes"; replace value of (/FormMetaData/FormFields/FormField[@FieldID=("CONSTITUENTLOOKUPID")]/SearchList/@SearchListID)[1] with "6C1DA9A2-766E-4B02-8878-5F5AF7EC527E"')

	update BATCHTEMPLATE SET
		FORMDEFINITIONXML = (select xmldata from @t where rowId = 1)
	where id = @TEMPLATEID

	UPDATE DATAFORMINSTANCECATALOG SET 
		FORMUIXML = (select xmldata from @t where rowId = 2)
	WHERE ID = @ADDINSTANCEID

	UPDATE DATAFORMINSTANCECATALOG SET 
		FORMUIXML = (select xmldata from @t where rowId = 3)
	WHERE ID = @EDITINSTANCEID

	UPDATE DATAFORMTEMPLATECATALOG SET 
		FORMDEFINITIONXML = (select xmldata from @t where rowId = 3)
	WHERE ID = @EDITINSTANCEID

	delete from @t
	delete from @batchTemplates where id = @batchTemplateID
END

ROLLBACK

