# AZ-204 Demo: Creating an Azure Cosmos DB account

In this demo you will create new Cosmos DB account and upload initial data. Then you run some query and scale performance.

## Before delivery:

- Prepare Cosmos DB New collection `Apollo` and DB `Missions`. Chose `/Operator` as partition Name
- Import Apollo files from folder `Apollo`
- Create Stored Proc from **sp.js** file
- Create UDF from **udf.js** file
- Create Pre Insert/Update Trigger from **trigger.js** file.

## In class:

1. From creation a new Cosmos DB account demonstrate how select different APIs
3. How Create collection and database.
4. Observe existed collection and partition settings for collection Apollo. 
5. Demonstrate how to create a new Document.

## Query demo

1. Open Cosmos DB
2. Open new Query, run query and demonstrate statics (RU)

**SELECT\*FROM c**

1. Run updated query and demonstrate statics (RU)

**SELECT c.id,c.Operator,c.Crew.Members FROM c**

![Statistics](stat.png)

1. Run few following queries and demonstrate results: 

- Simple query to any json document. Table name is up to you or can be missed.

**SELECT \* FROM Apollos m**

- Select all Apollo mission by id

**SELECT \* FROM Missions m where m.id = &#39;Apollo 10&#39;**

- Like query is not supported by you can use CONTAINS or STARTWITH

**SELECT \* FROM Missions m where CONTAINS(m.id,&#39;Apollo&#39;)**

- In the same we can use &#39;IN&#39;

**SELECT \* FROM Missions m WHERE m.id IN (&#39;Apollo 11&#39;, &#39;Apollo 17&#39;)**

- OR even CONTAINS in Array Type fields Like Crew=\&gt;[Members..]

**SELECT \* FROM Missions m where ARRAY\_CONTAINS(m.Crew.Members,&quot;Neil A. Armstrong&quot;)**

- You can query more than 1 level of the data and takes the fields with space in the brackets

**SELECT \* FROM Missions m WHERE ARRAY\_CONTAINS(m[&quot;Spacecraft properties&quot;].Spacecraft , &quot;Apollo LM-6&quot;)**

- You can also modify output data. Following query will give us all missions crew members.

**SELECT m.id,m.Crew.Members FROM Missions m**

- Or format output in to another json. Value – to get just resulted properties without additional fields

**SELECT VALUE {&quot;Mission Name&quot;: m.id, &quot;Commander&quot; : m.Crew.Members[0], &quot;Module Pilot&quot; : m.Crew.Members[1]} FROM Missions m**

- You also can use function to calculate length of array.

- Following example will calculate amount of useful artifacts by missions.

**SELECT VALUE {&quot;Mission Name&quot;: m.id, &quot;Artifacts count&quot; : ARRAY\_LENGTH(m.Artifacts)} FROM Missions m**

- Using joins is quite different from SQL. We can join the document to itself to change nesting levels. For example, following list will provide all crew members by mission in flat format. Sub query and cross document joins is not supported.

**SELECT m.id, memeber FROM Missions m JOIN memeber IN m.Crew.Members**

- You can also use TOP and ORDER criteria. The order criteria work with string/number not with dates that is why result is sorted A-Z

**SELECT TOP 5 m.id,m[&quot;Start of mission&quot;][&quot;Launch date&quot;] FROM m ORDER BY m[&quot;Start of mission&quot;][&quot;Launch date&quot;]**