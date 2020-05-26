"use strict";

const Gremlin = require('gremlin');
const config = require("./config");

const authenticator = new Gremlin.driver.auth.PlainTextSaslAuthenticator(`/dbs/${config.database}/colls/${config.collection}`, config.primaryKey)

const client = new Gremlin.driver.Client(
    config.endpoint, 
    { 
        authenticator,
        traversalsource : "g",
        rejectUnauthorized : true,
        mimeType : "application/vnd.gremlin-v2.0+json"
    }
);


function dropGraph()
{
    console.log('Running Drop');
    return client.submit('g.V().drop()', { }).then(function (result) {
        console.log("Result: %s\n", JSON.stringify(result));
    });
}

function addVertex1()
{
    console.log('Running Add Vertex1'); 
    return client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('age', age).property('userid', userid).property('pk', 'pk')", {
            label:"person",
            id:"thomas",
            firstName:"Thomas",
            age:44, userid: 1
        }).then(function (result) {
                console.log("Result: %s\n", JSON.stringify(result));
        });
    }

async function addVertex2()
{
    console.log('Running Add Vertex2');
        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('title', title).property('img', img)", { 
            label:"person",
            id:"Satya Nadella",
            firstName:"Satya",
            lastName: "Nadella",
            title: "CEO",
            img: "https://cdn.theorg.com/05f6d71c-65ba-4103-99d7-1abe2a536182_small.png"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Arne Sorenson",
            firstName:"Arne",
            lastName: "Sorenson",
            title: "Board Member",
            img: "https://cdn.theorg.com/6e526db1-a501-4e2c-93d7-751c12d87258_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Charles W. Scharf",
            firstName:"Charles",
            lastName: "W. Scharf",
            title: "Board Member",
            img: "https://cdn.theorg.com/69781732-bb20-4f52-b409-c4a215eeb034_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Emma Walmsley",
            firstName:"AEmmarne",
            lastName: "Walmsley",
            title: "Board Member",
            img: "https://cdn.theorg.com/9afb59cd-bd8d-48f7-a129-8fa4c33adc05_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"John W. Thompson",
            firstName:"John",
            lastName: "W. Thompson",
            title: "Board Member",
            img: "https://cdn.theorg.com/391e746c-cea1-4455-86b3-cde481062af4_small.jpg"
        })
        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Hugh Johnston",
            firstName:"Hugh",
            lastName: "Johnston",
            title: "Board Member",
            img: "https://cdn.theorg.com/c5cfabde-9f26-404c-ad13-0d1c3d2ebb3e_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Padmasree Warrior",
            firstName:"Padmasree",
            lastName: "Warrior",
            title: "Board Member",
            img: "https://cdn.theorg.com/a553fa9c-d258-4194-9573-8eeb58552364_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Penny Pritzker",
            firstName:"Penny",
            lastName: "Pritzker",
            title: "Board Member",
            img: "https://cdn.theorg.com/08787d39-8daf-4bbe-b8a9-c007005764de_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Reid Hoffman",
            firstName:"Reid",
            lastName: "Hoffman",
            title: "Board Member",
            img: "https://cdn.theorg.com/01e46f0f-5a47-4b76-874c-1990acf6ee5c_small.jpg"
        })
        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Sandra E. Peterson",
            firstName:"Sandra",
            lastName: "E. Peterson",
            title: "Board Member",
            img: "https://cdn.theorg.com/651f5e6b-592c-421f-a253-611094c68243_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Teri List-Stoll",
            firstName:"Teri",
            lastName: "List-Stoll",
            title: "Board Member",
            img: "https://cdn.theorg.com/d05fda68-907d-4d90-8099-94f22ae1b507_small.jpg"
        })



        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Judson Althoff",
            firstName:"Judson",
            lastName: "Althoff",
            title: "EVP Worldwide Commercial Business",
            img: "https://cdn.theorg.com/858edbb1-c7b5-4d66-b7a0-9cb4c79239e5_small.png"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Kate Johnson",
            firstName:"Kate",
            lastName: "Johnson",
            title: "President & Corporate Vice President",
            img: "https://cdn.theorg.com/ba06c6f1-ddf6-4c44-9abb-28e2f4c07523_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Gavriella Schuster",
            firstName:"Gavriella",
            lastName: "Schuster",
            title: "Corporate Vice President, One Commercial Partner Business",
            img: "https://cdn.theorg.com/ad67d46a-af79-4d31-814f-48a6da156b32_small.jpg"
        })


        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Corey Sanders",
            firstName:"Corey",
            lastName: "Sanders",
            title: "Corporate Vice President, Microsoft Solutions",
            img: "https://cdn.theorg.com/d4acb6cc-c30f-4b8b-be2c-1a348afd7e7c_small.jpg"
        })
        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Chris Weber",
            firstName:"Chris",
            lastName: "Weber",
            title: "Corporate Vice President, Small, Medium, & Corporate, Worldwide Commercial",
            img: "https://cdn.theorg.com/fb21d0f8-9397-45e7-bef1-3dc9e51d1fc8_small.jpg"
        })
        
        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Scott Guthrie",
            firstName:"Scott",
            lastName: "Guthrie",
            title: "EVP, Microsoft Cloud + AI",
            img: "https://cdn.theorg.com/0ef123d5-a8df-48b3-b1e6-6bde14728fbd_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Rajesh Jha",
            firstName:"Rajesh",
            lastName: "Jha",
            title: "EVP Office Product Group",
            img: "https://cdn.theorg.com/503255ae-76ce-4cc1-8f34-d2db4556d4b4_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Brad Smith",
            firstName:"Brad",
            lastName: "Smith",
            title: "President & Chief Legal Officer",
            img: "https://cdn.theorg.com/503255ae-76ce-4cc1-8f34-d2db4556d4b4_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Matt Renner",
            firstName:"Matt",
            lastName: "Renner",
            title: "President, US Enterprise",
            img: "https://cdn.theorg.com/5e91ede2-801b-426f-8b90-a2e279580bfe_small.jpg"
        })
        
        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Anthony Salcito",
            firstName:"Anthony",
            lastName: "Salcito",
            title: "Vice President, Worldwide Education & Public Sector",
            img: "https://cdn.theorg.com/9606c9e2-9d89-4390-8003-4c42628b815f_small.jpg"
        })

        await client.submit("g.addV(label).property('id', id).property('firstName', firstName).property('lastName', lastName).property('Title', title).property('img', img)", { 
            label:"person",
            id:"Toni Townes-Whitley",
            firstName:"Toni",
            lastName: "Townes-Whitley",
            title: "President, US Regulated Industries",
            img: "https://cdn.theorg.com/9606c9e2-9d89-4390-8003-4c42628b815f_small.jpg"
        })


        

     
    }

async function addEdge() {

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Kate Johnson",
        relationship: "manage",
        target: "Matt Renner"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Kate Johnson",
        relationship: "manage",
        target: "Toni Townes-Whitley"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Toni Townes-Whitley",
        relationship: "manage",
        target: "Anthony Salcito"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Judson Althoff",
        relationship: "manage",
        target: "Chris Weber"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Judson Althoff",
        relationship: "manage",
        target: "Corey Sanders"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Rajesh Jha"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Brad Smith"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Judson Althoff"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Arne Sorenson"
    })
    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Charles W. Scharf"
    })
    
    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Emma Walmsley"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Hugh Johnston"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "John W. Stanton"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "John W. Thompson"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Padmasree Warrior"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Penny Pritzker"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Reid Hoffman"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Sandra E. Peterson"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Satya Nadella",
        relationship: "manage",
        target: "Teri List-Stoll"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Judson Althoff",
        relationship: "manage",
        target: "Kate Johnson"
    })

    await client.submit("g.V(source).addE(relationship).to(g.V(target))", {
        source: "Kate Johnson",
        relationship: "manage",
        target: "Gavriella Schuster"
    })

    
}


function countVertices()
{
    console.log('Running Count');
    return client.submit("g.V().count()", { }).then(function (result) {
        console.log("Result: %s\n", JSON.stringify(result));
    });
}

function finish()
{
    console.log("Finished");
    console.log('Press any key to exit');
    
    process.stdin.resume();
    process.stdin.on('data', process.exit.bind(process, 0));
}

client.open()
    .then(dropGraph)
    .then(addVertex1)
    .then(addVertex2)
    .then(addEdge)
    .then(countVertices)
    .catch((err) => {
        console.error("Error running query...");
        console.error(err)
    }).then((res) => {
        client.close();
        finish();
    }).catch((err) => 
        console.error("Fatal error:", err)
    );
    
    

