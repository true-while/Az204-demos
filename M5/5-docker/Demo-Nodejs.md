# AZ-204 Demo: Dockerfile and a building docker image.

In the demo you will create new image from web app project and push it in ACR

## Technical Requirements:

- Install Docker Desktop
- Visual Studio Core
- Node 16

## Demonstration:

1. Docker Desktop must be run.

1. Open in VS Code **Nodejs** folder with web site.

1. Restore packages by command **npm instal**

1. Run the web site locally by use play button or run command  **node .\server.js**. 

1. Visit http://localhost:9090 url to make sure that web app works.

![localhost](localhost.png)

1. In VS Code open `DockerFile`.

1. Open **run.azcli** to build image. Run line by line to build and tag the image.

1. Run the docker image locally to observe the result.

![local docker image](localdocker.png)

1. Run the command from script to push the image in ACR you previously deploy. 