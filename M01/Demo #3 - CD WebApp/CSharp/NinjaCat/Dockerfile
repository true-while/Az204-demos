#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:3.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:3.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["NinjaCat/NinjaCat.csproj", "NinjaCat/"]
RUN dotnet restore "NinjaCat/NinjaCat.csproj"
COPY . .
WORKDIR "/src/NinjaCat"
RUN dotnet build "NinjaCat.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NinjaCat.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NinjaCat.dll"]