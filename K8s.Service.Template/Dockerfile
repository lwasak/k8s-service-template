FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
ARG VSS_NUGET_EXTERNAL_FEED_ENDPOINTS
COPY . .
RUN curl https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | sh  \
    && dotnet publish "K8s.Service.Template.sln" -c Release -o /app/publish
    
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "K8s.Service.Template.dll"]
