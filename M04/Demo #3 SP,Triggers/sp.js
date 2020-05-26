function GetArtifactsType(prefix) {
    var collection = getContext().getCollection();

    collection.queryDocuments(
        collection.getSelfLink(),
        'SELECT VALUE m.Artifacts FROM Missions m',
        function (err, feed, options) {
            if (err) throw err;

            var artTypes = {};
            if (!feed || !feed.length || !feed[0].length)
                getContext().getResponse().setBody('no artifacts found');
            else
            {
                var mission = feed[0][0]["Mission"]
                for(i=0; i<feed[0].length; i++)
                {
                    var key = feed[0][i]["Sample Type"];
                    if (artTypes[key]!=null )  artTypes[key]++; else artTypes[key] =1
                }
             getContext().getResponse().setBody(JSON.stringify({"Mission" : mission, "Artifacts" : artTypes}));
            }
        });
}
