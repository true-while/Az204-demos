 function(playerId1, playerId2) {

        var context = getContext();
        var collection = context.getCollection();
        var response = context.getResponse();

        var player1Document, player2Document;

        // query for players
        var filterQuery = 'SELECT * FROM Players p where p.id  = "' + playerId1 + '"';
        var accept = collection.queryDocuments(collection.getSelfLink(), filterQuery, {},
            function(err, documents, responseOptions) {
                if (err) throw new Error("Error" + err.message);

                if (documents.length != 1) throw "Unable to find both names";
                player1Document = documents[0];

                var filterQuery2 = 'SELECT * FROM Players p where p.id = "' + playerId2 + '"';
                var accept2 = collection.queryDocuments(collection.getSelfLink(), filterQuery2, {},
                    function(err2, documents2, responseOptions2) {
                        if (err2) throw new Error("Error" + err2.message);
                        if (documents2.length != 1) throw "Unable to find both names";
                        player2Document = documents2[0];
                        swapItems(player1Document, player2Document);
                        return;
                    });
                if (!accept2) throw "Unable to read player details, abort ";
            });

        if (!accept) throw "Unable to read player details, abort ";

        function swapItems(player1, player2) {

            //Player1 == Money => Player2
            //Player1 <= Items == Player2


            var player2ItemMoney = 0;
            player2.Items.forEach(function(entry) {
                player2ItemMoney += entry.Price;
            });
            player2Items = player2.Items;
            player1Money = player1.Money; 


            //Do update Palyer 2
            player2.Items = [];
            player2.Money += player2ItemMoney;

            //save Player 1
            var accept2 = collection.replaceDocument(player2._self, player2,
                function(err2, docReplaced) {
                    if (err2) throw "Unable to update player 2, abort ";

                    //Do update Player 1
                    player1.Money -= player2ItemMoney;
                    player1.Items = player1.Items.concat(player2Items);

                    //save Player 2
                    var accept1 = collection.replaceDocument(player1._self, player1,
                        function(err1, docReplaced2) {
                            if (err1) throw "Unable to update player 1, abort";
                        });

                    if (!accept1) throw "Unable to update player 2, abort";
                });

            if (!accept2) throw "Unable to update player 1, abort";

            //suddenly find out if the player1 has enough money, abort by throw exception
            if (player1Money < player2ItemMoney)
                throw new Error("Player " + player1.id + " does not have enough money to buy " + player2.id + "'s stuff, abort...");
            
        }
    }
